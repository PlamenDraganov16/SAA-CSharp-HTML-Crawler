using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaFinal1
{
    internal class HuffmanArchiver
    {
        
            // Вътрешен клас за представяне на възел в дървото на Хъфман
            private class HuffmanNode
            {
                public char Character { get; set; }
                public int Frequency { get; set; }
                public HuffmanNode Left { get; set; }
                public HuffmanNode Right { get; set; }

                public HuffmanNode(char character, int frequency)
                {
                    Character = character;
                    Frequency = frequency;
                    Left = null;
                    Right = null;
                }
            }

            private HuffmanNode root; 
            private Dictionary<char, string> huffmanCodes; 

            public HuffmanArchiver()
            {
                root = null;
                huffmanCodes = new Dictionary<char, string>();
            }

            // Създаване на дървото на Хъфман
            internal void BuildHuffmanTree(Dictionary<char, int> frequencies)
            {
                PriorityQueue<HuffmanNode> queue = new PriorityQueue<HuffmanNode>();

                //създаване на листа (възли с конкретни символи) и добавянето им в опашката
                foreach (var entry in frequencies)
                {
                //добавят се всички символи от входния текст в опашка
                queue.Enqueue(new HuffmanNode(entry.Key, entry.Value), entry.Value);
                }

                while (queue.Count > 1)
                {
                    HuffmanNode left = queue.Dequeue();
                    HuffmanNode right = queue.Dequeue();

                    //създаване на родителски възел със сумирана честота
                    HuffmanNode parent = new HuffmanNode('*', left.Frequency + right.Frequency)
                    {
                        Left = left,
                        Right = right
                    };

                    queue.Enqueue(parent, parent.Frequency);
                }

                root = queue.Dequeue();
            }

            // Генериране на кодовете на символите
            private void GenerateCodes(HuffmanNode node, string currentCode)
            {
                if (node == null)
                    return;

                if (node.Left == null && node.Right == null)
                {
                    huffmanCodes[node.Character] = currentCode;
                    return;
                }

                GenerateCodes(node.Left, currentCode + "0");
                GenerateCodes(node.Right, currentCode + "1");
            }

            // Компресиране на текст
            public byte[] Compress(string text)
            {
                Dictionary<char, int> frequencies = CalculateFrequencies(text);
                BuildHuffmanTree(frequencies);
                GenerateCodes(root, "");

                string encodedText = "";
                foreach (char c in text)
                {
                    encodedText += huffmanCodes[c];
                }

                return ConvertToBytes(encodedText); // Конвертира битовия низ в байтов масив
        }

            // Декомпресиране на текст
            public string Decompress(byte[] compressedData)
            {
                string bitString = ConvertToBitString(compressedData);

                string decodedText = "";
                HuffmanNode currentNode = root;
                foreach (char bit in bitString)
                {
                    //преминава наляво или надясно в зависимост от битовата стойност
                    currentNode = bit == '0' ? currentNode.Left : currentNode.Right;

                    if (currentNode.Left == null && currentNode.Right == null)
                    {
                        decodedText += currentNode.Character;
                        currentNode = root;
                    }
                }

                return decodedText;
            }

            // Запис в архивен файл
            public void SaveToArchive(string filePath, string content)
            {
                byte[] compressedData = Compress(content);
                File.WriteAllBytes(filePath, compressedData);
            }

            // Четене от архивен файл
            public string ReadFromArchive(string filePath)
            {
                byte[] compressedData = File.ReadAllBytes(filePath);
                return Decompress(compressedData);
            }

            // Изчисляване на честотите на символите
            private Dictionary<char, int> CalculateFrequencies(string text)
            {
                Dictionary<char, int> frequencies = new Dictionary<char, int>();

                //преброява срещането на всеки символ
                foreach (char c in text)
                {
                    if (!frequencies.ContainsKey(c))
                        frequencies[c] = 0;

                    frequencies[c]++;
                }

                return frequencies;
            }

            // Конвертиране на битов низ в байтов масив
            private byte[] ConvertToBytes(string bitString)
            {
                List<byte> bytes = new List<byte>();

                for (int i = 0; i < bitString.Length; i += 8)
                {
                    string byteString = bitString.Substring(i, Math.Min(8, bitString.Length - i));
                    bytes.Add(Convert.ToByte(byteString, 2)); //10тична стойност
                }

                return bytes.ToArray();
            }

            // Конвертиране на байтов масив в битов низ
            private string ConvertToBitString(byte[] bytes)
            {
                string bitString = "";

                foreach (byte b in bytes)
                {
                    bitString += Convert.ToString(b, 2).PadLeft(8, '0');
                }

                return bitString;
            }
        }

        internal class PriorityQueue<T>
        {
            private List<(T Item, int Priority)> elements;

            public PriorityQueue()
            {
                elements = new List<(T, int)>();
            }

            public void Enqueue(T item, int priority)
            {
                elements.Add((item, priority));
            }
            
            public T Dequeue()
            {
                int bestIndex = 0;

                //Намира елемента с най-висок приоритет
                for (int i = 1; i < elements.Count; i++)
                {
                    if (elements[i].Priority < elements[bestIndex].Priority)
                    {
                        bestIndex = i;
                    }
                }

                T bestItem = elements[bestIndex].Item;
                elements.RemoveAt(bestIndex);
                return bestItem;
            }
            //проверява броя на елементи в опашката
            public int Count => elements.Count;
        }
    
}
