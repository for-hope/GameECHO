using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{
    public GameObject objectToTrack;


    private Camera mainCam;
    void Start()
    {

        mainCam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (objectToTrack != null)
        {
            mainCam.transform.LookAt(objectToTrack.transform);
        }
    }
}
