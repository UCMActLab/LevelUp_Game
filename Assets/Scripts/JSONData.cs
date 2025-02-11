using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONData
{
    public int numMessages;
    public int numAnswers;
    public messagesData[] messages;
    public answerData[] answers;
};

[Serializable]
public class messagesData
{
    public string id;
    public string value;
    
}

[Serializable]
public class answerData
{
    public string id;
    public string value;
}
