using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScopeFilter
{

    public static int MIN_SCOPE_FILTER_SCORE = 11;
    public IDictionary<CommandAction, int> filteredCommands = new Dictionary<CommandAction, int>();


    public void Filter(CommandAction cmdAction, string text)
    {
        int scopeScore = LevenshteinDistance.Compute(cmdAction.phrase, text);
        if (scopeScore >= MIN_SCOPE_FILTER_SCORE && GameManager.Instance.ignoreLowScopeScores) return;
        filteredCommands[cmdAction] = scopeScore;
        //filteredCommands.Add(new KeyValuePair<CommandAction, int>(cmdAction, scopeScore));
    }

    public KeyValuePair<CommandAction, int> BestScoreCommand()
    {
        return filteredCommands.OrderBy(x => x.Value).FirstOrDefault();
    }


}
