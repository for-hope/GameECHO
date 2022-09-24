using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextFilter
{

    private IDictionary<CommandAction, int> commandsBasedOnContext = new Dictionary<CommandAction, int>();
    public void AddToFilter(List<CommandAction> commands)
    {

        foreach (CommandAction ca in commands)
        {
            int score = 0; //max 30
            if (ca.isJustLearnt) score += 15;
            if (!ca.isUnknown) score += 4; else score -= 12;
            if (ca.isUsedOnce) score -= 8; else score += 7;
            if (ca.isPossible) score += 4; else score -= 10;
            //add is next on the list
            if (score > 0) commandsBasedOnContext.Add(new KeyValuePair<CommandAction, int>(ca, score));
        }

    }

    public int CalculateScore(CommandAction command)
    {
        if (commandsBasedOnContext.ContainsKey(command)) return commandsBasedOnContext[command];
        return 0;
    }

}
