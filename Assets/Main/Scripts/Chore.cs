using System;
using UnityEngine;

[Serializable]
public class Chore
{
    public string Title { get; set; }
    public Sprite Icon { get; set; }
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
        this.Description = description;
    }
}