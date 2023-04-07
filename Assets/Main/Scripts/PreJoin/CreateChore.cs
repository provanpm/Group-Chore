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
    public TMP_InputField choreDescription;
    private Sprite choreIcon { get; set; }
    public GameObject Warning;
    private GameObject selectedIcon;

    void Start()
    {
        if (newChoreList.GetComponent<NewChoreList>().getChore().Title != "")
        {
            choreTitle.text = newChoreList.GetComponent<NewChoreList>().getChore().Title;
            choreDescription.text = newChoreList.GetComponent<NewChoreList>().getChore().Description;
            choreIcon = newChoreList.GetComponent<NewChoreList>().getChore().Icon;
        }

        foreach (Sprite currSprite in Icons)
        {
            GameObject newIcon = GameObject.Instantiate(iconTemplate) as GameObject;
            newIcon.transform.GetChild(1).GetComponent<Image>().sprite = currSprite;
            newIcon.transform.SetParent(iconParent.transform);
            newIcon.transform.localScale = new Vector3(1, 1, 1);
            newIcon.SetActive(true);

            if (choreIcon?.name == currSprite.name)
            {
                choreIcon = newIcon.transform.GetChild(1).GetComponent<Image>().sprite;
                newIcon.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                newIcon.transform.GetChild(1).GetComponent<Image>().color = new Color32(65, 78, 144, 255);
                selectedIcon = newIcon;
            }
        }
        iconScroll.GetComponent<Scrollbar>().value = 1;
    }

    public void updateChoreIcon(GameObject currIcon)
    {
        if (selectedIcon != null)
        {
            selectedIcon.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 0);
            selectedIcon.transform.GetChild(1).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }

        choreIcon = currIcon.transform.GetChild(1).GetComponent<Image>().sprite;
        currIcon.transform.GetChild(0).GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        currIcon.transform.GetChild(1).GetComponent<Image>().color = new Color32(65, 78, 144, 255);
        selectedIcon = currIcon;
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
            if (newChoreList.GetComponent<NewChoreList>().getList()[i].Title == choreTitle.text && choreTitle.text != newChoreList.GetComponent<NewChoreList>().getChore().Title)
            {
                warningText = "Duplicate chore title detected, please choose new title.";
            }
        }

        if (choreTitle.text == "" || choreIcon == null)
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
                    if (newChoreList.GetComponent<NewChoreList>().getList()[i].Title == newChoreList.GetComponent<NewChoreList>().getChore().Title)
                    {
                        newChoreList.GetComponent<NewChoreList>().getList()[i] = new Chore(choreTitle.text, choreIcon, choreDescription.text);
                    }
                }
            }
            else
            {
                newChoreList.GetComponent<NewChoreList>().addNewChore(new Chore(choreTitle.text, choreIcon, choreDescription.text));
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
