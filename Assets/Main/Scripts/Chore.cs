using System;
using UnityEngine;
using Firebase.Firestore;

[Serializable]
[FirestoreData]
public class Chore
{
    public string Title { get; set; }
    public Sprite Icon { get; set; }
    [FirestoreProperty]
    public string IconID { get; set; }
    [FirestoreProperty]
    public string Description { get; set; }

    public Chore()
    {
        this.Title = "";
    }

    public Chore(string title)
    {
        this.Title = title;
    }

    public Chore(string title, Sprite icon, string description)
    {
        this.Title = title;
        this.Icon = icon;
        this.IconID = icon.name;
        this.Description = description;
    }
}