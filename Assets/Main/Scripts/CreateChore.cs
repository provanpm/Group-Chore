using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateChore : MonoBehaviour
{
    public GameObject newChoreList;
    public List<Sprite> Icons;
    public GameObject iconTemplate;
    public GameObject iconParent;
    public GameObject iconScroll;
    public TMP_InputField choreTitle;
    public GameObject previewIcon;
    public TMP_Text previewTitle;
    public GameObject Warning;

    void Start()
    {
        if (newChoreList.GetComponent<NewChoreList>().getChore().Title != "")
        {
            previewTitle.text = newChoreList.GetComponent<NewChoreList>().getChore().Title;
            choreTitle.text = newChoreList.GetComponent<NewChoreList>().getChore().Title;
            previewIcon.GetComponent<Image>().sprite = newChoreList.GetComponent<NewChoreList>().getChore().Icon;
        }

        foreach (Sprite currSprite in Icons)
        {
            GameObject newIcon = GameObject.Instantiate(iconTemplate) as GameObject;
            newIcon.GetComponent<Image>().sprite = currSprite;
            newIcon.transform.SetParent(iconParent.transform);
            newIcon.transform.localScale = new Vector3(1, 1, 1);
            newIcon.SetActive(true);
        }
        iconScroll.GetComponent<Scrollbar>().value = 1;
    }

    public void updatePreviewIcon(GameObject currIcon)
    {
        previewIcon.GetComponent<Image>().sprite = currIcon.GetComponent<Image>().sprite;
    }

    public void updatePreviewTitle()
    {
        previewTitle.text = choreTitle.text;
    }

    public void cancelAddChore()
    {
        SceneManager.LoadScene("Group Creation");
    }

    public void confirmAddChore()
    {
        string warningText = "";

        for (int i = 0; i < newChoreList.GetComponent<NewChoreList>().getList().Count; i++)
        {
            if (newChoreList.GetComponent<NewChoreList>().getList()[i].Title == previewTitle.text && previewTitle.text != newChoreList.GetComponent<NewChoreList>().getChore().Title)
            {
                warningText = "Duplicate chore title detected, please choose new title.";
            }
        }

        if (previewTitle.text == "" || previewIcon.GetComponent<Image>().sprite.name == "EMPTY")
        {
            warningText = "Please enter required fields.";
        }

        if (warningText != "")
        {
            Warning.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = warningText;
            Warning.SetActive(true);
        }
        else
        {
            if (newChoreList.GetComponent<NewChoreList>().getChore().Title != "")
            {
                for (int i = 0; i < newChoreList.GetComponent<NewChoreList>().getList().Count; i++)
                {
                    if (newChoreList.GetComponent<NewChoreList>().getList()[i].Equals(newChoreList.GetComponent<NewChoreList>().getChore()))
                    {
                        newChoreList.GetComponent<NewChoreList>().getList()[i] = new Chore(previewTitle.text, previewIcon.GetComponent<Image>().sprite);
                    }
                }
            }
            else
            {
                newChoreList.GetComponent<NewChoreList>().addNewChore(new Chore(previewTitle.text, previewIcon.GetComponent<Image>().sprite));
            }
            SceneManager.LoadScene("Group Creation");
        }
    }

    public void closeWarning()
    {
        Warning.SetActive(false);
        Warning.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = "";
    }
}
