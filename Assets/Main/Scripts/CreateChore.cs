using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
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

    void Start()
    {
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

    public void confirmAddChore()
    {
        newChoreList.GetComponent<NewChoreList>().addNewChore(new Chore(previewTitle.text, previewIcon.GetComponent<Image>().sprite));
        SceneManager.LoadScene("Group Creation");
    }
}
