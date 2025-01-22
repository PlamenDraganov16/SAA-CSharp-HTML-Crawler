using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaFinal1
{
    internal class Helpers
    {
        public static string ReadFileContent(string filePath)
        {
            string content = "";

            try
            {   //нов FileStream обект
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {   //нов обект да чете filestream
                    using (StreamReader reader = new StreamReader(fileStream))
                    {   
                        char[] buffer = new char[1024];
                        int bytesRead;
                        // чете символи от потока fileStream и ги записва в буфера buffer
                        while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
                        {   // на всеки цикъл се добавя прочетеното
                            content += new string(buffer, 0, bytesRead);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: File not found.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading file: " + ex.Message);
            }
            return content;
        }

        public static string NumberOfTabluations(int tabIndex)
        {
            string tabs = "";

            
            for (int i = 0; i < tabIndex; i++)
            {
                tabs += "    "; 
            }

            return tabs;
        }

       public static string SerializeTree(HTMLNode node)
        {
            if (node == null) return "";

            
            string serialized = "";

            // Проверка дали името му е root
            if (node.TagName != "Root")
            {
                serialized += "<"; 
                serialized += node.TagName; 

                //ръчно добавя атрибути
                if (node.Argument != null && node.Argument.Length > 0)
                {
                    serialized += " ";
                    serialized += node.Argument; 
                }

                serialized += ">"; 
            }

            // ръчно серелиаризиране на всяко дете
            for (int i = 0; i < node.ChildrenList.Count; i++)
            {
                serialized += SerializeTree(node.ChildrenList[i]); // рекурсивно сереализиране
            }

            //затварящ таг
            if (node.TagName != "Root")
            {
                serialized += "</";  
                serialized += node.TagName; 
                serialized += ">"; 
            }

            return serialized;
        }
    }
}
