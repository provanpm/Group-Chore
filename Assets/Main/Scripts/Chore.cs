using System;
using UnityEngine;

[Serializable]
public class Chore
{
    public string Title { get; set; }
    public Sprite Icon { get; set; }

    public Chore()
    {
        this.Title = "";
    }

    public Chore(string title)
    {
        this.Title = title;
    }

    public Chore(string title, Sprite icon)
    {
        this.Title = title;
        this.Icon = icon;
    }

    public bool Equals(Chore newChore)
    {
        if (this.Title == newChore.Title && this.Icon.name == newChore.Icon.name)
        {
            return true;
        }
        return false;
    }
}