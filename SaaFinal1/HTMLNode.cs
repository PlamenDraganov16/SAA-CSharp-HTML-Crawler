using System;
using System.Collections.Generic;
using System.Text;

namespace SaaFinal1
{
    internal class HTMLNode
    {
        public string Type { get; set; }

        public string TagName { get; set; }

        public int NumberOfParents { get; set; }

        public string Argument { get; set; }

        public HTMLNode Parent { get; set; }

        public List<HTMLNode> ChildrenList { get; set; }

        public int NumberOfNode { get; set; }

        public HTMLNode(string tagName, string nodeType, int parentCount, string tagArguments, HTMLNode parent, List<HTMLNode> children, int nodeNumber)
        {
            TagName = tagName;
            Type = nodeType;
            NumberOfParents = parentCount;
            Argument = tagArguments;
            Parent = parent;
            ChildrenList = children ?? new List<HTMLNode>();
            
            NumberOfNode = nodeNumber;
        }
    }
}
