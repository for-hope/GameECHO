using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFilter : MonoBehaviour
{
    public static List<CommandAction> filteredByEnv(List<CommandAction> commandActions)
    {
        List<CommandAction> filtered = new List<CommandAction>();

        foreach (CommandAction command in commandActions)
        {
            EnvObjects envObject = CommandAction.tagToEnvObj[command.context.ToLower()];

            if (GameManager.currentExactEnvObject == envObject)
            {
                filtered.Add(command);
            }
        }
        return filtered;

    }

    public static Dictionary<CommandAction, EnvObjects> filterByFrameEnv(List<CommandAction> commandActions)
    {
        var filtered = new Dictionary<CommandAction, EnvObjects>();
        foreach (CommandAction command in commandActions)
        {
            EnvObjects envObject = CommandAction.tagToEnvObj[command.context.ToLower()];

            if (GameManager.currentFrameEnvObjects.Contains(envObject))
            {
                filtered.Add(command, envObject);
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

