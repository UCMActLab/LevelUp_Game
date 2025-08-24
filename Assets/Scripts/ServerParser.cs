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
    public List<Article> Articles;
}

[Serializable]
public class Article
{
    public string Themes;
    public bool Key;
    public bool Fakeornot;
    public string ConversationRef;

    public LanguageBlock ES;
    public LanguageBlock CR;
    public LanguageBlock B;
}

[Serializable]
public class LanguageBlock
{
    public string Headline;
    public string Body;
    public string Multimedia;
    public string Links;
    public string Source;

    public string Reaction_G1;
    public string Reaction_G2;
    public string Reaction_G3;
}

#endregion