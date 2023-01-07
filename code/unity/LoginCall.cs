using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LoginCall : MonoBehaviour
{
    //Public variables
    public TMP_InputField email;
    public TMP_InputField password;
    public GameObject message;
    public TMP_Text mText;

    //Private variables
    private string url;

    //Call to the symfony API for LogIn
    public void LogIn()
    {
        url = "http://127.0.0.1:8000/api/get_user?";
        //First we check the email
        if (email.text != "" && Regex.IsMatch (email.text, GlobalScript.MatchEmailPattern))
        {
            url += "email=" + email.text;
            if (password.text != "")
            {
                url += "&password=" + password.text;
            }
            //We let the API return the errors or the correct answer
            Debug.Log(url);
            UnityWebRequest request = UnityWebRequest.Get(url);
            StartCoroutine(OnResponseL(request));
        }
        else
        {
            mText.text = "Write a valid email address";
            message.SetActive(true);
        }
        
    }

    //Wait for the response
    private IEnumerator OnResponseL(UnityWebRequest req)
    {
        yield return req.SendWebRequest();
        if (req.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            ResponseAPIL responseApi = JsonUtility.FromJson<ResponseAPIL> (req.downloadHandler.text);
            //Error
            if(responseApi.code != 200)
            {
                mText.text = responseApi.data.message;
                message.SetActive(true);
            }
            //Correct
            else
            {
                Debug.Log(responseApi.data.nombre);
                GlobalScript.playerName = responseApi.data.nombre;
                //We load the new scene
                SceneManager.LoadScene("Playground");
            }
        }
        
    }
}

//Nested class for the api response
[System.Serializable]
public class DataApiL
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
public class ResponseAPIL
{
    public bool success;
    public int code;
    public DataApiL data;
}    
