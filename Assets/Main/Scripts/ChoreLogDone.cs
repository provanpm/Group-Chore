using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;

public class ChoreLogDone : MonoBehaviour
{
    public GameObject foundChoreList;
    public TMP_InputField addlNotes;

    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    public void LogChoreDone()
    {
        Query choreDetailListQuery = db.Collection($"Groups/{""/*foundChoreList.GetComponent<FoundChoreList>().getCode()*/}6LAC3UUT/Chores/Broom/Details");
        choreDetailListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot choreDetailListQuerySnapshot = task.Result;

            DocumentReference groupRef = db.Document($"Groups/{""/*foundChoreList.GetComponent<FoundChoreList>().getCode()*/}6LAC3UUT/Chores/Broom/Details/{choreDetailListQuerySnapshot.Documents.Count() + 1}");

            Dictionary<string, object> newChoreDone = new Dictionary<string, object>
            {
                { "Done By", "PLACEHOLDER" },
                { "Date Done", DateTime.Now },
                { "Notes", addlNotes.text }
            };

            groupRef.SetAsync(newChoreDone).ContinueWithOnMainThread(task => {
                Debug.Log("Chore Done Successfully Added");
            });
        });
    }
}
