using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{



    public static GameManager Instance = null;
    Animator animator;


    public static bool isVoiceInteractionEnabled = true;
    public bool ignoreLowScopeScores = true;
    public static EnvObjects currentExactEnvObject;
    public static List<EnvObjects> currentFrameEnvObjects;
    public static List<CommandAction> commandActions = new List<CommandAction>();
    private Cinemachine.CinemachineVirtualCamera playerCam;
    private Cinemachine.CinemachineVirtualCamera handCam;
    private PlayerInput _playerInput;
    private List<CommandAction> showList = new List<CommandAction>();
    private TMPro.TextMeshProUGUI hintText;
    private bool isLostScreen = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("HandCamPrefab").GetComponent<Animator>();
        playerCam = GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        handCam = GameObject.Find("HandCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        currentFrameEnvObjects = new List<EnvObjects>();
        _playerInput = GameObject.Find("PlayerCapsule").GetComponent<PlayerInput>();
        hintText = GameObject.Find("CommandTextHint").GetComponent<TMPro.TextMeshProUGUI>();
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
            if (playerCam.Priority > handCam.Priority)
            {
                handCam.Priority = playerCam.Priority + 1;
            }
        }
        else if (handCam != null)
        {
            if (handCam.Priority > playerCam.Priority)
            {
                playerCam.Priority = handCam.Priority - 1;
                handCam.Priority = -1;
            }
        }

        //check if ESC is pressed with playerInput
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("ESC pressed");
            Application.Quit();
        }

        List<bool> FnPressed = new List<bool>(){
            Keyboard.current.f1Key.wasPressedThisFrame,
            Keyboard.current.f2Key.wasPressedThisFrame,
            Keyboard.current.f3Key.wasPressedThisFrame,
            Keyboard.current.f4Key.wasPressedThisFrame,
            Keyboard.current.f5Key.wasPressedThisFrame,
            Keyboard.current.f6Key.wasPressedThisFrame,
            Keyboard.current.f7Key.wasPressedThisFrame,
            Keyboard.current.f8Key.wasPressedThisFrame,
            Keyboard.current.f9Key.wasPressedThisFrame,
            Keyboard.current.f10Key.wasPressedThisFrame,
            Keyboard.current.f11Key.wasPressedThisFrame,
            Keyboard.current.f12Key.wasPressedThisFrame
        };

        for (int i = 0; i < FnPressed.Count; i++)
        {
            if (FnPressed[i])
            {
                if (showList.Count > i && !showList[i].isUsedOnce)
                {
                    hintText.text = "[ Hint: \"" + showList[i].phrase + "\" ]";
                    StartCoroutine(resetHintText());
                }

            }
        }

        if (isLostScreen && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isLostScreen = false;
            GameObject.Find("LostScreen").SetActive(false);
        }


    }

    IEnumerator resetHintText()
    {
        yield return new WaitForSeconds(6);
        hintText.text = "";
    }

    public void Lose()
    {

        GameObject canvas = GameObject.Find("Canvas");

        canvas.transform.GetChild(canvas.transform.childCount - 1).gameObject.SetActive(true);
        isLostScreen = true;
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
            commandsTextComponent.GetComponent<TMPro.TextMeshProUGUI>().text = "";
            return;
        }

        GameObject gameObject = GameObject.FindGameObjectsWithTag(objectTag).Where(x => x.GetComponent<VAction>() != null).FirstOrDefault();

        string commandsText = "<color=#ADD8E6><b>Actions:</b></color> \n";
        VAction va = gameObject.GetComponent<VAction>();
        var defaultCommands = va.GetVisibleCommands();
        var inspectedCommands = va.GetInvisibleCommands();
        var hiddenCommands = va.GetHiddenCommands();
        var cmdList = new List<CommandAction>(va.inspect ? defaultCommands.Concat(inspectedCommands).ToList() : defaultCommands);
        GameManager.Instance.showList = new List<CommandAction>(cmdList);
        for (int i = 0; i < cmdList.Count; i++)
        {
            var Fn = i + 1;
            var actionName = cmdList[i].isUsedOnce ? "<s>" + "<color=#FDEB37>[F" + (Fn) + "]</color> " + cmdList[i].actionName + "</s>" : "<color=#FDEB37>[F" + (Fn) + "]</color> " + cmdList[i].actionName;
            commandsText += actionName + " \n";
        }

        commandsText += hiddenCommands.Count != 0 && va.inspect ? "+ " + hiddenCommands.Count + " Hidden" : "";
        commandsTextComponent.GetComponent<TMPro.TextMeshProUGUI>().text = commandsText;
    }


    public static void RevealHiddenCommandsOfAction(List<CommandAction> cmds)
    {


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


