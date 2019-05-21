using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NFSPanel.Data {
    public class NFSExportsService {
        public List<NFSMount> Mounts { get; set; } = new List<NFSMount>();

        public NFSExportsService() {
            Reload();
        }

        private static string Exec(string cmd) {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process {
                StartInfo = new ProcessStartInfo {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "bash",
                    Arguments = $"-c \"{escapedArgs}\""
                }
            };
            string stdout = "";
            process.OutputDataReceived += (object sender, DataReceivedEventArgs e) => {
                stdout += e.Data + "\r\n";
            };
            process.ErrorDataReceived += (object sender, DataReceivedEventArgs e) => {
                stdout += e.Data + "\r\n";
            };
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();

            return stdout;
        }

        public void Reload() {
            // 取得NFS Exports設定
            if (!File.Exists(Environment.GetEnvironmentVariable("Exports"))) {
                using (File.Create(Environment.GetEnvironmentVariable("Exports"))) { }
            }

            Mounts = File.ReadAllText(Environment.GetEnvironmentVariable("Exports"))
                .Split('\n') // 每行切割
                .Select(x => x.Trim())
                .Where(x => !x.StartsWith("#")) // 略過註解
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => NFSMount.Parse(x))
                .ToList();
        }

        public void SaveChanges() {
            var output = string.Join("\n", Mounts.Select(x => x.ToString()));
            File.WriteAllText(Environment.GetEnvironmentVariable("Exports"), output);
        }


        public void RestartService() {
            Exec(Environment.GetEnvironmentVariable("NFS") + " restart");
        }
    }
}
