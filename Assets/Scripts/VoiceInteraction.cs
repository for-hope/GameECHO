using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceInteraction
{
    public string phrase;
    public int interactionId;
    public int id;

    public VoiceInteraction(string phrase, int interactionId ,int id)
    {
        this.phrase = phrase;
        this.id = id;
        this.interactionId = interactionId;
       
    }

}
