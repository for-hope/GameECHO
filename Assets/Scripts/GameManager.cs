using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    private int LEVELS_COUNT = 4;
    public int currentLevel = 1;
    public static GameManager Instance = null;
    public Animator animator;


    public static bool isVoiceInteractionEnabled = true;
    public bool ignoreLowScopeScores = true;
    public static EnvObjects currentExactEnvObject;
    public static List<EnvObjects> currentFrameEnvObjects;
    public static List<CommandAction> commandActions = new List<CommandAction>();
    public static List<CommandAction> allCommandActions = new List<CommandAction>();
    private Cinemachine.CinemachineVirtualCamera playerCam;
    private Cinemachine.CinemachineVirtualCamera handCam;
    private PlayerInput _playerInput;
    private List<CommandAction> showList = new List<CommandAction>();
    private TMPro.TextMeshProUGUI hintText;
    private bool isLostScreen = false;
    public GameObject IntroScreen;
    public bool isIntroPlaying;
    public bool predictorEnabled = true;
    private bool isPaused = false;
    private GameObject pauseScreen;
    private GameObject startScreen;
    public int targetFrameRate = 60;
    // Start is called before the first frame update
    void Start()
    {
        animator = GameObject.Find("HandCamPrefab").GetComponent<Animator>();
        pauseScreen = GameObject.Find("PauseScreen");
        startScreen = GameObject.Find("StartScreen");

        pauseScreen.SetActive(false);
        playerCam = GameObject.Find("PlayerFollowCamera").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        handCam = GameObject.Find("HandCam").GetComponent<Cinemachine.CinemachineVirtualCamera>();
        currentFrameEnvObjects = new List<EnvObjects>();
        _playerInput = GameObject.Find("PlayerCapsule").GetComponent<PlayerInput>();
        hintText = GameObject.Find("CommandTextHint").GetComponent<TMPro.TextMeshProUGUI>();
        var introSoundFN = Resources.Load<AudioClip>("Sounds/intro");
        IntroScreen = GameObject.Find("IntroContainer").transform.GetChild(0).gameObject;
        SoundManager.Instance.Play(introSoundFN);
        isIntroPlaying = true;
        updateVoiceObjects();
        if (currentLevel == 1)
        {
            GameObject.Find("Level4").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("Level3").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("GlobalLight").transform.GetChild(0).gameObject.SetActive(false);
            //disable room 2 lighting
            GameObject.Find("Room2").transform.GetChild(0).gameObject.SetActive(false);
        }
        Pause();
    }


    // Initialize the singleton instance.
    private void Awake()
    {
        //Target FPS 60
        if (targetFrameRate > 0)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = targetFrameRate;
        }

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

    private void Pause()
    {
        Debug.Log("Paused");
        Time.timeScale = 0;
        _playerInput.enabled = false;
        SoundManager.Instance.EffectsSource.Pause();
        SoundManager.Instance.MusicSource.Pause();
        //show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseScreen.SetActive(true);
        isVoiceInteractionEnabled = false;
        isPaused = true;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Resume()
    {
        Debug.Log("Resumed");
        if (startScreen.activeSelf) startScreen.SetActive(false);
        _playerInput.enabled = true;
        Time.timeScale = 1;
        SoundManager.Instance.EffectsSource.UnPause();
        SoundManager.Instance.MusicSource.UnPause();
        Cursor.visible = false;
        pauseScreen.SetActive(false);
        //hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isVoiceInteractionEnabled = true;
        isPaused = false;
    }

    private void Update()
    {

        if (!SoundManager.Instance.EffectsSource.isPlaying && isIntroPlaying)
        {
            isIntroPlaying = false;
            IntroScreen.SetActive(true);
        }
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

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Space Pressed");
            Debug.Log("Commands count " + commandActions.Count + " All Commands Length " + allCommandActions.Count);
        }

        if (isPaused || startScreen.activeSelf)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //check if ESC is pressed with playerInput
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Debug.Log("ESC pressed");
            if (startScreen.activeSelf) return;
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }

            //Application.Quit();
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
                if (showList.Count > i && !showList[i].isUsedOnce && showList[i].visibility != Visibility.HIDDEN)
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

    public void updateCurrentLevel(int level)
    {
        currentLevel = level;
        updateVoiceObjects();
        filterVoiceCommandsPerLevel();
        Debug.Log("Commands for current level: " + currentLevel + " are: " + commandActions.Count);
        foreach (var command in commandActions)
        {
            Debug.Log(command.phrase);
        }
    }

    private void updateVoiceObjects()
    {
        for (int i = 1; i <= LEVELS_COUNT; i++)
        {
            if (i != currentLevel) enableLevelVoiceObjects(i, false);
            if (i == currentLevel) enableLevelVoiceObjects(i, true);
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
        _playerInput.enabled = !isPutDown;
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

        GameObject gameObject = GameManager.Instance.FindGameObjectWithVAction(objectTag);

        string commandsText = "<color=#ADD8E6><b>Actions:</b></color> \n";
        VAction va = gameObject.GetComponent<VAction>();
        var defaultCommands = va.GetVisibleCommands();
        var inspectedCommands = va.GetInvisibleCommands();
        var hiddenCommands = va.GetHiddenCommands();
        var cmdList = new List<CommandAction>(va.inspect ? defaultCommands.Concat(inspectedCommands).Concat(hiddenCommands).ToList() : defaultCommands);
        GameManager.Instance.showList = new List<CommandAction>(cmdList);
        for (int i = 0; i < cmdList.Count; i++)
        {
            var Fn = i + 1;
            var actionName = cmdList[i].isUsedOnce ? "<s>" + "<color=#FDEB37>[F" + (Fn) + "]</color> " + cmdList[i].actionName + "</s>" : "<color=#FDEB37>[F" + (Fn) + "]</color> " + cmdList[i].actionName;
            commandsText += cmdList[i].visibility == Visibility.HIDDEN ? "<color=#4f504e>[F" + Fn + "]</color> Hidden\n" : actionName + " \n";
        }

        //commandsText += hiddenCommands.Count != 0 && va.inspect ? "+ " + hiddenCommands.Count + " Hidden" : "";
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

    private List<string> levelObjectTags(int level)
    {
        List<string> tags = new List<string>();
        switch (level)
        {
            case 1:
                return System.Enum.GetValues(typeof(Level1Objects)).Cast<Level1Objects>().Select(v => v.ToString().ToLower()).ToList();
            case 2:
                return System.Enum.GetValues(typeof(Level2Objects)).Cast<Level2Objects>().Select(v => v.ToString().ToLower()).ToList();
            case 3:
                return System.Enum.GetValues(typeof(Level3Objects)).Cast<Level3Objects>().Select(v => v.ToString().ToLower()).ToList();
            case 4:
                return System.Enum.GetValues(typeof(Level4Objects)).Cast<Level4Objects>().Select(v => v.ToString().ToLower()).ToList();
        }
        return tags;
    }

    public void AddActionCommands(List<CommandAction> cmds)
    {
        Debug.Log("Adding " + cmds.Count + " cmds with context " + cmds[0].context);
        allCommandActions.AddRange(cmds);
        foreach (CommandAction cmd in cmds)
        {
            if (levelObjectTags(currentLevel).Contains(cmd.context.ToLower()))
            {
                commandActions.Add(cmd);

            }

        }

    }
    private void filterVoiceCommandsPerLevel()
    {
        Debug.Log("Filtering voice commands per level [All commands: " + allCommandActions.Count + "] | commands count before: " + commandActions.Count);
        foreach (var command in allCommandActions)
        {
            Debug.Log("Command: " + command.context.ToLower());
        }
        commandActions = allCommandActions.Where(x => levelObjectTags(currentLevel).Contains(x.context.ToLower())).ToList();
        Debug.Log("commands count after: " + commandActions.Count);
    }
    private void enableLevelVoiceObjects(int level, bool enable)
    {

        var values = System.Enum.GetValues(typeof(Level1Objects));
        if (level == 2)
        {
            values = System.Enum.GetValues(typeof(Level2Objects));
        }
        else if (level == 3)
        {
            values = System.Enum.GetValues(typeof(Level3Objects));
        }
        else if (level == 4)
        {
            values = System.Enum.GetValues(typeof(Level4Objects));
        }

        foreach (var value in values)
        {

            var gameObjects = GameObject.FindGameObjectsWithTag(value.ToString());
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (enable) gameObjects[i].layer = 6;
                else gameObjects[i].layer = 0;
            }


        }

    }

    public GameObject FindGameObjectWithVAction(string tag)
    {
        return GameObject.FindGameObjectsWithTag(tag.ToUpper()).Where(x => x.GetComponent<VAction>() != null).FirstOrDefault();
    }
    public static void TriggerAction(int id, string context)
    {
        Debug.Log("TAG TO INSPECT " + context.ToUpper());
        GameObject gameObjectWithVAction = GameManager.Instance.FindGameObjectWithVAction(context.ToUpper());
        VAction action = gameObjectWithVAction.GetComponent<VAction>();
        action.TriggerAction(id);
    }



}


