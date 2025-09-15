using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;

namespace XenositeFramework.Editor
{
    public class GeminiCmdClient
    {
        private readonly string _geminiCmdPath;

        public GeminiCmdClient(string geminiCmdPath)
        {
            if (!File.Exists(geminiCmdPath))
            {
                UnityEngine.Debug.LogError("指定されたgemini.cmdが見つかりません。");
            }

            _geminiCmdPath = geminiCmdPath;
        }

        public async UniTask<string> SendPromptAsync(string prompt, string modelName, int timeoutMs = 20000)
        {
            var psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{_geminiCmdPath}\" --model {modelName}",
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
                StandardInputEncoding = Encoding.UTF8,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            };

            using var process = new Process { StartInfo = psi };
            process.Start();

            await process.StandardInput.WriteLineAsync(prompt);
            process.StandardInput.Close();

            // タイムアウト処理
            if (!process.WaitForExit(timeoutMs))
            {
                process.Kill();
                throw new TimeoutException($"Gemini CLI process timed out after {timeoutMs / 1000} seconds.");
            }

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            if (!string.IsNullOrEmpty(error))
                throw new InvalidOperationException($"Gemini CLI error: {error}");

            return output.Trim();
        }

    }

    public static class ProcessExtensions
    {
        public static UniTask WaitForExitAsync(Process process)
        {
            var tcs = new UniTaskCompletionSource();

            process.EnableRaisingEvents = true;
            process.Exited += (_, __) => tcs.TrySetResult();

            if (process.HasExited) // すでに終了していたら即完了
                tcs.TrySetResult();

            return tcs.Task;
        }
    }
}
