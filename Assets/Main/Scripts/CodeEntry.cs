using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using TMPro;

public class CodeEntry : MonoBehaviour
{
    public TMP_InputField codeInput;
    public TMP_Text feedbackText;

    private bool pauseInputListener = false;

    FirebaseFirestore db;
    CollectionReference colRef;

    void Start()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringChars = new char[8];
        var random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        string finalString = new String(stringChars);

        db = FirebaseFirestore.DefaultInstance;
        colRef = db.Collection("Groups");

        Dictionary<string, string> newGroup = new Dictionary<string, string>
        {
            { "Code", finalString },
            { "Group Name", "Test Group" },
            { "Group Owner", "Paul" },
        };

        colRef.Document(finalString).SetAsync(newGroup).ContinueWithOnMainThread(task => {
            Debug.Log("Success.");
        });
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
                    Dictionary<string, object> codes = snapshot.ToDictionary();
                }
                else
                {
                    feedbackText.text = "Group not found";
                }
            });
        }
        else
        {
            feedbackText.text = "Enter valid code.";
        }
    }
}
