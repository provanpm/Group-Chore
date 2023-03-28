using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoreNav: MonoBehaviour
{
    private static string CurrentScene = "";
    private static string PreviousScene = "";
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
        SceneManager.LoadScene(PreviousScene);
    }

    public void GoToCurrentScene () {
        SceneManager.LoadScene(CurrentScene);
    }

    public void SetCurrentScene (string currentScene) {
        CurrentScene = currentScene;
    }

    public void SetPreviousScene (string previousScene) {
        PreviousScene = previousScene;
    }

}