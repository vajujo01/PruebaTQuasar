using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFacingCamera : MonoBehaviour
{
    public Transform cameraMain;
    public TMP_Text text3d;

    void Start()
    {
        text3d.text = GlobalScript.playerName;
    }

    // Update is called once per frame
    void Update()
    {
        text3d.transform.rotation = cameraMain.rotation;
    }
}
