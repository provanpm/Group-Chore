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

public class ChoreDetail : MonoBehaviour
{
    FirebaseFirestore db;
    public GameObject foundChoreList;
    public GameObject singleChoreDetail;
    public GameObject DataTemplate;
    public GameObject choreLogListParent;
    public GameObject choreNav;
    public GameObject fullDetailPanel;
    public GameObject individualDetailPanel;
    public GameObject logDonePanel;
    public TMP_Text individualDetailDate;
    public TMP_InputField individualDetailDoneBy;
    public TMP_InputField individualDetailNotes;
    public TMP_Text choreTitle;

    public TMP_InputField addlNotes;
    public TMP_Text dateText;
    public TMP_Text choreNameText;

    private string choreName;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        choreName  = singleChoreDetail.GetComponent<SingleChoreDetail>().getTitle();
        choreTitle.text  = choreName;

        loadChores();
    }

    public void loadChores()
    {
        Query choreDetailListQuery = db.Collection($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores/{choreName}/Details");
        choreDetailListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot choreDetailListQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in choreDetailListQuerySnapshot.Documents)
            {
                GameObject choreLog = GameObject.Instantiate(DataTemplate) as GameObject;
                choreLog.transform.SetParent(choreLogListParent.transform);
                choreLog.transform.localScale = new Vector3(1, 1, 1);
                Dictionary<string, object> details = documentSnapshot.ToDictionary();
                choreLog.transform.GetChild(0).GetComponent<TMP_Text>().text = details["Done By"].ToString();
                choreLog.transform.GetChild(1).GetComponent<TMP_Text>().text = details["Date"].ToString();
                choreLog.transform.GetChild(2).GetComponent<TMP_Text>().text = details["Notes"].ToString();
                choreLog.GetComponent<Button>().onClick.AddListener(() => detailClicked(details["Done By"].ToString(), details["Date"].ToString(), details["Notes"].ToString()));
                choreLog.SetActive(true);
            }
        });
    }

    public void LogChoreDone()
    {
        Query choreDetailListQuery = db.Collection($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores/{choreName}/Details");
        choreDetailListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot choreDetailListQuerySnapshot = task.Result;

            DocumentReference groupRef = db.Document($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores/{choreName}/Details/{choreDetailListQuerySnapshot.Documents.Count() + 1}");

            Dictionary<string, object> newChoreDone = new Dictionary<string, object>
            {
                { "Done By", PlayerPrefs.GetString("DisplayName") },
                { "Date", DateTime.Now.ToString() },
                { "Notes", addlNotes.text }
            };

            groupRef.SetAsync(newChoreDone).ContinueWithOnMainThread(task => {
                Debug.Log("Chore Log Successfully Added");
                backToJoinGroup();
            });
        });
    }

    public void detailClicked(string doneBy, string dateDone, string fullNotes)
    {
        individualDetailDate.text = dateDone;
        individualDetailDoneBy.text = doneBy;
        individualDetailNotes.text = fullNotes;
        fullDetailPanel.SetActive(false);
        individualDetailPanel.SetActive(true);
    }

    public void showFullDetail()
    {
        fullDetailPanel.SetActive(true);
        individualDetailPanel.SetActive(false);
    }

    public void showLogDone()
    {
        dateText.text = DateTime.Now.ToString();
        choreNameText.text = choreName;
        fullDetailPanel.SetActive(false);
        logDonePanel.SetActive(true);
    }

    public void backToChoreDetail()
    {
        fullDetailPanel.SetActive(true);
        logDonePanel.SetActive(false);
        individualDetailPanel.SetActive(false);
    }

    public void backToJoinGroup()
    {
        choreNav.GetComponent<ChoreNav>().GoToPreviousScene();
    }
}