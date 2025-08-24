using System;
using System.Collections.Generic;
using UnityEngine;

// Temas de ejemplo
public enum ID_Temas { GeneralMisinformation = 001, ScienceClimateHealth = 002, ScamsOnlineSecurity = 003, ConspiracyTheories = 004, ArtificialIntelligence = 005}

#region Login

[Serializable]
public class LoginData
{
    public Data data;
}

[Serializable]
public class Data
{
    public bool success;
    public string token;
    public User user;
}

[Serializable]
public class User
{
    public string username;
    public string role;
}

#endregion

#region Resources

[Serializable]
public class RootObject
{
    public RootData data;
}

[Serializable]
public class RootData
{
    public List<ItemData> data;
    public Pagination pagination;
}

[Serializable]
public class ItemData
{
    public string _id;
    public string country;
    public string createdAt;
    public string description;
    public List<Resource> resources;
    public string headline;
    public string sources;
    public string body;
    public string theme;
    public string verified;
    public string updatedAt;
    public List<Answer> answers;
    public string conversationRef;
}

[Serializable]
public class Answer
{
    public string title;
    public string explanation;
    public List<Resource> resources;
}

[Serializable]
public class Resource
{
    public string type;
    public string name;
    public string resourceLocator;
}

[Serializable]
public class Pagination
{
    public int currentPage;
    public int totalPages;
    public int totalItems;
    public int itemsPerPage;
}


#endregion