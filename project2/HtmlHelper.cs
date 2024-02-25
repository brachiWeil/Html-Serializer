using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace project2
{
    public class HtmlHelper
    {
        public string[] AllTags { get; set; }
        public string[] SelfClosingTags { get; set; }
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public HtmlHelper()
        {
            try
            {
                var allTagsJson = File.ReadAllText("allTags.json");
                var selfClosingTagsJson = File.ReadAllText("selfClosingTags.json");

                AllTags = JsonSerializer.Deserialize<string[]>(allTagsJson);
                SelfClosingTags = JsonSerializer.Deserialize<string[]>(selfClosingTagsJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading HTML tags: {ex.Message}");
          
            }


        }

        public string CleanHtml(string html)
        {
            return  new Regex("\\s").Replace(html, "");
        }

        public List<string> ExtractHtmlLines(string cleanHtml)
        {
          
            return new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToList();
        }


        
        public HtmlElement  Build(List<string> htmlLines)
        {
            HtmlElement rootElement = new HtmlElement("root", null);
            HtmlElement currentElement = rootElement;

            foreach (var line in htmlLines)
            {
                var parts = line.Split(new char[] { ' ' }, 2);
                var tag = parts[0];

                if (tag.StartsWith("html/"))
                {
                    break;
                }

                if (currentElement != null && tag.StartsWith("/"))
                {
                    currentElement = currentElement.Parent;
                }
                else if (AllTags.Contains(tag))
                {
                    var newElement = new HtmlElement(tag, currentElement);
                    newElement.Classes = new List<string>();
                    newElement.Attributes = new List<string>();

                    // Parse attributes
                    var attributes = new Regex("<(.*?)>").Split(line).Skip(1).Where(s => s.Length > 0).ToList();
                    foreach (var attribute in attributes)
                    {
                        var parts1 = attribute.Split(new char[] { '=' }, 2);

                        // Check if there are two parts before accessing parts1[1]
                        if (parts1 != null && parts1.Length == 2)
                        {
                            var name = parts1[0].Trim();
                            var value = parts1[1].Trim();

                            if (name.Equals("class"))
                            {
                                newElement.Classes.Add(value);
                            }
                            else
                            {
                                newElement.Attributes.Add(name);
                            }
                        }
                    }

                    // Check if tag is self-closing
                    if (tag.EndsWith("/") || SelfClosingTags.Contains(tag) || line.EndsWith("/>"))
                    {
                        newElement.InnerHtml = "";
                        if (currentElement != null)
                        {
                            if (currentElement.Children == null)
                            {
                                currentElement.Children = new List<HtmlElement>();
                            }
                            currentElement.Children.Add(newElement);
                        }
                    }
                    else
                    {
                        newElement.InnerHtml = parts.Length > 1 ? parts[1] : "";

                        if (currentElement != null)
                        {
                            if (currentElement.Children == null)
                            {
                                currentElement.Children = new List<HtmlElement>();
                            }
                            currentElement.Children.Add(newElement);
                            currentElement = newElement;
                        }
                    }
                }
                else
                {
                    if (currentElement != null)
                    {
                        currentElement.InnerHtml += line;
                    }
                }
            }

            return rootElement;
        }

    }


}






















