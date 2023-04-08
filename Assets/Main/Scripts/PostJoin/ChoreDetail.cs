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
    public TMP_Text individualDetailDate;
    public TMP_InputField individualDetailDoneBy;
    public TMP_InputField individualDetailNotes;
    public TMP_Text choreTitle;

    void Start(){
        db = FirebaseFirestore.DefaultInstance;

        string choreName  = singleChoreDetail.GetComponent<SingleChoreDetail>().getTitle();
        choreTitle.text  = choreName;

        Query choreDetailListQuery = db.Collection($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores/{choreName}/Details");
        choreDetailListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            QuerySnapshot choreDetailListQuerySnapshot = task.Result;
            Debug.Log(choreDetailListQuerySnapshot.Documents.Count());
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

    public void toLogDone()
    {
        choreNav.GetComponent<ChoreNav>().GoToPreviousScene();
    }

    public void backToJoinGroup()
    {
        choreNav.GetComponent<ChoreNav>().GoToPreviousScene();
    }
}