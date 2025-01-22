using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaFinal1
{
    internal class HtmlSearcherFinal
    {
        private HTMLNode root;

        public HtmlSearcherFinal(HTMLNode rootNode)
        {
            root = rootNode;
        }

        public List<HTMLNode> Search(string path)
        {

            //разделя пътя на части
            var pathParts = ParsePath(path);
            var currentNodes = new List<HTMLNode> { root };

            foreach (var part in pathParts)
            {
                if (IsEmpty(part))
                {
                    continue;
                }

                if (Contains(part, "*"))
                {
                    currentNodes = GetChildren(currentNodes);
                }
                else if (Contains(part, "["))
                {
                    int index = ParseIndex(part);
                    currentNodes = GetChildren(currentNodes);
                    if (index >= 0 && index < currentNodes.Count)
                    {
                        currentNodes = new List<HTMLNode> { currentNodes[index] };
                    }
                    else
                    {
                        currentNodes.Clear();
                    }
                }
                else
                {
                    currentNodes = GetChildren(currentNodes);
                    currentNodes = FilterByTag(currentNodes, part);
                }

                if (currentNodes.Count == 0)
                {   //връщаме празен списък
                    return currentNodes;
                }
            }

            return currentNodes;
        }

        private List<HTMLNode> GetChildren(List<HTMLNode> nodes)
        {
            var children = new List<HTMLNode>();
            foreach (var node in nodes)
            {
                children.AddRange(node.ChildrenList);
            }
            return children;
        }

        private List<HTMLNode> FilterByTag(List<HTMLNode> nodes, string tag)
        {
            var filtered = new List<HTMLNode>();
            foreach (var node in nodes)
            {
                if (CompareStrings(node.TagName, tag))
                {
                    filtered.Add(node);
                }
            }
            return filtered;
        }

        //Метод за извличане на индекса
        private int ParseIndex(string part)
        {
            int index = 0;
            string indexStr = ExtractSubstring(part, "[", "]");
            if (indexStr != null)
            {
                index = ConvertToInt(indexStr) - 1;
            }
            return index;
        }
        // Метод за извличане на подстринг между два символа
        private string ExtractSubstring(string str, string start, string end)
        {
            int startIndex = FindSubstringIndex(str, start);
            if (startIndex == -1) return null;

            int endIndex = FindSubstringIndex(str, end, startIndex + start.Length);
            if (endIndex == -1) return null;

            return str.Substring(startIndex + start.Length, endIndex - startIndex - start.Length);
        }
        // Метод за намиране на индекс на подстринг в стринг.
        private int FindSubstringIndex(string str, string sub, int startIndex = 0)
        {
            for (int i = startIndex; i <= str.Length - sub.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < sub.Length; j++)
                {
                    if (str[i + j] != sub[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                {
                    return i;
                }
            }
            return -1;
        }
        // Метод за разделяне на пътя на части според разделителя "/".
        public List<string> ParsePath(string path)
        {
            var parts = new List<string>();
            int currentIndex = 0;

            while (currentIndex < path.Length)
            {
                if (path[currentIndex] == '/')
                {
                    currentIndex++;
                    continue;
                }

                int nextIndex = FindSubstringIndex(path, "/", currentIndex);
                if (nextIndex == -1)
                {
                    parts.Add(path.Substring(currentIndex));
                    break;
                }

                parts.Add(path.Substring(currentIndex, nextIndex - currentIndex));
                currentIndex = nextIndex + 1;
            }

            return parts;
        }

        private bool IsEmpty(string str)
        {
            return str == null || str.Length == 0;
        }
        // Проверява дали даден стринг съдържа подстринг.
        private bool Contains(string str, string sub)
        {
            return FindSubstringIndex(str, sub) != -1;
        }
        // Конвертира стринг в число
        public static int ConvertToInt(string str)
        {
            int result = 0;
            foreach (var ch in str)
            {
                result = result * 10 + (ch - '0'); //48
            }
            return result;
        }
        // Сравнява два стринга символ по символ.
        private bool CompareStrings(string str1, string str2)
        {
            if (str1.Length != str2.Length)
                return false;

            for (int i = 0; i < str1.Length; i++)
            {
                if (str1[i] != str2[i])
                    return false;
            }

            return true;
        }

    }
}
