using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VerifyUser : MonoBehaviour
{
    public GameObject CreateOrLogin;
    public GameObject NameEmailEntry;
    public GameObject EmailEntry;
    public GameObject CodeEntry;
    public GameObject Warning;
    public TMP_InputField NewDisplayName;
    public TMP_InputField NewEmail;
    public TMP_InputField ExistingEmail;
    public TMP_InputField EmailCode;
    public TMP_Text warningText;

    private int emailPIN;
    private Email sendMessage;
    private string loginType = "";

    void Start()
    {
        sendMessage = ScriptableObject.CreateInstance("Email") as Email;
    }

    public void toCreateOrLogin()
    {
        CreateOrLogin.SetActive(true);
        EmailEntry.SetActive(false);
        NameEmailEntry.SetActive(false);
        CodeEntry.SetActive(false);
    }

    public void toNewUser()
    {
        CreateOrLogin.SetActive(false);
        EmailEntry.SetActive(false);
        NameEmailEntry.SetActive(true);
        CodeEntry.SetActive(false);
        loginType = "New";
    }

    public void toNewDevice()
    {
        CreateOrLogin.SetActive(false);
        EmailEntry.SetActive(true);
        NameEmailEntry.SetActive(false);
        CodeEntry.SetActive(false);
        loginType = "Existing";
    }

    public void toVerifyCode()
    {
        CreateOrLogin.SetActive(false);
        EmailEntry.SetActive(false);
        NameEmailEntry.SetActive(false);
        CodeEntry.SetActive(true);
    }

    public void backFromVerifyCode()
    {
        if (loginType == "New")
        {
            toNewUser();
        }
        else if (loginType == "Existing")
        {
            toNewDevice();
        }
    }

    public void VerifyExistingEmail()
    {
        if (ExistingEmail.text != "")
        {
            string result = newEmailCode(ExistingEmail.text);
            if (result == "Success")
            {
                toVerifyCode();
            }
            else
            {
                warningText.text = "Invalid email.";
                Warning.SetActive(true);
            }
        }
        else {
            warningText.text = "Please enter email.";
            Warning.SetActive(true);
        }
    }

    public void VerifyNewEmail()
    {
        if (NewEmail.text != "" && NewDisplayName.text != "")
        {
            string result = newEmailCode(NewEmail.text);
            if (result == "Success")
            {
                toVerifyCode();
            }
            else
            {
                warningText.text = "Invalid email.";
                Warning.SetActive(true);
            }
        }
        else
        {
            warningText.text = "Please enter required fields.";
            Warning.SetActive(true);
        }
    }

    public void VerifyCode()
    {
        if (EmailCode.text == emailPIN.ToString())
        {
            UnityEngine.Debug.Log("Success!");
        }
        else
        {
            warningText.text = "Code incorrect.";
            Warning.SetActive(true);
        }
    }

    public string newEmailCode(string Email)
    {
        emailPIN = UnityEngine.Random.Range(1000, 10000);
        string result = sendMessage.SendEmail(Email, "GroupChore Verification", "Hello The account confirmation code is: " + emailPIN.ToString());
        return result;
    }

    public void closeWarning()
    {
        warningText.text = "";
        Warning.SetActive(false);
    }
}
