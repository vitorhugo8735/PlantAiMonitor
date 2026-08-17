using System.Diagnostics;
using System.Text;
using PlantDiseaseAI.Api.AI.Interfaces;
using PlantDiseaseAI.Api.AI.Models;
using PlantDiseaseAI.Api.Common.Exceptions;

namespace PlantDiseaseAI.Api.AI.Inference;

/// <summary>
/// Executes Python scripts and captures their output.
/// </summary>
public sealed class PythonProcessExecutor : IPythonProcessExecutor
{
    private readonly ILogger<PythonProcessExecutor> _logger;

    public PythonProcessExecutor(ILogger<PythonProcessExecutor> logger)
    {
        _logger = logger;
    }

    public async Task<PythonProcessResult> ExecuteAsync(
        PythonProcessRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(request.FileName);
        ArgumentException.ThrowIfNullOrWhiteSpace(request.WorkingDirectory);

        var startInfo = new ProcessStartInfo
        {
            FileName = request.FileName,
            WorkingDirectory = request.WorkingDirectory,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
            StandardOutputEncoding = Encoding.UTF8,
            StandardErrorEncoding = Encoding.UTF8
        };

        foreach (var argument in request.Arguments)
        {
            startInfo.ArgumentList.Add(argument);
        }

        using var process = new Process { StartInfo = startInfo, EnableRaisingEvents = true };

        try
        {
            if (!process.Start())
            {
                throw new InferenceException("Failed to start the Python inference process.");
            }
        }
        catch (Exception exception) when (exception is not InferenceException)
        {
            throw new InferenceException("Failed to start the Python inference process.", exception);
        }

        var stdoutTask = process.StandardOutput.ReadToEndAsync();
        var stderrTask = process.StandardError.ReadToEndAsync();

        using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        if (request.Timeout > TimeSpan.Zero)
        {
            timeoutCts.CancelAfter(request.Timeout);
        }

        try
        {
            await process.WaitForExitAsync(timeoutCts.Token);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            TryKill(process);
            throw new InferenceTimeoutException(
                $"Python inference timed out after {request.Timeout.TotalSeconds:F0} seconds.");
        }
        catch (OperationCanceledException)
        {
            TryKill(process);
            throw;
        }

        var standardOutput = await stdoutTask;
        var standardError = await stderrTask;

        _logger.LogDebug(
            "Python process finished with exit code {ExitCode}. Stdout length: {StdoutLength}. Stderr length: {StderrLength}.",
            process.ExitCode,
            standardOutput.Length,
            standardError.Length);

        if (process.ExitCode != 0)
        {
            var errorMessage = string.IsNullOrWhiteSpace(standardError)
                ? "The Python inference process exited with an error."
                : standardError.Trim();

            _logger.LogError(
                "Python inference failed with exit code {ExitCode}. Stderr: {Stderr}",
                process.ExitCode,
                errorMessage);

            throw new InferenceException(
                $"Python inference process failed with exit code {process.ExitCode}.",
                new InvalidOperationException(errorMessage));
        }

        return new PythonProcessResult
        {
            ExitCode = process.ExitCode,
            StandardOutput = standardOutput,
            StandardError = standardError
        };
    }

    private static void TryKill(Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill(entireProcessTree: true);
            }
        }
        catch
        {
        }
    }
}