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
    public TMP_Text groupTitle;
    public List<Sprite> Icons;
    public GameObject choreTemplate;
    public GameObject choreListParent;
    public GameObject foundChoreList;
    public GameObject createChore;
    public GameObject choreNav;
    public GameObject singleChoreDetail;

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
                Dictionary<string, object> group = snapshot.ToDictionary();
                groupTitle.text = group["Group Title"].ToString();
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
                Dictionary<string, object> chores = documentSnapshot.ToDictionary();

                GameObject newChore = GameObject.Instantiate(choreTemplate) as GameObject;
                newChore.transform.GetChild(0).GetComponent<Image>().sprite = Icons.Where(icon => icon.name == chores["IconID"].ToString()).First();
                newChore.transform.GetChild(1).GetComponent<TMP_Text>().text = documentSnapshot.Id;
                newChore.transform.GetChild(2).GetComponent<TMP_Text>().text = chores["Description"].ToString();
                newChore.transform.SetParent(choreListParent.transform);
                newChore.transform.localScale = new Vector3(1, 1, 1);
                newChore.GetComponent<Button>().onClick.AddListener(() => ChoreClicked(documentSnapshot.Id));
                newChore.SetActive(true);

                Query choreDetailListQuery = db.Collection($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores/{documentSnapshot.Id}/Details");
                choreDetailListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot choreDetailListQuerySnapshot = task.Result;

                    if (choreDetailListQuerySnapshot.Documents.Count() > 0)
                    {
                        DocumentSnapshot documentSnapshot = choreDetailListQuerySnapshot.Documents.Last();
                        Dictionary<string, object> details = documentSnapshot.ToDictionary();
                        newChore.transform.GetChild(3).GetComponent<TMP_Text>().text = "Times Done: " + choreDetailListQuerySnapshot.Documents.Count().ToString();
                        newChore.transform.GetChild(4).GetComponent<TMP_Text>().text = "Last Done: " + details["Date"];
                        newChore.transform.GetChild(5).GetComponent<TMP_Text>().text = "By: " + details["Done By"];
                    }
                    else
                    {
                        newChore.transform.GetChild(3).GetComponent<TMP_Text>().text = "Times Done: 0";
                        newChore.transform.GetChild(4).GetComponent<TMP_Text>().text = "Last Done: N/A";
                        newChore.transform.GetChild(5).GetComponent<TMP_Text>().text = "By: N/A";
                    }
                });
            }
        });
    }

    public void toCodeEntry()
    {
        SceneManager.LoadScene("Code Entry");
    }

    public void toNewChore()
    {
        choreNav.GetComponent<ChoreNav>().SetCurrentScene("Chore Creation");
        choreNav.GetComponent<ChoreNav>().SetPreviousScene("Joined Group");
        choreNav.GetComponent<ChoreNav>().GoToCurrentScene();
    }

    public void ChoreClicked(String choreName)
    {
        singleChoreDetail.GetComponent<SingleChoreDetail>().setTitle(choreName);
        choreNav.GetComponent<ChoreNav>().SetCurrentScene("Chore Detail");
        choreNav.GetComponent<ChoreNav>().SetPreviousScene("Joined Group");
        choreNav.GetComponent<ChoreNav>().GoToCurrentScene();
    }
}
