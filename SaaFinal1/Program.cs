using System.ComponentModel;

namespace SaaFinal1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string htmlContent = Helpers.ReadFileContent("kursovaSaa.html");  // Път към HTML файла

            List<string> elements = TreeMaker.ElementSplitter(htmlContent);
            HTMLNode root = new HTMLNode("root", "Root", 0, null, null, new List<HTMLNode>(), 0);  // Стартов корен
            TreeMaker.BuildTree(elements, ref root);
            TreeMaker.ArgumentSplitter(ref root);
            HuffmanArchiver archiver = new HuffmanArchiver();
           
            HtmlSearcherFinal searcher1 = new HtmlSearcherFinal(root);
            

            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - View the tree \n2 - Search inside the tree \n3 - Modify the children \n4 - Save HTML \n5 - Decompress HTML File \n6 - Exit the program");
                var key = Console.ReadKey().KeyChar;

                Console.Clear();

                switch (key)
                {
                    case '1':
                        
                        Console.Clear();
                        TreeVisualiser.TreeVisualization(root, 0);
                        Console.ReadLine();
                        break;

                    case '2':
                        Console.Clear();
                        Console.WriteLine("Enter the path:");
                        string query = Console.ReadLine();

                        if (query == "//")
                        {
                            Console.Clear();
                            TreeVisualiser.TreeVisualization(root, 0);
                            Console.ReadLine();
                        }
                        else
                        {
                            try
                            {
                                //Търсачка
                                List<HTMLNode> searchResults = searcher1.Search(query);

                                if (searchResults.Count > 0)
                                {

                                    foreach (var result in searchResults)
                                    {
                                        //търси децата на възела
                                        if (result.ChildrenList.Count > 0)
                                        {
                                            
                                            foreach (var child in result.ChildrenList)
                                            {
                                                Console.WriteLine($"{child.TagName}");
                                            }
                                        }
                                        else
                                        {   
                                            Console.WriteLine($"The node '{result.TagName}' has no children.");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("No results found for the given path.");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error during search: {ex.Message}");
                            }

                            Console.ReadLine();
                        }
                        break;

                    case '3':
                        try
                        {
                            Console.WriteLine("Enter the path to modify:");
                            string xpathQuery = Console.ReadLine();
                            //търсачка
                            List<HTMLNode> parentNodes = searcher1.Search(xpathQuery);
                            if (parentNodes.Count == 0)
                            {
                                Console.WriteLine("No nodes found.");
                            }
                            else
                            {  
                                Console.WriteLine($"Found {parentNodes.Count} node(s).");

                                Console.WriteLine("Enter the new node:");
                                string newTagName = Console.ReadLine();
                                //за всяко избрано дете се извършва промяна
                                foreach (var parentNode in parentNodes)
                                {
                                    foreach (var childNode in parentNode.ChildrenList)
                                    {
                                        childNode.TagName = newTagName;
                                    }
                                }

                                Console.WriteLine("Successfull");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                        }
                        Console.ReadLine();
                        break;

                    case '4':
                        Console.Clear();
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string archiveFileName = "compressed_html_content.bin"; 
                        string fullPath = Path.Combine(desktopPath, archiveFileName);

                        string serializedHtml = Helpers.SerializeTree(root);
                        archiver.SaveToArchive(fullPath, htmlContent);
                        Console.WriteLine($"HTML content has been saved to {fullPath} successfully!");
                        Console.ReadLine();
                        break;

                    case '5':
                        try
                        {
                            Console.WriteLine("Enter the path of the compressed archive file to decompress:");
                            string filePath = Console.ReadLine();
                            
                            string decompressedContent = archiver.ReadFromArchive(filePath);

                            Console.WriteLine("Decompressed content:");
                            Console.WriteLine(decompressedContent);

                            //Изработва наново дърво с елементите, които е извадил
                            List<string> elements2 = TreeMaker.ElementSplitter(decompressedContent);
                            root = new HTMLNode("root", "Root", 0, null, null, new List<HTMLNode>(), 0);
                            TreeMaker.BuildTree(elements2, ref root);
                            TreeMaker.ArgumentSplitter(ref root);

                            Console.WriteLine("HTML tree has been rebuilt from decompressed content.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error during decompression: {ex.Message}");
                        }
                        Console.ReadLine();
                        break;

                    case '6':
                        Console.WriteLine("Goodbye!");
                        return;

                    default:

                        Console.WriteLine("Error! Can't identify the desired command!");
                        Console.ReadLine();
                        break;


                }
            }
            
        }
    }
}
