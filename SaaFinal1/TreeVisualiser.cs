using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SaaFinal1
{
    internal class TreeVisualiser
    {
        public static void TreeVisualization(HTMLNode node, int depth)
        {
            if (node != null)
            {
                string tabs = Helpers.NumberOfTabluations(depth);
                
                if (node.Type == "open")
                {
                    Console.WriteLine($"{tabs}<{node.TagName}>");
                    
                }
                else if (node.Type == "selfClosing")
                {
                    Console.WriteLine($"{tabs}<{node.TagName}>");
                }
                else if (node.Type == "text")
                {
                    // Извеждаме текстовия възел
                    Console.WriteLine($"{tabs} {node.TagName}");
                }
                else if (node.Type == "close")
                {
                    // затваряме таг
                    Console.WriteLine($"{tabs}<{node.TagName}>");
                }

                // визуализираме децата на текущия възел
                foreach (HTMLNode child in node.ChildrenList)
                {
                    TreeVisualization(child, depth + 1);
                }
            }
        }
    }
}
