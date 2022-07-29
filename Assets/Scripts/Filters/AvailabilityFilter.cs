using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailabilityFilter
{
    
    static public CommandsAvailibity[] commandsAvailibiliy = new CommandsAvailibity[]
    {
        new CommandsAvailibity(1, new[]{Levels.LEVEL1, } , new[]{GameState.HAS_MESSABLE_OBJ}),
        new CommandsAvailibity(2,new Levels[]{Levels.LEVEL1},new GameState[]{GameState.HAS_DOOR_BAR}),
        new CommandsAvailibity(3, new Levels[]{Levels.LEVEL1}, new GameState[]{GameState.HAS_GREEN_GUARDS, GameState.HAS_GUARDS}),
        new CommandsAvailibity(4, new Levels[]{Levels.LEVEL1}, new GameState[]{GameState.HAS_RED_GUARDS, GameState.HAS_GUARDS}),
        new CommandsAvailibity(5, new Levels[]{Levels.LEVEL1}, new GameState[]{GameState.HAS_GREEN_GUARDS, GameState.HAS_GUARDS}),
        new CommandsAvailibity(6, new Levels[]{Levels.LEVEL1}, new GameState[]{GameState.HAS_RED_GUARDS, GameState.HAS_GUARDS}),
        new CommandsAvailibity(7, new Levels[]{Levels.LEVEL1}, new GameState[]{GameState.HAS_TERMINAL}),
        new CommandsAvailibity(8, new Levels[]{Levels.LEVEL1}, new GameState[]{GameState.HAS_DOORS}),
        new CommandsAvailibity(9, new Levels[]{Levels.LEVEL1}, new GameState[]{GameState.NOT_WIDE_OPEN}),
        new CommandsAvailibity(10, new Levels[]{Levels.LEVEL1}, new GameState[]{GameState.HAS_OPEN_LIGHT })

    };

    public static List<int> filterByAv()
    {
        List<int> commandsAvailibiliyFiltered = new List<int>();
       
        for (int i = 0; i < commandsAvailibiliy.Length; i++)
        {
            CommandsAvailibity ca = commandsAvailibiliy[i];
            List<Levels> levels = new List<Levels>(ca.levels);
            List<GameState> gameState = new List<GameState>(ca.gState);
            bool hasGameState = false;
            for (int j = 0; j < gameState.Count; j++)
            {
                if (GameManager.currentGameState.Contains(gameState[j]))
                {
                    hasGameState = true;
                    break;
                }
            }
            if (levels.Contains(GameManager.currentLevels) && hasGameState)
            {
                commandsAvailibiliyFiltered.Add(ca.commandId);
            }
         
          
        }
        return commandsAvailibiliyFiltered;
    }
    
    public static IDictionary<CommandAction, int> filterByGameState(List<CommandAction> commands)
    {
        IDictionary<CommandAction, int> filtered = new Dictionary<CommandAction, int>();
     
        for (int i = 0; i < commands.Count; i++)
        {
            int score = 0;
            CommandAction ca = commands[i];
            Debug.Log("ca.commandId: " + ca.phrase + " just learn? " + ca.isJustLearnt + " isUnknown " + ca.isUnknown + " isUsedOnce " + ca.isUsedOnce + " isPossible" + ca.isPossible);
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