using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ConfigEditor
{
    public class RPConfig : Config
    {
        public RPConfig(string configDir) : base(configDir, "redpower.cfg")
        {            
        }

        public override void ReadFile()
        {
            this.Items.Clear();
            var lines = File.ReadAllLines(this.ConfigDir);
            Stack<string> blockName = new Stack<string>();
            List<string> commentBuffer = new List<string>();
            for(int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }                
                if(line.Trim().StartsWith("#"))
                {
                    commentBuffer.Add(line.Trim());
                    continue;
                }
                if(line == "}")
                {
                    blockName.Pop();
                    commentBuffer.Clear();
                    continue;
                }
                if(line.EndsWith("{"))
                {
                    blockName.Push(line.Replace("{", "").Trim());
                    commentBuffer.Clear();
                    continue;                    
                }
                Item item = new Item(this, line.Trim().Split('=')[0], line.Trim().Split('=')[1]);
                item.SimpleName = line.Trim().Split('=')[0];
                if(blockName.Count > 0)
                {
                    var category = new StringBuilder();
                    for (int j = 0; j < blockName.Count; j++)
                    {
                        var block = blockName.Reverse().ToArray()[j];
                        if(j == 0)
                        {
                            item.Category = block;
                        }
                        else
                        {
                            category.Append("/");
                            category.Append(block);
                        }
                    }
                    item.Name = category.ToString() + "/" + item.SimpleName;
                    if(item.Name.StartsWith("/"))
                    {
                        item.Name = item.Name.Remove(0, 1);
                    }
                }
                if(commentBuffer.Count > 0)
                {
                    var builder = new StringBuilder();
                    foreach(var comment in commentBuffer)
                    {
                        builder.Append(comment + "\n");
                    }
                    item.Info = builder.ToString();
                    commentBuffer.Clear();
                }
                if (Regex.IsMatch(item.Name, @".*(\.id)$") || Regex.IsMatch(item.Value, @"[0-9]+"))
                {
                    item.Type = "integer";
                }
                else if (item.Value == "true" || item.Value == "false")
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
                    if(line.Contains(item.SimpleName))
                    {
                        var newLine = item.SimpleName + "=" + item.Value;
                        if(newLine != line.Trim())
                        {
                            var level = CountChar(item.Name, '/') + 1;
                            line = newLine;
                            for(int j = 0; j < level; j++)
                            {
                                line = "    " + line;
                            }                            
                        }
                    }
                }
                writer.WriteLine(line);
            }
            writer.Close();
        }

        private int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }
    }
}
