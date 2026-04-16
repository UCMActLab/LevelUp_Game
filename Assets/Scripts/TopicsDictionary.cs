using System.Collections.Generic;
using System.Collections.ObjectModel;

public class TopicsDictionary
{
    public static ReadOnlyDictionary<Topics, string> topics = new ReadOnlyDictionary<Topics, string>(new Dictionary<Topics, string>() 
    {
        {Topics.GENERAL, "General Disinformation" },
        {Topics.AI, "Artificial Intelligence" },
        {Topics.SCIENCE, "Science, Climate, and Health" },
        {Topics.SCAMS, "Scams and online security" },
        {Topics.CONSPIRACY, "Conspiracy theories" }
    });
}