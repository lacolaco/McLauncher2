using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace McLauncher2
{
    public class RunManager
    {
        private string exePath;
        private Target target;
        

        public RunManager(string exePath, Target target)
        {
            this.exePath = exePath;
            this.target = target;
        }

        public void RunMinecraft()
        {
            if (!File.Exists(exePath))
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.DownloadFile(@"https://s3.amazonaws.com/MinecraftDownload/launcher/Minecraft.exe", exePath);
                wc.Dispose();
            }
            var batPath = Environment.CurrentDirectory + @"\emb\run.bat";

            File.WriteAllText(batPath, GenerateScript());
            var p = new Process();
            p.StartInfo.FileName = batPath;
            if(!Properties.Settings.Default.LogEnabled)
            {
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            p.Start();
        }

        private string GenerateScript()
        {
            var builder = new StringBuilder(template);
            builder.Replace("{target}", Directory.GetParent(target.Path).FullName);
            builder.Replace("{exepath}",  "\"" + exePath + "\"");
            builder.Replace("{noupdate}", Properties.Settings.Default.NoUpdate ? "--noupdate" : "");
            builder.Replace("{log}", Properties.Settings.Default.LogEnabled ? "pause" : "");
            return builder.ToString();
        }

        private string template =
@"cd /d {target}
setlocal
set APPDATA={target}
java -jar {exepath} {noupdate}
{log}";
    }
}
