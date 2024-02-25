using project2;
using System;
using System.Collections.Generic;

public class Selector
{
    public string TagName { get; set; }
    public string SelectorId { get; set; }
    public List<string> Classes { get; set; }
    public Selector Parent { get; set; }
    public Selector Child { get; set; }

    public Selector(string tagName = null, string selectorId = null, List<string> classes = null,
                       Selector parent = null, Selector child = null)
    {
        TagName = tagName;
        SelectorId = selectorId;
        Classes = classes ?? new List<string>();
        Parent = parent;
        Child = child;
    }

    public static Selector ParseSelector(string query)
    {
        string[] parts = query.Split(' ');

        Selector rootSelector = new Selector();
        Selector currentSelector = rootSelector;

        foreach (string part in parts)
        {
            if (part.StartsWith("#"))
            {
                currentSelector.SelectorId = part.Substring(1);

            }
            else if (part.StartsWith("."))
            {
                currentSelector.Classes.Add(part.Substring(1));

            }
            else
            {
                string tagName = part;
                if (IsValidTagName(tagName))
                {
                    currentSelector.TagName = tagName;
                }


            }
            Selector childSelector = new Selector { TagName = part };
            currentSelector.Child = childSelector;
            currentSelector = childSelector;


        }

        return rootSelector;
    }

    public bool Match(HtmlElement element)
    {
        if (!string.IsNullOrEmpty(TagName) && element.Name != TagName)
        {
            return false;
        }

        if (!string.IsNullOrEmpty(SelectorId) && element.Id != SelectorId)
        {
            return false;
        }

        if (Classes != null && !Classes.All(c => element.Classes.Contains(c)))
        {
            return false;
        }


        return true;
    }


    private static bool IsValidTagName(string tagName)
    {
        // Read the file containing valid HTML tags
        var validTags = File.ReadAllText("allTags.json");

        // Check if the given tag name is in the list of valid tags
        return validTags.Contains(tagName);
    }

}