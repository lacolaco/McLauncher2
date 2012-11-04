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
                if(!Directory.Exists(Directory.GetParent(exePath).FullName))
                {
                    Directory.CreateDirectory(Directory.GetParent(exePath).FullName);
                }
                System.Net.WebClient wc = new System.Net.WebClient();
                wc.DownloadFile(@"https://s3.amazonaws.com/MinecraftDownload/launcher/Minecraft.exe", exePath);
                wc.Dispose();
            }
            string batPath = Environment.CurrentDirectory + @"\emb\run.bat";
            var customEnabled = Properties.Settings.Default.UseCustom && File.Exists(Environment.CurrentDirectory + @"\emb\custom.txt");
            File.WriteAllText(batPath, GenerateScript(customEnabled),Encoding.GetEncoding("Shift-JIS"));
            var p = new Process();
            p.StartInfo.FileName = batPath;
            if(!Properties.Settings.Default.LogEnabled)
            {
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            }
            p.Start();
        }

        private string GenerateScript(bool useCustom = false)
        {
            var builder = new StringBuilder(useCustom ? File.ReadAllText(Environment.CurrentDirectory + @"\emb\custom.txt",Encoding.Default) : BatTemplate);
            builder.Replace("{target}", target.Path);
            builder.Replace("{exepath}",  "\"" + exePath + "\"");
            builder.Replace("{noupdate}", Properties.Settings.Default.NoUpdate ? "--noupdate" : "");
            builder.Replace("{log}", Properties.Settings.Default.LogEnabled ? "pause" : "");
            return builder.ToString();
        }

        public static string BatTemplate =
@"cd /d {target}
setlocal
set APPDATA={target}
java -jar {exepath} {noupdate}
{log}";
    }
}
