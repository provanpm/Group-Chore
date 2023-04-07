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
    public GameObject foundChoreList;
    public TMP_Text showCodeText;

    public GameObject createGroupPanel;
    public GameObject showCodePanel;

    public GameObject Warning;
    public GameObject choreNav;

    private string finalCode = "";

    FirebaseFirestore db;

    void Start()
    {
        groupTitle.text = newChoreList.GetComponent<NewChoreList>().getTitle();

        foreach (Chore currChore in newChoreList.GetComponent<NewChoreList>().getList())
        {
            GameObject newChore = GameObject.Instantiate(choreTemplate) as GameObject;
            newChore.transform.GetChild(0).GetComponent<Image>().sprite = currChore.Icon;
            newChore.transform.GetChild(1).GetComponent<TMP_Text>().text = currChore.Title;
            newChore.transform.GetChild(2).GetComponent<TMP_Text>().text = currChore.Description;
            newChore.transform.SetParent(choreListParent.transform);
            newChore.transform.localScale = new Vector3(1, 1, 1);
            newChore.SetActive(true);
        }
    }

    public void editChore(GameObject currChore)
    {
        newChoreList.GetComponent<NewChoreList>().setTitle(groupTitle.text);
        newChoreList.GetComponent<NewChoreList>().setChore(new Chore(currChore.transform.GetChild(1).GetComponent<TMP_Text>().text, currChore.transform.GetChild(0).GetComponent<Image>().sprite, currChore.transform.GetChild(2).GetComponent<TMP_Text>().text));
        SceneManager.LoadScene("Chore Creation");
    }

    public void deleteChore(GameObject currChore)
    {
        for (int i = 0; i < newChoreList.GetComponent<NewChoreList>().getList().Count; i++)
        {
            if (newChoreList.GetComponent<NewChoreList>().getList()[i].Title == currChore.transform.GetChild(1).GetComponent<TMP_Text>().text)
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
        //SceneManager.LoadScene("Chore Creation");
        choreNav.GetComponent<ChoreNav>().SetCurrentScene("Chore Creation");
        choreNav.GetComponent<ChoreNav>().SetPreviousScene("Group Creation");
        choreNav.GetComponent<ChoreNav>().GoToCurrentScene();
    }

    public void confirmCreateGroup()
    {
        string warningText = "";

        if (groupTitle.text == "")
        {
            warningText = "Please choose a group title.";
        }

        if (newChoreList.GetComponent<NewChoreList>().getList().Count == 0)
        {
            warningText = "Cannot create group with 0 chores.";
        }

        if (warningText != "")
        {
            Warning.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = warningText;
            Warning.SetActive(true);
        }
        else
        {
            uploadGroup();
            newChoreList.GetComponent<NewChoreList>().resetData();
            showCode();
        }
    }

    public void showCode()
    {
        showCodeText.text = finalCode;
        createGroupPanel.SetActive(false);
        showCodePanel.SetActive(true);
    }

    public void confirmShowCode()
    {
        foundChoreList.GetComponent<FoundChoreList>().setCode(finalCode);
        if (PlayerPrefs.GetString("DisplayName") == "")
        {
            SceneManager.LoadScene("User Verification");
        }
        else {
            SceneManager.LoadScene("Joined Group");
        }
    }

    private void uploadGroup()
    {
        db = FirebaseFirestore.DefaultInstance;

        finalCode = generateCode();

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
            newChoreRef.SetAsync(currChore).ContinueWithOnMainThread(task => {
                Debug.Log("Chore Successfully Added");
            });
        }

        Debug.Log(finalCode);
    }

    private string generateCode()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var stringChars = new char[8];
        var random = new System.Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        return new String(stringChars);
    }

    public void closeWarning()
    {
        Warning.SetActive(false);
        Warning.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "";
    }

    public void toCodeEntry()
    {
        newChoreList.GetComponent<NewChoreList>().resetData();
        choreNav.GetComponent<ChoreNav>().GoToPreviousScene();
    }
}
