using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTick : MonoBehaviour
{
    UnityEngine.UI.Image circleTick;
    void Awake()
    {
        circleTick = gameObject.GetComponent<UnityEngine.UI.Image>();
        InvokeRepeating("TickCircle", 0.5f, 0.9f);
    }

    // Update is called once per frame
    void TickCircle()
    {
        circleTick.enabled = !circleTick.enabled;
    }
}
