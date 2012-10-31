using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace ConfigEditor
{
    public abstract class Config
    {
        public string ConfigDir;
        public string Name;
        public Dictionary<string, Item> Items;

        public Config(string configDir, string name)
        {
            this.ConfigDir = configDir;
            this.Name = name;
            this.Items = new Dictionary<string, Item>();
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj is Config)
            {
                return (obj as Config).Name == this.Name;
            }
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public abstract void ReadFile();
        
        public abstract void WriteFile();
    }

}
