using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreateUser : MonoBehaviour
{
    public Email sendMessage;
    public TMP_InputField userEmail;

    private static int emailPIN;

    void Start()
    {
        sendMessage = new Email();
    }

    public void newEmailCode()
    {
        emailPIN = UnityEngine.Random.Range(5000, 10000);
        sendMessage.SendEmail(userEmail.text, "GroupChore Verification", "The account confirmation code is: " + emailPIN.ToString());
        UnityEngine.Debug.Log(emailPIN.ToString());
    }
}
