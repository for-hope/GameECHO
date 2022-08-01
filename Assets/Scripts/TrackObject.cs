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
            Debug.Log("Make camera look at objectToTrack");

            mainCam.transform.LookAt(objectToTrack.transform);
        }
    }
}
