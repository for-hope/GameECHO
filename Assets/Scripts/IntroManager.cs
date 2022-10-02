using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class IntroCommand
{
    public int id;
    public string playerResponse;
    public AudioClip audioPhrase;

    public string hint = "";
    public bool isDone = false;

    public IntroCommand(int id, AudioClip audioPhrase, string playerResponse)
    {
        this.id = id;
        this.playerResponse = playerResponse;
        this.audioPhrase = audioPhrase;
        this.hint = "Say\n\n '" + playerResponse + "'\n\n to continue";
    }

    public IntroCommand(int id, AudioClip audioPhrase, string playerResponse, string hint)
    {
        this.id = id;
        this.playerResponse = playerResponse;
        this.audioPhrase = audioPhrase;
        this.hint = hint;
    }
}

public class IntroManager : MonoBehaviour
{
    public bool isIntroActive;
    public IntroCommand activeIntroCommand;
    public GameObject introPanel;
    //public Ending? endingToConfirm;

    private Queue<IntroCommand> introCommands = new Queue<IntroCommand>();
    private string audioBasePath = "Sounds/Intro/";
    private bool isIntroAudioPlaying = false;
    private GameObject introTextObject;
    private GameObject introScreen;

    //public bool isEndingConfirmed = false;
    public static IntroManager Instance;


    // Start is called before the first frame update
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
    void Start()
    {
        introScreen = GameObject.Find("Canvas").transform.Find("Intro").gameObject;
        introScreen.SetActive(true);
        introTextObject = GameObject.Find("IntroText");
        introCommands.Enqueue(new IntroCommand(0, Resources.Load<AudioClip>(audioBasePath + "0"), "Yes"));
        introCommands.Enqueue(new IntroCommand(1, Resources.Load<AudioClip>(audioBasePath + "1"), "No"));
        introCommands.Enqueue(new IntroCommand(2, Resources.Load<AudioClip>(audioBasePath + "2"), "Sure"));
        introCommands.Enqueue(new IntroCommand(3, Resources.Load<AudioClip>(audioBasePath + "3"), "Done", "Move the mouse to look around. then say 'Done' to continue."));
        introCommands.Enqueue(new IntroCommand(4, Resources.Load<AudioClip>(audioBasePath + "4"), "Okay"));
        isIntroActive = true;
        NextCommand();
    }


    // Update is called once per frame
    void Update()
    {

        if (isIntroActive)
        {
            if (introCommands.Count >= 2) GameManager.Instance.DisablePlayerControls();
            else GameManager.Instance.EnablePlayerControls();
            if (!SoundManager.Instance.EffectsSource.isPlaying && isIntroAudioPlaying && !GameManager.Instance.isPaused)
            {
                Debug.Log("Intro audio finished");
                //GameManager.isVoiceInteractionEnabled = true;
                StartCommand();
            }
        }


    }

    void StartCommand()
    {
        StartCoroutine(DictationInputManager.StartRecording());
        isIntroAudioPlaying = false;
        activeIntroCommand = introCommands.Dequeue();
        introTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = activeIntroCommand.hint;
    }

    public void NextCommand()
    {
        activeIntroCommand = null;
        introTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = "";
        if (introCommands.Count == 0)
        {
            EndIntro();
            return;
        }
        var nextCmd = introCommands.Peek();
        SoundManager.Instance.Play(nextCmd.audioPhrase);
        isIntroAudioPlaying = true;
    }

    private void EndIntro()
    {
        isIntroActive = false;
        introScreen.SetActive(false);
        StartCoroutine(DictationInputManager.StartRecording());
    }


    // public enum Ending
    // {
    //     ESCAPE_BY_DOOR,
    //     ESCAPE_BY_WINDOW,
    //     ESCAPE_BY_SLEEPING,
    // }
    // public async void ShowEndingConfirmation(Ending ending)
    // {

    //     introScreen.SetActive(true);
    //     var formattedEnding = ending.ToString().Replace("_", " ").ToLower();
    //     introTextObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Are you sure you want to " + formattedEnding + "?\n\nSay 'Yes' to continue or 'No' to cancel.";
    //     endingToConfirm = ending;
    //     //wait for isEndingConfirmed to be true
    //     while (!isEndingConfirmed)
    //     {
    //         await Task.Delay(100);
    //     }
    //     isEndingConfirmed = false;
    // }

    // public void EndingConfirmation(bool isConfirmed)
    // {
    //     isEndingConfirmed = isConfirmed;
    //     if (!isConfirmed)
    //     {
    //         endingToConfirm = null;
    //     }
    //     introScreen.SetActive(false);

    // }
}
