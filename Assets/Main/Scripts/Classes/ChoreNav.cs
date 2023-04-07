using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class ChoreNav: MonoBehaviour
{
    private static string CurrentScene = "";
    private static List<string> PreviousScene = new List<string>();
/*
    public ChoreNav()
    {
        this.CurrentScene = "";
        this.PreviousScene = "";
    }

    public ChoreNav(string currentScene, string previousScene)
    {
        CurrentScene = currentScene;
        PreviousScene = previousScene;
    }

*/
    public void GoToPreviousScene () {
        SceneManager.LoadScene(PreviousScene.Last());
        PreviousScene.RemoveAt(PreviousScene.Count - 1);
    }

    public void GoToCurrentScene () {
        SceneManager.LoadScene(CurrentScene);
    }

    public void SetCurrentScene (string currentScene) {
        CurrentScene = currentScene;
    }

    public void SetPreviousScene (string previousScene) {
        PreviousScene.Add(previousScene);
    }

}