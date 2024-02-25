

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project2
{

    class Program
    {
        static async Task Main(string[] args)
        {
            var html = await Load("https://learn.malkabruk.co.il/practicode/projects/pract-2/#_3");

            var cleanHtml = new Regex("\\s").Replace(html, "");
            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);      
            static async Task<string> Load(string url)
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                var html = await response.Content.ReadAsStringAsync();
                return html;
            }
            //build tree
            var root = new HtmlElement("div");
            var child1 = new HtmlElement("dcd", root);
            var child2 = new HtmlElement("ss", root);
            var grandchild = new HtmlElement("sxs”", child1);

            root.Children.Add(child1);
            root.Children.Add(child2);
            child1.Children.Add(grandchild);
            //Descendants function
            Console.WriteLine("Descendants:");
            foreach (var element in root.Descendants())
            {
                Console.WriteLine(element.Name);
            }

            // Ancestors function
            Console.WriteLine("\nAncestors:");
            foreach (var element in grandchild.Ancestors())
            {
                Console.WriteLine(element.Name);
            }


            //FindElements function
            string queryString = "div #p .class-name";
            Selector selector2 = Selector.ParseSelector(queryString);
            Console.WriteLine("1");
            Console.WriteLine("\nFindElements:");
            var elements = root.FindElements(selector2);
            foreach (var element in elements)
            {
                Console.WriteLine(element.Name);
            }




            //build selector from string  and print the selector
            string queryString2 = "div #p .class-name";
            Selector selector3 = Selector.ParseSelector(queryString);
            Console.WriteLine(selector3.TagName);      
            Console.WriteLine(selector3.Child.SelectorId);   
            Console.WriteLine(selector3.Child.Child.Classes[0]);   

            Console.WriteLine(string.Join(", ", selector2.Classes));




            //print HtmlTreetree
            static void PrintHtmlTree(HtmlElement element, int depth)
            {
                if (element == null)
                {
                    return;
                }

                Console.WriteLine($"{new string(' ', depth * 2)}{element.Name}");

                foreach (var child in element.Children)
                {
                    PrintHtmlTree(child, depth + 1);
                }
            }

            html = await Load("https://www.example.net/");


            HtmlElement rootElement3 = HtmlHelper.Instance.Build(HtmlHelper.Instance.ExtractHtmlLines(HtmlHelper.Instance.CleanHtml(html)));
            Console.WriteLine("start");
            PrintHtmlTree(rootElement3, 0);
            Console.WriteLine("end");
           
          

        }

    }
}



