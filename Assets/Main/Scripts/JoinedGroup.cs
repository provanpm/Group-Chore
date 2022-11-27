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

public class JoinedGroup : MonoBehaviour
{
    public List<Sprite> Icons;
    public GameObject choreTemplate;
    public GameObject choreListParent;
    public GameObject foundChoreList;

    FirebaseFirestore db;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db.Document($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}");

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
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
        Query choreListQuery = db.Collection($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores");
        choreListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot choreListQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in choreListQuerySnapshot.Documents)
            {
                Debug.Log(String.Format("Document data for {0} document:", documentSnapshot.Id));
                Dictionary<string, object> chores = documentSnapshot.ToDictionary();

                GameObject newChore = GameObject.Instantiate(choreTemplate) as GameObject;
                newChore.transform.GetChild(0).GetComponent<Image>().sprite = Icons.Where(icon => icon.name == chores["IconID"].ToString()).First();
                newChore.transform.GetChild(1).GetComponent<TMP_Text>().text = documentSnapshot.Id;
                newChore.transform.GetChild(2).GetComponent<TMP_Text>().text = chores["Description"].ToString();
                newChore.transform.SetParent(choreListParent.transform);
                newChore.transform.localScale = new Vector3(1, 1, 1);
                newChore.SetActive(true);
            }
        });
    }

    public void toCodeEntry()
    {
        SceneManager.LoadScene("Code Entry");
    }
}
