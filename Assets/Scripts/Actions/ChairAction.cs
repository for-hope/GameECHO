using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChairAction : MonoBehaviour
{
    private static string TAG = "Chair";
    // Start is called before the first fframe update
    public bool inspect;
    public bool tryBreak;
    public bool takeOutPipe;
    private CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the chair");





    void Start()
    {
        inspect = false;
        GameManager.commandActions.Add(cmdAction);

    }

    public static void triggerAction(int commandId)
    {
        GameObject gameObject = GameObject.Find(TAG);
        ChairAction chairAction = gameObject.GetComponent<ChairAction>();
        if (commandId == 0)
        {
            chairAction.Inspect();
        }
    }

    public void Inspect()
    {
        Debug.Log("Inspecting The chair");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a23");
        SoundManager.Instance.Play(DialogAudio);
        VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        vo.isInspected = true;

    }

}
