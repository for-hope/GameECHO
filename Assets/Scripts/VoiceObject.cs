using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VoiceObject : MonoBehaviour
{ 

    public string objectName;
    public string[] defaultCommands;
    public string[] inspectedCommands;
    public string[] hiddenCommands;
    public bool isInspected;
    public bool isKnown;

    private void Start()
    {
        objectName = gameObject.name;
        isInspected = false;
    }

    private void Update()
    {
        
    }


}

