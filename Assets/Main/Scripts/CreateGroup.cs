using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Firestore;
using Firebase.Extensions;

public class CreateGroup : MonoBehaviour
{
    public TMP_InputField groupTitle;
    public GameObject newChoreList;
    public GameObject choreTemplate;
    public GameObject choreListParent;
    public GameObject confirmButton;
    public GameObject Warning;

    FirebaseFirestore db;

    void Start()
    {
        groupTitle.text = newChoreList.GetComponent<NewChoreList>().getTitle();

        foreach (Chore currChore in newChoreList.GetComponent<NewChoreList>().getList())
        {
            GameObject newChore = GameObject.Instantiate(choreTemplate) as GameObject;
            newChore.transform.GetChild(0).GetComponent<Image>().sprite = currChore.Icon;
            newChore.transform.GetChild(1).GetComponent<TMP_Text>().text = currChore.Title;
            newChore.transform.SetParent(choreListParent.transform);
            newChore.transform.localScale = new Vector3(1, 1, 1);
            newChore.SetActive(true);
        }

        
    }

    public void editChore(GameObject currChore)
    {
        newChoreList.GetComponent<NewChoreList>().setTitle(groupTitle.text);
        newChoreList.GetComponent<NewChoreList>().setChore(new Chore(currChore.transform.GetChild(1).GetComponent<TMP_Text>().text, currChore.transform.GetChild(0).GetComponent<Image>().sprite));
        SceneManager.LoadScene("Chore Creation");
    }

    public void deleteChore(GameObject currChore)
    {
        for (int i = 0; i < newChoreList.GetComponent<NewChoreList>().getList().Count; i++)
        {
            if (newChoreList.GetComponent<NewChoreList>().getList()[i].Equals(new Chore(currChore.transform.GetChild(1).GetComponent<TMP_Text>().text, currChore.transform.GetChild(0).GetComponent<Image>().sprite)))
            {
                newChoreList.GetComponent<NewChoreList>().getList().RemoveAt(i);
                break;
            }
        }
        Destroy(currChore);
    }

    public void toCreateChore()
    {
        newChoreList.GetComponent<NewChoreList>().setTitle(groupTitle.text);
        newChoreList.GetComponent<NewChoreList>().setChore(new Chore());
        SceneManager.LoadScene("Chore Creation");
    }

    public void confirmCreateGroup()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringChars = new char[8];
        var random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        string finalCode = new String(stringChars);

        db = FirebaseFirestore.DefaultInstance;

        DocumentReference newGroupRef = db.Document($"Groups/{finalCode}");
        Dictionary<string, object> newGroupData = new Dictionary<string, object>
        {
            { "Group Title", groupTitle.text }
        };

        newGroupRef.SetAsync(newGroupData).ContinueWithOnMainThread(task => {
            Debug.Log("Group Successfully Added");
        });

        foreach (Chore currChore in newChoreList.GetComponent<NewChoreList>().getList())
        {
            DocumentReference newChoreRef = db.Document($"Groups/{finalCode}/Chores/{currChore.Title}");
            Dictionary<string, object> newChoreData = new Dictionary<string, object>
            {
                { "Icon ID", currChore.Title },
                { "Chore Description", "Test chore desc" }
            };
            newChoreRef.SetAsync(newChoreData).ContinueWithOnMainThread(task => {
                Debug.Log("Chore Successfully Added");
            });
        }

        Debug.Log(finalCode);

        //string warningText = "";

        //if (groupTitle.text == "")
        //{
        //    warningText = "Please choose a group title.";
        //}

        //if (newChoreList.GetComponent<NewChoreList>().getList().Count == 0)
        //{
        //    warningText = "Cannot create group with 0 chores.";
        //}

        //if (warningText != "")
        //{
        //    Warning.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = warningText;
        //    Warning.SetActive(true);
        //}
        //else
        //{

        //}
    }

    public void closeWarning()
    {
        Warning.SetActive(false);
        Warning.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "";
    }

    public void toCodeEntry()
    {
        SceneManager.LoadScene("Code Entry");
    }
}
