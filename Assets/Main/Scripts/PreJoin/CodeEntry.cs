using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;
using UnityEngine.SceneManagement;

public class CodeEntry : MonoBehaviour
{
    public TMP_InputField codeInput;
    public TMP_Text feedbackText;
    public GameObject foundChoreList;
    public GameObject choreNav;

    private bool pauseInputListener = false;

    FirebaseFirestore db;
    CollectionReference colRef;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        colRef = db.Collection("Groups");
    }

    public void codeFormatCheck()
    {
        if (!pauseInputListener)
        {
            pauseInputListener = true;
            codeInput.text = codeInput.text.ToUpper();

            if (Input.GetKeyDown("backspace"))
            {
                if (codeInput.text.Length == 5)
                {
                    codeInput.text = codeInput.text.Substring(0, codeInput.text.Length - 1);
                }
            }

            if (codeInput.text.Length > 4 && !codeInput.text.Contains("-"))
            {
                codeInput.text = codeInput.text.Insert(4, "-");
                codeInput.MoveToEndOfLine(false, false);
            }

            pauseInputListener = false;
        }
    }

    public void codeExistsCheck()
    {
        if (codeInput.text.Length == 9)
        {
            feedbackText.text = "Searching...";
            colRef.Document(codeInput.text.Remove(4, 1)).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    feedbackText.text = "Group found!";
                    //Dictionary<string, object> codes = snapshot.ToDictionary();
                    foundChoreList.GetComponent<FoundChoreList>().setCode(codeInput.text.Remove(4, 1));
                    SceneManager.LoadScene("Joined Group");
                }
                else
                {
                    feedbackText.text = "Group not found!";
                }
            });
        }
        else
        {
            feedbackText.text = "Enter valid code.";
        }
    }

    public void toCreateGroup()
    {
        choreNav.GetComponent<ChoreNav>().SetCurrentScene("Group Creation");
        choreNav.GetComponent<ChoreNav>().SetPreviousScene("Code Entry");
        choreNav.GetComponent<ChoreNav>().GoToCurrentScene();
        //SceneManager.LoadScene("Group Creation");
    }
}
