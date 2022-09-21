using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateGroup : MonoBehaviour
{
    public GameObject newChoreList;

    void Start()
    {
        foreach (Chore newChore in newChoreList.GetComponent<NewChoreList>().getList())
        {
            Debug.Log(newChore.Title);
        }
    }

    public void toCreateChore()
    {
        SceneManager.LoadScene("Chore Creation");
    }
}
