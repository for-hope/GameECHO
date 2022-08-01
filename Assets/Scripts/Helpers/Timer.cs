using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    UnityEngine.UI.Text timerText;
    void Awake()
    {
        timerText = gameObject.GetComponent<UnityEngine.UI.Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerText == null) return;
        int time = (int)Time.timeSinceLevelLoad;
        int hours = time / 3600;
        int minutes = (time % 3600) / 60;
        int seconds = time % 60;
        timerText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);

    }
}
