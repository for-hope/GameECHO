using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandAction
{
    public int id;
    public string context;
    public string phrase;
    public bool isUsedOnce = false;
    public bool isPossible = true;
    public bool isUnknown = false;
    public bool isJustLearnt = false;


    public CommandAction(int id, string context, string phrase)
    {
        this.id = id;
        this.context = context;
        this.phrase = phrase;
        

    }

    //equals
    

    public static Dictionary<string, EnvObjects> tagToEnvObj = new Dictionary<string, EnvObjects>()
    {
        {"door", EnvObjects.DOORS },
        {"wall", EnvObjects.WALLS },
        {"bucket", EnvObjects.BUCKETS },
        {"chair", EnvObjects.CHAIR },
        {"table", EnvObjects.TABLE },
        {"bed", EnvObjects.BED },
        {"toilet", EnvObjects.WC },
        {"hanged clothes", EnvObjects.HANGED_CLOTHES },
        {"light", EnvObjects.LIGHT },
        {"trash", EnvObjects.TRASH },
        {"metal box", EnvObjects.METAL_BOX }

    };

    public override bool Equals(object obj)
    {
        return obj is CommandAction action &&
               id == action.id && context == action.context;
    }

    public override int GetHashCode()
    {
        int hashCode = -59994532;
        hashCode = hashCode * -1521134295 + id.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(context);
        return hashCode;
    }
}

