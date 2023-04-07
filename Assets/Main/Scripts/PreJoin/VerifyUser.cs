using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Firestore;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class VerifyUser : MonoBehaviour
{
    public GameObject createOrLogin;
    public GameObject nameemailEntry;
    public GameObject emailEntry;
    public GameObject codeEntry;
    public GameObject warning;
    public TMP_InputField newDisplayName;
    public TMP_InputField newEmail;
    private string existingDisplayName;
    public TMP_InputField existingEmail;
    public TMP_InputField emailCode;
    public TMP_Text warningText;

    private int emailPIN;
    private Email sendMessage;
    private string loginType = "";

    FirebaseFirestore db;
    CollectionReference colRef;

    void Start()
    {
        sendMessage = ScriptableObject.CreateInstance("Email") as Email;
        db = FirebaseFirestore.DefaultInstance;
        colRef = db.Collection("Users");
    }

    public void toCreateOrLogin()
    {
        createOrLogin.SetActive(true);
        emailEntry.SetActive(false);
        nameemailEntry.SetActive(false);
        codeEntry.SetActive(false);
    }

    public void toNewUser()
    {
        createOrLogin.SetActive(false);
        emailEntry.SetActive(false);
        nameemailEntry.SetActive(true);
        codeEntry.SetActive(false);
        loginType = "New";
    }

    public void toNewDevice()
    {
        createOrLogin.SetActive(false);
        emailEntry.SetActive(true);
        nameemailEntry.SetActive(false);
        codeEntry.SetActive(false);
        loginType = "Existing";
    }

    public void toVerifyCode()
    {
        createOrLogin.SetActive(false);
        emailEntry.SetActive(false);
        nameemailEntry.SetActive(false);
        codeEntry.SetActive(true);
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

    public void verifyExistingEmail()
    {
        if (existingEmail.text != "")
        {
            colRef.Document(existingEmail.text).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Dictionary<string, object> user = snapshot.ToDictionary();
                    existingDisplayName = user["Display Name"].ToString();
                    string result = newEmailCode(existingEmail.text);
                    toVerifyCode();
                }
                else
                {
                    warningText.text = "No account associated with this email was found.";
                    warning.SetActive(true);
                }
            }); 
        }
        else {
            warningText.text = "Please enter email.";
            warning.SetActive(true);
        }
    }

    public void verifyNewEmail()
    {
        if (newEmail.text != "" && newDisplayName.text != "")
        {
            colRef.Document(newEmail.text).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    warningText.text = "An account with this email already exists.";
                    warning.SetActive(true);
                }
                else
                {
                    string result = newEmailCode(newEmail.text);
                    if (result == "Success")
                    {
                        toVerifyCode();
                    }
                    else
                    {
                        warningText.text = "Invalid email.";
                        warning.SetActive(true);
                    }
                }
            });
        }
        else
        {
            warningText.text = "Please enter required fields.";
            warning.SetActive(true);
        }
    }

    public void verifyCode()
    {
        if (emailCode.text == emailPIN.ToString())
        {
            if (loginType == "New")
            {
                string DisplayName = newDisplayName.text;

                DocumentReference newUserRef = db.Document($"Users/{newEmail.text}");
                newUserRef.SetAsync(DisplayName).ContinueWithOnMainThread(task => {
                    Debug.Log("User Successfully Added");
                    PlayerPrefs.SetString("DisplayName", newDisplayName.text);
                    PlayerPrefs.SetString("Email", newEmail.text);
                });
            }
            else if (loginType == "Existing")
            {
                PlayerPrefs.SetString("DisplayName", existingDisplayName);
                PlayerPrefs.SetString("Email", existingEmail.text);
            }

            SceneManager.LoadScene("Joined Group");
        }
        else
        {
            warningText.text = "Code incorrect.";
            warning.SetActive(true);
        }
    }

    public string newEmailCode(string Email)
    {
        emailPIN = UnityEngine.Random.Range(1000, 10000);
        string result = sendMessage.SendEmail(Email, "GroupChore Verification", "Your account confirmation code is: " + emailPIN.ToString());
        return result;
    }

    public void closeWarning()
    {
        warningText.text = "";
        warning.SetActive(false);
    }
}
