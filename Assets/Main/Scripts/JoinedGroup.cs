using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;

public class JoinedGroup : MonoBehaviour
{
    FirebaseFirestore db;
    Dictionary<string, object> groupData = new Dictionary<string, object>();
    Dictionary<string, object> choreNames = new Dictionary<string, object>();

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db.Collection("Groups").Document("76JFIQQD");

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                groupData = snapshot.ToDictionary();
                choreNames = groupData["Chore Names"] as Dictionary<string, object>;
                loadChoreData();
            }
            else
            {
                Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
            }
        });
        
    }

    private void loadChoreData()
    {
        string groupTitle = groupData["Group Title"] as string;

        foreach (KeyValuePair<string, object> data in choreNames)
        {
            Debug.Log(String.Format("{0}: {1}", data.Key, data.Value));
        }
    }
}
