using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentFilter
{

    private List<CommandAction> commandsBasedOnRaycast = new List<CommandAction>();
    private List<CommandAction> commandsBasedOnFrame = new List<CommandAction>();
    public int uniqueFrameObjectsCount = 0;
    public void AddToFilter(List<CommandAction> commandActions)
    {
        //clear commandsBasedOnRaycast
        commandsBasedOnRaycast = new List<CommandAction>();
        commandsBasedOnFrame = new List<CommandAction>();
        uniqueFrameObjectsCount = 0;
        var uniqueEnvOjs = new HashSet<EnvObjects>();
        foreach (CommandAction command in commandActions)
        {

            EnvObjects envObject = CommandAction.tagToEnvObj[command.context.ToLower()];

            if (GameManager.currentExactEnvObject == envObject)
            {
                Debug.Log("OBJECT BASED ON RAY " + envObject);
                commandsBasedOnRaycast.Add(command);
            }
            if (GameManager.currentFrameEnvObjects.Contains(envObject))
            {
                Debug.Log("ENV OBJECT IN FRAME " + envObject);
                commandsBasedOnFrame.Add(command);
                uniqueEnvOjs.Add(envObject);
            }
            uniqueFrameObjectsCount = uniqueEnvOjs.Count;
        }

    }

    public int CalculateScore(CommandAction command)
    {
        int score = 0;
        if (commandsBasedOnRaycast.Contains(command)) score += 20;
        if (commandsBasedOnFrame.Contains(command)) score += Mathf.RoundToInt(20 / uniqueFrameObjectsCount);
        return score;
    }

    public int getCommandScore(CommandAction command)
    {
        var score = 0;
        if (commandsBasedOnRaycast.Contains(command)) score += 20;
        if (commandsBasedOnFrame.Contains(command)) score += 10 / uniqueFrameObjectsCount;
        return score;
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
    DESKS,
    BACKPACKS,
    NOTEBOOKS,
    POSTERS,
    BROOM,
    SPEAKER,
    CLASSROOM_WINDOWS
}

