using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace ConfigEditor
{
    public class MLPropConfig : Config
    {        
        public MLPropConfig(string configDir, string name) : base(configDir, name)
        {            
        }

        public override void ReadFile()
        {
            var commentLines = new List<string>();
            var itemLines = new List<string>();
            var lines = File.ReadAllLines(this.ConfigDir);
            foreach(var line in lines)
            {
                if(string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                if(line.Trim().StartsWith("#"))
                {
                    commentLines.Add(line);
                }
                else
                { 
                    itemLines.Add(line);   
                }
            }
            this.Items.Clear();
            foreach (var itemLine in itemLines)
            {
                Item item = new Item(this, itemLine.Split('=')[0], itemLine.Split('=')[1]);
                foreach (var commentLine in commentLines)
                {
                    if (commentLine.Contains(item.Name))
                    {
                        var comment = commentLine.Replace("MLProp : ", "");
                        var pattern = @"^#[ ]*(?<name>.+)[ ]*[(](?<type>.+?):(?<detail>.*?)[)](?: -- )?(?<info>.*)$";
                        var re = new Regex(pattern);
                        var m = re.Match(comment);
                        item.Info = m.Groups["info"].Success ? m.Groups["info"].Value : "";
                        if (m.Groups["type"].Success)
                        {
                            var type_ = m.Groups["type"].Value.ToLower();
                            if(type_.Contains("string"))
                            {
                                item.Type = "string";
                                item.DefaultValue = m.Groups["detail"].Success ? m.Groups["detail"].Value : "";
                            }
                            else
                            {
                                item.Type = type_;
                                if(m.Groups["detail"].Success)
                                {
                                    var detail_ = m.Groups["detail"].Value.Split(',');
                                    item.DefaultValue = detail_[0];
                                    foreach(var data in detail_)
                                    {
                                        if(data.Contains(">="))
                                        {
                                            item.MinValue = data.Replace(">=", "");
                                        }
                                        if(data.Contains("<="))
                                        {
                                            item.MaxValue = data.Replace("<=", "");
                                        }
                                    }
                                }
                            }
                        }                        
                    }
                }
                this.Items.Add(item.Name, item);
            }
        }

        public override void WriteFile()
        {
            var lines = File.ReadAllLines(this.ConfigDir);
            var writer = new StreamWriter(this.ConfigDir, false);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (!line.StartsWith("#"))
                {
                    foreach (var item in this.Items.Values)
                    {
                        if (line.Contains(item.Name))
                        {
                            var newLine = item.Name + "=" + item.Value;
                            if (newLine != line)
                            {
                                line = newLine;
                            }
                        }
                    }
                }
                writer.WriteLine(line);
            }
            writer.Close();
        }
    }
}
