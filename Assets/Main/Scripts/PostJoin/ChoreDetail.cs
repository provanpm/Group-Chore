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
    FirebaseFirestore db;
    public GameObject foundChoreList;

    void Start(String choreName){
        db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db.Document($"Groups/{foundChoreList.GetComponent<FoundChoreList>().getCode()}/Chores/{choreName}");

        


    }

    void loadChoreData() {
      
    }
}