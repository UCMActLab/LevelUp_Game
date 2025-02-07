using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JSONData
{
    public messageData[] messageData;
};

public class messageData
{
    public string id;
    public string value;
}
