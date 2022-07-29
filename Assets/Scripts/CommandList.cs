using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommandList
{
    /* phrases: 
     Mess with the closet, 1
     Mess with the door bar, 1
    overhear the green guards, 2
    overhear the red guards, 2
    eleminate the green guards, 3
    eliminate the red guards, 3
    enter the code into the terminal, 4
    analyse door paths, 5
    analyse hidden paths, 6
    turn off the lights, 7
    */

    //array of tuples

    public static List<string> phrases = new List<string>

    {
       "mess with the closet",
       "mess with the door bar",
       "overhear the green guards",
       "overhear the red guards",
       "eliminate the green guards",
       "eliminate the red guards",
       "enter the code into the terminal",
       "analyse door paths",
       "analyse hidden paths",
       "turn off the lights"
        //new Dictionary<int, string>(1, "Mess with the door bar"),
        //new Dictionary<int, string>(2, "overhear the green guards"),
        //new Dictionary<int, string>(2, "overhear the red guards"),
        //new Dictionary<int, string>(3, "eleminate the green guards"),
        //new Dictionary<int, string>(3, "eleminate the red guards"),
        //new Dictionary<int, string>(4, "enter the code into the terminal"),
        //new Dictionary<int, string>(5, "analyse door paths"),
        //new Dictionary<int, string>(6, "analyse hidden paths"),
        //new Dictionary<int, string>(7, "turn off the lights")
    };

    public static List<VoiceInteraction> commandsArray = new List<VoiceInteraction>
    {
        new VoiceInteraction("Mess with the closet", 1, 1),
        new VoiceInteraction("Mess with the door bar", 1, 2),
        new VoiceInteraction("overhear the green guards", 2, 3),
        new VoiceInteraction("overhear the red guards", 2, 4),
        new VoiceInteraction("eliminate the green guards", 3, 5),
        new VoiceInteraction("eliminate the red guards", 3, 6),
        new VoiceInteraction("enter the code into the terminal", 4, 7),
        new VoiceInteraction("analyse door paths", 5, 8),
        new VoiceInteraction("analyse hidden paths", 6, 9),
        new VoiceInteraction("turn off the lights", 7, 10)
    };



}
