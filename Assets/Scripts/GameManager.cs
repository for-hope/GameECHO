using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class GameManager : MonoBehaviour
{



    public static GameManager Instance = null;
    Animator animator;


    public bool ignoreLowScopeScores = true;
    public static EnvObjects currentExactEnvObject;
    public static List<EnvObjects> currentFrameEnvObjects;
    public static List<CommandAction> commandActions = new List<CommandAction>();
   private Cinemachine.CinemachineVirtualCamera playerCam;
     private Cinemachine.CinemachineVirtualCamera handCam;
    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("HandCamPrefab").GetComponent<Animator>();
        playerCam = GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        handCam = GameObject.Find("HandCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        currentFrameEnvObjects = new List<EnvObjects>();
    }

    // Initialize the singleton instance.
    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
       //check if animator is animating
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("PutDownCam") || animator.GetCurrentAnimatorStateInfo(0).IsName("PutUp"))
        {
            if (playerCam.Priority > handCam.Priority) {
                handCam.Priority = playerCam.Priority + 1; 
            }
        } else if (handCam != null) {
            if (handCam.Priority > playerCam.Priority) {
                playerCam.Priority = handCam.Priority - 1;
                handCam.Priority = -1;
            }
        }
    }

    public void putDownCamera(bool isPutDown)
    {
        Debug.Log("putDownCamera " + isPutDown);
        animator.SetBool("putDown", isPutDown);

    }

    public static void updateCommandsList(string objectTag)
    {
        GameObject commandsTextComponent = GameObject.Find("CommandsListText");

        if (objectTag == "")
        {
            commandsTextComponent.GetComponent<UnityEngine.UI.Text>().text = "";
            return;
        }
        GameObject gameObject = GameObject.FindGameObjectWithTag(objectTag);

        string commandsText = "Actions: \n";
        //VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        VAction va = gameObject.GetComponent<VAction>();
        List<string> defaultCommands = va.GetVisibleCommands().Select(x => x.actionName).ToList();
        List<string> inspectedCommands = va.GetInvisibleCommands().Select(x => x.actionName).ToList();
        List<string> hiddenCommands = va.GetHiddenCommands().Select(x => x.actionName).ToList();
        List<string> cmdList = new List<string>(va.inspect ? defaultCommands.Concat(inspectedCommands).ToList() : defaultCommands);
        for (int i = 0; i < cmdList.Count; i++)
        {
            commandsText += cmdList[i] + " \n";
        }
        commandsText += hiddenCommands.Count != 0 && va.inspect ? "+ " + hiddenCommands.Count + " Hidden" : "";
        commandsTextComponent.GetComponent<UnityEngine.UI.Text>().text = commandsText;
    }


    public static void RevealHiddenCommandsOfAction(List<CommandAction> cmds)
    {
        foreach (CommandAction cmd in cmds)
        {
            Debug.Log("Revealing hidden command: " + cmd.phrase);
        }

        //iterate through the commands of GameManager.
        foreach (CommandAction gmCommand in commandActions)
        {

            if (cmds.Contains(gmCommand))
            {
                if (gmCommand.id == 0) gmCommand.isUsedOnce = true;
                else
                {
                    gmCommand.isJustLearnt = true;
                    gmCommand.isUnknown = false;
                }
            }
            else
            {
                gmCommand.isJustLearnt = false;
            }

        }

        foreach (CommandAction gmCommand in commandActions)
        {
            Debug.Log("gmCommand: " + gmCommand.phrase + " just learnt" + gmCommand.isJustLearnt);
        }

    }

    public static void TriggerAction(int id, string context)
    {
        Debug.Log("TAG TO INSPECT " + context.ToUpper());
        GameObject go = GameObject.FindGameObjectWithTag(context.ToUpper());
        VAction action = go.GetComponent<VAction>();
        action.TriggerAction(id);
    }

}


