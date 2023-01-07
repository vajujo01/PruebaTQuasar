using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePage : MonoBehaviour
{
    //Public variables
    public GameObject login;
    public GameObject register;
    public GameObject message;

    
    //Change between login and register page
    public void ChangeActive()
    {
        login.SetActive(!login.activeSelf);
        register.SetActive(!register.activeSelf);
    }

    public void OpenCloseMessage()
    {
        message.SetActive(!message.activeSelf);
    }
}
