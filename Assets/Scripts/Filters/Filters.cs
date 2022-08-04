using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Filters
{

    public static IDictionary<CommandAction, int> filterByContext(List<CommandAction> commands)
    {
        IDictionary<CommandAction, int> filteredCommands = new Dictionary<CommandAction, int>();

        foreach (CommandAction ca in commands)
        {
            int score = 0;
            if (ca.isJustLearnt) score += 5;
            if (!ca.isUnknown) score += 1; else score -= 5;
            if (ca.isUsedOnce) score -= 3; else score += 3;
            if (ca.isPossible) score += 1; else score -= 7;
            if (score > 1) filteredCommands.Add(new KeyValuePair<CommandAction, int>(ca, score));
        }
        return filteredCommands;
    }

}
