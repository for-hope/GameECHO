using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Visibility
{
    VISIBLE,
    INVISIBLE,
    HIDDEN
}
public class CommandAction
{
    public static string INSPECT_ACTION_NAME = "Inspect";
    public int id;
    public string context; // example BED or TRASH
    public string phrase;
    public bool isUsedOnce = false;
    public bool isPossible = true;
    public bool isUnknown = false;
    public bool isJustLearnt = false;
    public Visibility visibility = Visibility.VISIBLE;

    public string actionName;




    public CommandAction(int id, string context, string phrase, string actionName, Visibility visibility = Visibility.INVISIBLE, bool isPossible = true)
    {
        this.id = id;
        this.context = context;
        this.phrase = phrase;
        this.actionName = actionName;
        this.visibility = visibility;
        this.isUnknown = visibility == Visibility.HIDDEN || visibility == Visibility.INVISIBLE;
    }

        public CommandAction(int id, string context, string phrase)
    {
        this.id = id;
        this.context = context;
        this.phrase = phrase;
        this.actionName = INSPECT_ACTION_NAME;
        this.visibility = Visibility.VISIBLE;
    }


    //equals


    public static Dictionary<string, EnvObjects> tagToEnvObj = new Dictionary<string, EnvObjects>()
    {
        {"door", EnvObjects.DOOR },
        {"wall", EnvObjects.WALLS },
        {"buckets", EnvObjects.BUCKETS },
        {"chair", EnvObjects.CHAIR },
        {"table", EnvObjects.TABLE },
        {"bed", EnvObjects.BED },
        {"toilet", EnvObjects.TOILET },
        {"hanged_clothes", EnvObjects.HANGED_CLOTHES },
        {"light", EnvObjects.LIGHT },
        {"trash", EnvObjects.TRASH },
        {"metal_box", EnvObjects.METAL_BOX },
        {"metal_door", EnvObjects.METAL_DOOR}
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

