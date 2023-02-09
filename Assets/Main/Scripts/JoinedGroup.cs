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
        QuerySnapshot choreListQuerySnapshot;

        Query choreListQuery = db.Collection($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores");
        choreListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            choreListQuerySnapshot = task.Result;
            foreach (DocumentSnapshot documentSnapshot in choreListQuerySnapshot.Documents)
            {
                Dictionary<string, object> chores = documentSnapshot.ToDictionary();

                GameObject newChore = GameObject.Instantiate(choreTemplate) as GameObject;
                newChore.transform.GetChild(0).GetComponent<Image>().sprite = Icons.Where(icon => icon.name == chores["IconID"].ToString()).First();
                newChore.transform.GetChild(1).GetComponent<TMP_Text>().text = documentSnapshot.Id;
                newChore.transform.GetChild(2).GetComponent<TMP_Text>().text = chores["Description"].ToString();
                newChore.transform.SetParent(choreListParent.transform);
                newChore.transform.localScale = new Vector3(1, 1, 1);
                newChore.SetActive(true);


                Query choreDetailListQuery = db.Collection($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores/{documentSnapshot.Id}/Details");
                choreDetailListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    QuerySnapshot choreDetailListQuerySnapshot = task.Result;
                    foreach (DocumentSnapshot documentSnapshot in choreDetailListQuerySnapshot.Documents)
                    {
                        Dictionary<string, object> details = documentSnapshot.ToDictionary();
                        newChore.transform.GetChild(3).GetComponent<TMP_Text>().text = "Times Done: " + choreDetailListQuerySnapshot.Documents.Count().ToString();
                        newChore.transform.GetChild(4).GetComponent<TMP_Text>().text = details["Date Done"].ToString();
                        newChore.transform.GetChild(5).GetComponent<TMP_Text>().text = details["Done By"].ToString();
                        //Debug.Log(choreListParent.transform.GetChild(i).GetChild(3).name);
                        //choreListParent.transform.GetChild(i).GetChild(3).GetComponent<TMP_Text>().text = "AAAAAAAA";
                        //choreListParent.transform.GetChild(i).GetChild(4).GetComponent<TMP_Text>().text = details["Date Done"].ToString();
                        //choreListParent.transform.GetChild(i).GetChild(5).GetComponent<TMP_Text>().text = details["Done By"].ToString();
                        Debug.Log(details.ElementAt(0));
                        Debug.Log(details.ElementAt(1));
                        Debug.Log(details.ElementAt(2));
                    }
                });

            }
           // loadChoreDetails();
        });
    }

    private void loadChoreDetails()
    {
        for (int i = 1; i <= choreListParent.transform.childCount; i++)
        {
            string choreName = choreListParent.transform.GetChild(i).GetChild(1).GetComponent<TMP_Text>().text;
            Query choreDetailListQuery = db.Collection($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores/{choreName}/Details");
            choreDetailListQuery.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                QuerySnapshot choreDetailListQuerySnapshot = task.Result;
                foreach (DocumentSnapshot documentSnapshot in choreDetailListQuerySnapshot.Documents)
                {
                    Dictionary<string, object> details = documentSnapshot.ToDictionary();
                    //Debug.Log(choreListParent.transform.GetChild(i).GetChild(3).name);
                    //choreListParent.transform.GetChild(i).GetChild(3).GetComponent<TMP_Text>().text = "AAAAAAAA";
                    //choreListParent.transform.GetChild(i).GetChild(4).GetComponent<TMP_Text>().text = details["Date Done"].ToString();
                    //choreListParent.transform.GetChild(i).GetChild(5).GetComponent<TMP_Text>().text = details["Done By"].ToString();
                    Debug.Log(details.ElementAt(0));
                    Debug.Log(details.ElementAt(1));
                    Debug.Log(details.ElementAt(2));
                }
            });
        }
    }

    public void toCodeEntry()
    {
        SceneManager.LoadScene("Code Entry");
    }
}
