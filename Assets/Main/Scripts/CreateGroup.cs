using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CreateGroup : MonoBehaviour
{
    public GameObject newChoreList;
    public GameObject choreTemplate;
    public GameObject choreListParent;

    void Start()
    {
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
        newChoreList.GetComponent<NewChoreList>().setChore(new Chore(currChore.transform.GetChild(1).GetComponent<TMP_Text>().text, currChore.transform.GetChild(0).GetComponent<Image>().sprite));
        SceneManager.LoadScene("Chore Creation");
    }

    public void deleteChore()
    {

    }

    public void toCreateChore()
    {
        newChoreList.GetComponent<NewChoreList>().setChore(new Chore());
        SceneManager.LoadScene("Chore Creation");
    }
}
