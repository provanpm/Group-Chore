using System;
using UnityEngine;
using Firebase.Firestore;

public class SingleChoreDetail : MonoBehaviour
{
    private static string Title = "";
    
    public void setTitle(string newTitle)
    {
        Title = newTitle;
    }

    public string getTitle()
    {
        return Title;
    }
}