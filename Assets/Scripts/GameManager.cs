using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class GameManager : MonoBehaviour
{

    public static Levels currentLevels;
    public static List<GameState> currentGameState;
    public static EnvObjects currentExactEnvObject;
    public static List<EnvObjects> currentFrameEnvObjects;
    public static List<CommandAction> commandActions = new List<CommandAction>();
    // Start is called before the first frame update
    void Start()
    {
        currentLevels = Levels.LEVEL1;
        currentGameState = new List<GameState>
        {
           GameState.HAS_MESSABLE_OBJ,
           GameState.HAS_DOOR_BAR,
           GameState.HAS_GUARDS,
           GameState.HAS_GREEN_GUARDS,
           GameState.HAS_RED_GUARDS,
           GameState.HAS_TERMINAL,
           GameState.HAS_DOORS,
           GameState.NOT_WIDE_OPEN
        };

       
        currentFrameEnvObjects = new List<EnvObjects>
        {

        };
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
        VoiceObject vo = gameObject.GetComponent<VoiceObject>();
        string[] defaultCommands = vo.defaultCommands;
        string[] inspectedCommands = vo.inspectedCommands;
        string[] hiddenCommands = vo.hiddenCommands;
        List<string> cmdList = new List<string>(vo.isInspected ? inspectedCommands : defaultCommands);
        for (int i = 0; i < cmdList.Count; i++)
        {
            commandsText += cmdList[i] + " \n";
        }
        commandsText += hiddenCommands.Length != 0 && vo.isInspected ? "+ " + hiddenCommands.Length + " Hidden Actions" : "";
        commandsTextComponent.GetComponent<UnityEngine.UI.Text>().text = commandsText;
    }


    public static void LearnCommands(List<CommandAction> cmds)
    {
        for (int i = 0; i < GameManager.commandActions.Count; i++)
        {
            CommandAction gmCmdAction = GameManager.commandActions[i];
            for (int j = 0; j < cmds.Count; j++)
            {
                if (gmCmdAction != cmds[j])
                {
                    GameManager.commandActions[i].isJustLearnt = false;
                    continue;
                }
                if (cmds[j].id == 0)
                {
                    GameManager.commandActions[i].isUsedOnce = true;
                }
                else
                {
                    GameManager.commandActions[i].isJustLearnt = !GameManager.commandActions[i].isUnknown;
                    GameManager.commandActions[i].isUnknown = false;
                }

            }
        }
    }

    public static void TriggerAction(int id, string context)
    {
        Debug.Log("TAG TO INSPECT " + context.ToUpper());
        GameObject go = GameObject.FindGameObjectWithTag(context.ToUpper());
        VAction action = go.GetComponent<VAction>();
        Debug.Log("Action Triggered! " + id + " " + context);
        action.TriggerAction(id);
    }

}


