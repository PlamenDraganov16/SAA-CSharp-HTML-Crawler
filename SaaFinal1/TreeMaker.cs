using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaFinal1
{
    internal class TreeMaker
    {
        public static void BuildTree(List<string> elements, ref HTMLNode rootNode)
        {   
            HTMLNode currNode = rootNode;

            List<string> selfClosingTags = new List<string> { "br", "img", "input", "hr", "meta", "link" };

            for (int i = 0; i < elements.Count; i++)
            {
                string element = elements[i]; // Вземаме текущия елемент

                // Проверка дали е отворен таг (например <div>)
                if (element.Length > 1 && element[0] == '<' && element[1] != '/')
                {
                    string tagName = GetTagName(element);

                    if (selfClosingTags.Contains(tagName))
                    {
                        // Добавяме самозатварящ се таг като дете на текущия възел
                        HTMLNode childNode = new HTMLNode(element, "selfClosing", currNode.NumberOfParents + 1, null, currNode, new List<HTMLNode>(), i);
                        currNode.ChildrenList.Add(childNode);
                    }

                    
                    else
                    {
                        HTMLNode childNode = new HTMLNode(element, "open", currNode.NumberOfParents + 1, null, currNode, new List<HTMLNode>(), i);
                        currNode.ChildrenList.Add(childNode);
                        currNode = childNode; // Преминаваме на новия възел
                    }

                }
                
                else if (element.Length > 2 && element[0] == '<' && element[1] == '/')
                {                    
                    HTMLNode childNode = new HTMLNode(element, "close", currNode.NumberOfParents - 1, null, currNode, new List<HTMLNode>(), i);
                    currNode = currNode.Parent;
                }
                
                else
                {
                    // Добавяме текстов възел
                    HTMLNode textNode = new HTMLNode(element, "text", currNode.NumberOfParents + 1, null, currNode, new List<HTMLNode>(), i);
                    currNode.ChildrenList.Add(textNode);
                }
            }
        }
        
        private static string GetTagName(string element)
        {
            string tagName = "";
            bool insideTag = false;
            for (int i = 1; i < element.Length; i++)
            {
                if (element[i] == ' ' || element[i] == '>')
                {
                    break;
                }
                tagName += element[i];
            }
            return tagName;
        }

        
        public static void ArgumentSplitter(ref HTMLNode root)
        {
            string name = null;
            string argument = null;
            int index = 0;

            // Извличаме името на тага
            for (int i = 0; i < root.TagName.Length; i++)
            {
                if (root.TagName[i] != '<')
                {
                    if (root.TagName[i] != ' ' && root.TagName[i] != '>')
                    {
                        name += root.TagName[i];
                        index++;
                    }
                    else
                        break;
                }
            }

            // Извличаме аргументите на тага
            for (int i = index + 1; i < root.TagName.Length; i++)
            {
                if (root.TagName[i] == '>')
                {
                    if (argument != null)
                        root.Argument = argument;
                    break;
                }

                if (root.TagName[i] != ' ')
                    argument += root.TagName[i];
                else
                {
                    if (argument != null)
                    {
                        root.Argument = argument;
                        argument = null;
                    }
                }
            }

            root.TagName = name;

            //обработваме децата на възела
            for (int i = 0; i < root.ChildrenList.Count; i++)
            {
                HTMLNode nodeX = root.ChildrenList[i];
                ArgumentSplitter(ref nodeX);
                root.ChildrenList[i] = nodeX;
            }
        }

        public static List<string> ElementSplitter(string textFromFile)
        {
            List<string> elements = new List<string>();
            string currentElement = null;

            for (int i = 0; i < textFromFile.Length; i++)
            {
                
                if (textFromFile[i] == '<')
                {
                    // Събираме целия таг
                    currentElement = "<";
                    i++;
                    while (i < textFromFile.Length && textFromFile[i] != '>')
                    {
                        currentElement += textFromFile[i];
                        i++;
                    }
                    currentElement += '>';
                    elements.Add(currentElement);
                    currentElement = null;
                }
                else if (textFromFile[i] != ' ' && textFromFile[i] != '\n' && textFromFile[i] != '\r' && textFromFile[i] != '\t')
                {
                    // Ако не е таг, събираме текст
                    currentElement = "";
                    while (i < textFromFile.Length && textFromFile[i] != '<' && textFromFile[i] != ' ' && textFromFile[i] != '\n' && textFromFile[i] != '\r' && textFromFile[i] != '\t')
                    {
                        currentElement += textFromFile[i];
                        i++;
                    }
                    elements.Add(currentElement);
                    currentElement = null;
                    i--; // Намаляваме i, защото може да сме преминали един символ извън текста
                }
            }

            return elements;
        }

    }
}
