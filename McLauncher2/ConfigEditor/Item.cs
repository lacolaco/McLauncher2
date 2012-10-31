using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigEditor
{
    public class Item
    {
        public Config Parent { get; private set; }
        public string Name;
        public string Value;
        public string Type;
        public string DefaultValue;
        public string MinValue;
        public string MaxValue;
        public string Info;
        public string Category;
        public string SimpleName;

        public Item(Config parent, string name, string value)
        {
            this.Parent = parent;
            this.Name = name;
            this.Value = value;
            this.Type = "";
            this.DefaultValue = "";
            this.MinValue = "";
            this.MaxValue = "";
            this.Info = "";
            this.Category = "";
            this.SimpleName = "";
        }
    }
}
