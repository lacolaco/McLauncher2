using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ConfigEditor
{
    public class ForgeConfig : Config
    {

        public ForgeConfig(string configDir, string name) : base(configDir, name)
        {
        }

        public override void ReadFile()
        {
            this.Items.Clear();
            var lines = File.ReadAllLines(this.ConfigDir);
            bool inBlock = false;
            string blockName = "";
            for(int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim(); ;
                if(line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                    continue;
                if(line == "}")
                {
                    inBlock = false;
                    blockName = "";
                    continue;
                }
                if(line.EndsWith("{"))
                {
                    inBlock = true;
                    blockName = line.Replace("{", "").Trim();
                    continue;
                }
                Item item = new Item(this, line.Split('=')[0], line.Split('=')[1]);
                if(inBlock)
                {
                    item.Category = blockName;
                }
                if(i != 0 && lines[i - 1].Trim().StartsWith("#"))
                {
                    item.Info = lines[i - 1].Replace("#", "").Trim();
                }
                if(Regex.IsMatch(item.Name, @".*(\.id)$") || Regex.IsMatch(item.Value, @"[0-9]+"))
                {
                    item.Type = "integer";
                }
                else if(item.Value == "true" || item.Value == "false")
                {
                    item.Type = "boolean";
                }
                else
                {
                    item.Type = "string";
                }
                this.Items.Add(item.Name, item);
            }
        }

        public override void WriteFile()
        {
            var lines = File.ReadAllLines(this.ConfigDir);
            var writer = new StreamWriter(this.ConfigDir, false);
            for(int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (string.IsNullOrWhiteSpace(line) || line.Trim().StartsWith("#") || line.Contains("{") || line.Contains("}"))
                {
                    writer.WriteLine(line);
                    continue;
                }
                foreach(var item in this.Items.Values)
                {
                    if(line.Contains(item.Name))
                    {
                        var newLine = item.Name + "=" + item.Value;
                        if(newLine != line.Trim())
                        {
                            line = "   " + newLine;
                        }
                    }
                }
                writer.WriteLine(line);
            }
            writer.Close();
        }
    }
}
