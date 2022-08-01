using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cinemachine;

public class PlayerNavMesh : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private GameObject target;
    private CinemachineVirtualCamera objectCam;
    private CinemachineVirtualCamera playerCam;
    private Camera mainCamera; 
    private bool isStopped = false;
    private  GameObject transformCenter;
    public delegate void OnReachedTargetDelegate();
    public OnReachedTargetDelegate OnReachedTarget;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        objectCam = GameObject.Find("ObjectLookAtCamera").GetComponent<CinemachineVirtualCamera>();
        playerCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        transformCenter = new GameObject("TransformCenter");
    }
    
    private void Update()
    {
        if (target == null) return;
        navMeshAgent.destination = target.GetComponent<Renderer>().bounds.center;
        transformCenter.transform.position = target.GetComponent<Renderer>().bounds.center;
        objectCam.LookAt = transformCenter.transform;
        // Debug.Log("Position: " + target.transform.position);
        // Debug.Log("Bounds: " + target.GetComponent<Renderer>().bounds.center);
        bool pathPending = navMeshAgent.pathPending;
        if (!pathPending)
        {
            if(objectCam.Priority < playerCam.Priority) SwitchCamPriority();
            if (navMeshAgent.remainingDistance <= 0.15f )
            {
                if (objectCam.Priority > playerCam.Priority) SwitchCamPriority();
                target = null;
                if (OnReachedTarget != null) OnReachedTarget();
                Debug.Log("Agent Stopped!!!!!!!!");
            }
        }




    }
    

    public void GoToTarget(GameObject target, OnReachedTargetDelegate onReachedTarget)
    {
        this.target = target;
        this.OnReachedTarget = onReachedTarget;
    }

    private void SwitchCamPriority()
    {
        if (objectCam.Priority > playerCam.Priority)
        {
            objectCam.Priority = 0;
            playerCam.Priority = 1;
            return;

        }

        objectCam.Priority = 1;
        playerCam.Priority = 0;
    }
}
