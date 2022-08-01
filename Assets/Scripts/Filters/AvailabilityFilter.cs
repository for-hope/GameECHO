using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailabilityFilter
{
   

    public static IDictionary<CommandAction, int> filterByGameState(List<CommandAction> commands)
    {
        IDictionary<CommandAction, int> filtered = new Dictionary<CommandAction, int>();
     
        for (int i = 0; i < commands.Count; i++)
        {
            int score = 0;
            CommandAction ca = commands[i];
            if (ca.isJustLearnt) score += 5;
            if (!ca.isUnknown) score += 1; else score -= 5;
            if (ca.isUsedOnce) score -= 3; else score += 3;
            if (ca.isPossible) score += 1; else score -= 7;
            if (score > 1) filtered.Add(new KeyValuePair<CommandAction, int>(ca, score)); 
        }
        return filtered;
    }

}

public enum Levels
{
    ALL,
    LEVEL1,
    LEVEL2,
    LEVEL3
}


public enum GameState
{
    HAS_MESSABLE_OBJ,
    HAS_DOOR_BAR,
    HAS_GUARDS,
    HAS_GREEN_GUARDS,
    HAS_RED_GUARDS,
    HAS_YELLOW_GUARDS,
    HAS_TERMINAL,
    HAS_DOORS,
    NOT_WIDE_OPEN,
    HAS_OPEN_LIGHT
}

public class CommandsAvailibity
{
    public int commandId;
    public Levels[] levels;
    public GameState[] gState;

    //constructor
    public CommandsAvailibity(int commandId, Levels[] levels, GameState[] gState)
    {
        this.commandId = commandId;
        this.levels = levels;
        this.gState = gState;
    }
  
}