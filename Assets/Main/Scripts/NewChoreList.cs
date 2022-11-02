using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewChoreList : MonoBehaviour
{
    static private List<Chore> newChoreList = new List<Chore>();
    static private Chore choreToEdit = new Chore();

    public void setChore(Chore newChore)
    {
        choreToEdit = newChore;
    }

    public Chore getChore()
    {
        return choreToEdit;
    }

    public void setList(List<Chore> newList)
    {
        newChoreList = newList;
    }

    public void addNewChore(Chore newChore)
    {
        newChoreList.Add(newChore);
    }

    public List<Chore> getList()
    {
        return newChoreList;
    }
}
