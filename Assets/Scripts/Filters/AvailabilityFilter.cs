using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailabilityFilter
{


    public static IDictionary<CommandAction, int> filterByContext(List<CommandAction> commands)
    {
        IDictionary<CommandAction, int> filteredCommands = new Dictionary<CommandAction, int>();

        foreach (CommandAction ca in commands)
        {
            int score = 0; //max 30
            if (ca.isJustLearnt) score += 15;
            if (!ca.isUnknown) score += 4; else score -= 12;
            if (ca.isUsedOnce) score -= 8; else score += 7;
            if (ca.isPossible) score += 4; else score -= 10;
            //add is next on the list
            if (score > 0) filteredCommands.Add(new KeyValuePair<CommandAction, int>(ca, score));
        }
        return filteredCommands;
    }

}
