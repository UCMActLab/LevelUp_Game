using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONData
{
    public int numMessages;
    public int numAnswers;
    public int numPictures;
    public int numVideos;
    public int numAudios;
    public messagesData[] messages;
    public answerData[] answers;
    public pictureData[] pictures;
    public videoData[] videos;
    public audioData[] audios;
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

[Serializable]
public class pictureData
{
    public string id;
    public string path;
}

[Serializable]
public class videoData
{
    public string id;
    public string path;
}

[Serializable]
public class audioData
{
    public string id;
    public string path;
}