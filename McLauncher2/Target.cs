using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace McLauncher2
{
    public class Target
    {
        public string Path;
        public string Name;
        public List<string> ChildFolders;
        
        public Target(string path, string name)
        {
            this.Path = path;
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
