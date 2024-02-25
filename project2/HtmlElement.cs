using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace project2
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }

        public HtmlElement(string name)
        {
            Name = name;
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }
       


        public HtmlElement(string name, HtmlElement parent)
        {
            Name = name;
            Parent = parent;
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
        }

        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();

            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var currentElement = queue.Dequeue();

                yield return currentElement;

                foreach (var child in currentElement.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            var currentElement = this;

            while (currentElement.Parent != null)
            {
                currentElement = currentElement.Parent;
                yield return currentElement;
            }
        }

        public  List<HtmlElement> FindElements(Selector selector)
        {
            HashSet<HtmlElement> resultsHashSet = new HashSet<HtmlElement>();
            FindElementsRecursive(this, selector, resultsHashSet);
            List<HtmlElement> results = new List<HtmlElement>(resultsHashSet);
            return results;
        }
       
        public  void FindElementsRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            if (selector.Match(element))
            {
                if (!results.Contains(element))
                {
                    results.Add(element);

                    if (selector.Child != null)
                    {
                        foreach (var childElement in element.Descendants())
                        {
                            if (selector.Child.Match(childElement))
                            {
                                FindElementsRecursive(childElement, selector.Child, results);
                            }
                        }
                    }
                }
            }
        }
        
    }
}




