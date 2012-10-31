using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace McLauncher2
{
    public class Target : DirectoryTreeNode
    {
        public Target(string path, string name):base(path, name)
        {
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

        public bool RunMinecraft()
        {            
            return false;
        }
    }

    public class DirectoryTreeNode
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public List<DirectoryTreeNode> Children { get; set; }

        public DirectoryTreeNode(string path, string name)
        {
            this.Path = path;
            this.Name = name;
            this.Children = new List<DirectoryTreeNode>();
            SearchChildren();
        }

        

        public void SearchChildren()
        {
            this.Children.Clear();
            var entries = Directory.GetFileSystemEntries(this.Path);
            foreach (var entry in entries)
            {
                if (Directory.Exists(entry))
                {
                    Children.Add(new DirectoryTreeNode(entry, System.IO.Path.GetFileName(entry)));
                }
            }
        }
    }
}
