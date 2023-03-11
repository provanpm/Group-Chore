using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoundChoreList : MonoBehaviour
{
    static private List<Chore> newChoreList = new List<Chore>();
    static private string groupTitle = "";
    static private string groupCode = "";

    public void setTitle(string newTitle)
    {
        groupTitle = newTitle;
    }

    public string getTitle()
    {
        return groupTitle;
    }

    public void setList(List<Chore> newList)
    {
        newChoreList = newList;
    }

    public List<Chore> getList()
    {
        return newChoreList;
    }

    public void setCode(string newCode)
    {
        groupCode = newCode;
    }

    public string getCode()
    {
        return groupCode;
    }
}
