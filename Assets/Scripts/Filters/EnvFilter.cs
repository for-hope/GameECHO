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
            Debug.Log("Key Given " + command.context.ToLower());
            EnvObjects envObject = CommandAction.tagToEnvObj[command.context.ToLower()];

            if (GameManager.currentExactEnvObject == envObject)
            {
                Debug.Log("ENV OBJECT BASED ON RAY " + envObject);
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
                Debug.Log("ENV OBJECT IN FRAME " + envObject);
                filtered.Add(command, envObject);
            }


        }
        return filtered;
    }


}


public enum EnvObjects
{
    DOOR,
    WALLS,
    BUCKETS,
    CHAIR,
    TABLE,
    BED,
    TOILET,
    HANGED_CLOTHES,
    LIGHT,
    TRASH,
    METAL_BOX,

    METAL_DOOR,
    SMALL_BATHROOM,
    WIRES,
    BOARD,
    BRUSH,
    LOCKER,
    WHITE_DOOR,
    ORANGE_DOOR,
    MIRROR,
    SINK,
    TRASHCAN,
    ALARM,
    FIREPLUG,
    FIRE_EXTINGUISHER,
    HALL_WINDOWS,
    CLASSROOM,
    BLACKBOARD,
    SCREEN,
    CLASSROOM_DOOR,
    CLOCK,
    DESKS,
    BACKPACKS,
    NOTEBOOKS,
    POSTERS,
    BROOM,
    SPEAKER,
    CLASSROOM_WINDOWS
}

enum Level1Objects
{

    DOOR,
    WALLS,
    BUCKETS,
    CHAIR,
    TABLE,
    BED,
    TOILET,
    HANGED_CLOTHES,
    LIGHT,
    TRASH,
    METAL_BOX,

    METAL_DOOR,

}

enum Level2Objects
{
    SMALL_BATHROOM,
    WIRES,
    BOARD,
    BRUSH,
    LOCKER,
    WHITE_DOOR,
    ORANGE_DOOR,
    MIRROR,
    SINK,
    TRASHCAN,
    ALARM
}


enum Level3Objects
{
    FIREPLUG,
    FIRE_EXTINGUISHER,
    HALL_WINDOWS,
    CLASSROOM,
}

enum Level4Objects
{
    BLACKBOARD,
    SCREEN,
    CLASSROOM_DOOR,
    CLOCK,
    BACKPACKS,
    NOTEBOOKS,
    POSTERS,
    BROOM,
    SPEAKER,
    CLASSROOM_WINDOW
}

