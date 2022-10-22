using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophonePicker : MonoBehaviour
{
    // Start is called before the first frame update
    //drop down textmeshpro
    private TMPro.TMP_Dropdown dropdown;
    public VoiceProcessor voiceProcessor;
    void Start()
    {
        dropdown = GetComponent<TMPro.TMP_Dropdown>();
        PopulateList();
        dropdown.value = voiceProcessor.CurrentDeviceIndex;

        //onValueChaned SelectMicrophone
        dropdown.onValueChanged.AddListener(delegate
        {
            SelectMicrophone();
        });
        Debug.Log("Current Device Index " + voiceProcessor.CurrentDeviceIndex);
        //set selected option to 1
    }
   

    public void PopulateList()
    {
        dropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (string device in voiceProcessor.Devices)
        {
            options.Add(device);
        }
        dropdown.AddOptions(options);
    }

    public void SelectMicrophone()
    {
        string selectedMicrophone = dropdown.options[dropdown.value].text;
        int micIndex = dropdown.value;
        voiceProcessor.StopRecording();
        voiceProcessor.ChangeDevice(micIndex);
        voiceProcessor.StartRecording();

    }
}
