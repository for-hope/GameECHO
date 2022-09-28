using System.Collections.Generic;
using System.Linq;


public class ScopeFilter
{

    public static int MIN_SCOPE_FILTER_SCORE = 12; //lower means consider only more accurate
    public IDictionary<CommandAction, int> filteredCommands = new Dictionary<CommandAction, int>();


    public void AddToFilter(CommandAction cmdAction, string text)
    {
        int scopeScore = LevenshteinDistance.Compute(cmdAction.phrase, text);
        if (scopeScore >= MIN_SCOPE_FILTER_SCORE && GameManager.Instance.ignoreLowScopeScores) return;
        filteredCommands[cmdAction] = scopeScore;

    }


    public int CalculateScore(CommandAction cmdAction)
    {
        if (filteredCommands.ContainsKey(cmdAction)) return System.Convert.ToInt32((ScopeFilter.MIN_SCOPE_FILTER_SCORE - filteredCommands[cmdAction] + 1) * 4); ;
        return 0;
    }
    public KeyValuePair<CommandAction, int> BestScoreCommand()
    {
        return filteredCommands.OrderBy(x => x.Value).FirstOrDefault();
    }


}
