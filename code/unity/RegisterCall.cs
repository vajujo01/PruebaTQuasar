using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class RegisterCall : MonoBehaviour
{
    //Public variables
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField name_;
    public TMP_InputField last_name;
    public TMP_Text mText;
    public GameObject login;
    public GameObject register;
    public GameObject message;

    //Private variables
    private string url;

    //Call to the symfony API for Register
    public void Register()
    {
        url = "http://127.0.0.1:8000/api/create_user?";
        //First we check the email
        if (email.text != "" && Regex.IsMatch (email.text, GlobalScript.MatchEmailPattern))
        {
            url += "email=" + email.text;
            //Second the password
            if (Regex.IsMatch (password.text, GlobalScript.MatchPasswordPattern))
            {
                url += "&password=" + password.text;
                if(name_.text != "" && last_name.text != "")
                {
                    url += "&name=" + name_.text + "&last_name=" + last_name.text;
                }
                //We let the API return the errors or the correct answer
                Debug.Log(url);
                UnityWebRequest request = UnityWebRequest.Get(url);
                StartCoroutine(OnResponseR(request));
            }
            else
            {
                mText.text = "Write a valid password";
                message.SetActive(true);
            }
        }
        else
        {
            mText.text = "Write a valid email address";
            message.SetActive(true);
        }
        
    }

    //Wait for the response
    private IEnumerator OnResponseR(UnityWebRequest req)
    {
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            ResponseAPIR responseApi = JsonUtility.FromJson<ResponseAPIR> (req.downloadHandler.text);
            //Error
            if(responseApi.code != 200)
            {
                mText.text = responseApi.data.message;
                message.SetActive(true);
            }
            //Correct
            else
            {
                //Changing message
                mText.text = responseApi.data.message;
                GlobalScript.playerName = name_.text;
                //Clear the inputs
                email.text = "";
                password.text = "";
                name_.text = "";
                last_name.text = "";
                //Active gameObjects for the user to login with the new user
                login.SetActive(true);
                register.SetActive(false);
                message.SetActive(true);
            }
        }
        
    }
}

//Nested class for the api response
[System.Serializable]
public class DataApiR
{
    public int id;
    public string message;
    public string created_at;
    public string nombre;
    public string apellidos;
    public string correo;
    public string password;
}

[System.Serializable]
public class ResponseAPIR
{
    public bool success;
    public int code;
    public DataApiR data;
}    
