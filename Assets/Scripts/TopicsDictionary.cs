using System.Collections.Generic;
using System.Collections.ObjectModel;

public class TopicsDictionary
{
    public static ReadOnlyDictionary<string, Topics> topics = new ReadOnlyDictionary<string, Topics>(new Dictionary<string, Topics>() 
    {
        {"General Disinformation", Topics.GENERAL },
        {"Artificial Intelligence", Topics.AI },
        {"Science, Climate, and Health",Topics.SCIENCE },
        {"Scams and online security", Topics.SCAMS },
        {"Conspiracy theories", Topics.CONSPIRACY}
    });
}