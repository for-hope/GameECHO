using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableAction : MonoBehaviour
{
    private static string TAG = "Table";
    // Start is called before the first fframe update
    public bool inspect;
    public bool tryBreak;
    public bool takeOutPipe;
    private CommandAction cmdAction = new CommandAction(0, TAG, "Inspect the table");





    void Start()
    {
        inspect = false;
        GameManager.commandActions.Add(cmdAction);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void triggerAction(int commandId)
    {
        GameObject gameObject = GameObject.Find(TAG);
        TableAction tableAction = gameObject.GetComponent<TableAction>();
        if (commandId == 0)
        {
            tableAction.Inspect();
        }
    }

    public void Inspect()
    {
        Debug.Log("Inspecting The table");
        AudioClip DialogAudio = Resources.Load<AudioClip>("Sounds/a24");
        SoundManager.Instance.Play(DialogAudio);
        VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        vo.isInspected = true;

    }

}
