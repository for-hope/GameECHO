using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvFilter : MonoBehaviour
{
    public static List<CommandAction> filteredByEnv(List<CommandAction> commandActions)
    {
        List<CommandAction> filtered = new List<CommandAction>();
        foreach (CommandAction command in commandActions)
        {
            Debug.Log("KEY: " + command.context);
            EnvObjects envObject = CommandAction.tagToEnvObj[command.context.ToLower()];
            
                if (GameManager.currentExactEnvObject == envObject)
                {
                    filtered.Add(command);
                }
            

        }
        return filtered;

    }

    public static List<CommandAction> filterByFrameEnv(List<CommandAction> commandActions)
    {
        List<CommandAction> filtered = new List<CommandAction>();
        foreach (CommandAction command in commandActions)
        {
            EnvObjects envObject = CommandAction.tagToEnvObj[command.context.ToLower()];

            if (GameManager.currentFrameEnvObjects.Contains(envObject))
            {
                filtered.Add(command);
                
            }


        }
        return filtered;
    }
}

public enum EnvObjects
{
    DOORS,
    WALLS,
    BUCKETS,
    CHAIR,
    TABLE,
    BED,
    TOILET,
    HANGED_CLOTHES,
    LIGHT,
    TRASH,
    METAL_BOX
}

public class CommandsEnv
{
    public int commandId;
    public EnvObjects[] envObjects;

    public CommandsEnv(int commandId, EnvObjects[] envObjects)
    {
        this.commandId = commandId;
        this.envObjects = envObjects;

    }

}