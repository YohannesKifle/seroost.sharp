using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

namespace Seroost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide the name of the directory to index");
                return;
            }

            var files = Directory.GetFiles(args[0], "*.xhtml", SearchOption.AllDirectories);

            var tfIndex = new Dictionary<string, Dictionary<string, int>>();

            foreach(var file in files)
            {
                XDocument doc = XDocument.Load(file);
                tfIndex[file] = GetTermFreq(string.Concat(doc.Elements().Select(e => e.Value.ToUpper().ToString())));
                Console.WriteLine($"Processing {file}...");
            }

            File.WriteAllText("index.json", JsonSerializer.Serialize(files));
        }

        public static Dictionary<string, int> GetTermFreq(string input)
        {
            Dictionary<string, int> tf = new();
            Lexer lexer = new(input);
            var token = lexer.NextToken();
            while (token != null)
            {
                if (tf.ContainsKey(token))
                {
                    tf[token] += 1;
                }
                else
                {
                    tf[token] = 1;
                }
                token = lexer.NextToken();
            }
            return tf;
        }
    }
}
