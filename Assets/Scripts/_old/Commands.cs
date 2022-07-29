using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VoiceCommand;

public class Commands
{
    
    static string firePhrase = "use a fire element";
    static Availability[] fireAv = { Availability.UNLOCKED_FIRE, Availability.HAS_FIRE_AMMO, Availability.NOT_IN_ICE };
    static Likelyhood[] fireLikely = { Likelyhood.NEAR_WATER_ENEMY, Likelyhood.NEAR_WATERFALL_GATE, Likelyhood.JUST_LEARNT_FIRE };
    static VoiceCommand fire = new VoiceCommand(firePhrase, fireAv , fireLikely);
    static string waterPhrase = "use a water element";
    static Availability[] waterAv = { Availability.UNLOCKED_WATER, Availability.HAS_WATER_AMMO, Availability.NOT_IN_FIRE };
    static Likelyhood[] waterLikely = { Likelyhood.NEAR_WATER_ENEMY, Likelyhood.NEAR_FIREWALL_GATE, Likelyhood.JUST_LEARNT_WATER };
    static VoiceCommand water = new VoiceCommand(waterPhrase, waterAv, waterLikely);
    static string lightPhrase = "let there be light";
    static Availability[] lightAv = { Availability.UNLOCKED_LIGHT, Availability.HAS_LIGHT_ENERGY, Availability.NOT_IN_SUN };
    static Likelyhood[] lightLikely = { Likelyhood.IN_DARK_PLACE, Likelyhood.JUST_LEART_LIGHT };
    static VoiceCommand light = new VoiceCommand(lightPhrase, lightAv, lightLikely);
    static string darkPhrase = "let there be dark";
    static Availability[] darkAv = { Availability.UNLOCKED_DARK, Availability.HAS_DARK_ENERGY, Availability.NOT_IN_DARKNESS };
    static Likelyhood[] darkLikely = { Likelyhood.IN_LIT_AREA, Likelyhood.JUST_LEARNT_DARK };
    static VoiceCommand dark = new VoiceCommand(darkPhrase, darkAv, darkLikely);
    static string transformRedPhrase = "transform to red color";
    static Availability[] transformRedAv = { Availability.UNLOCKED_RED, Availability.IS_NOT_RED };
    static Likelyhood[] transformRedLikely = { Likelyhood.VS_BLUE };
    static VoiceCommand transformRed = new VoiceCommand(transformRedPhrase, transformRedAv, transformRedLikely);
    static string transformBluePhrase = "transform to blue color";
    static Availability[] transformBlueAv = { Availability.UNLOCKED_BLUE, Availability.IS_NOT_BLUE };
    static Likelyhood[] transformBlueLikely = { Likelyhood.VS_RED };
    static VoiceCommand transformBlue = new VoiceCommand(transformBluePhrase, transformBlueAv, transformBlueLikely);
    static string destroyRightGatePhrase = "destroy the right gate";
    static Availability[] destroyRightGateAv = { Availability.RIGHT_GATE};
    static Likelyhood[] destroyRightGateLikely = { Likelyhood.IS_STUCK, Likelyhood.IS_PLAYER_LOOKING_RIGHT };
    static VoiceCommand destroyRightGate = new VoiceCommand(destroyRightGatePhrase, destroyRightGateAv, destroyRightGateLikely);
    static string destroyLeftGatePhrase = "destroy the left gate";
    static Availability[] destroyLeftGateAv = { Availability.LEFT_GATE };
    static Likelyhood[] destroyLeftGateLikely = { Likelyhood.IS_STUCK, Likelyhood.IS_PLAYER_LOOKING_LEFT };
    static VoiceCommand destroyLeftGate = new VoiceCommand(destroyLeftGatePhrase, destroyLeftGateAv, destroyLeftGateLikely);
    static string teleportPhrase = "teleport back to me";
    static Availability[] teleportAv = { Availability.CAN_TELEPORT, Availability.NOT_NEAR_PLAYER };
    static Likelyhood[] teleportLikely = { Likelyhood.IS_DYING };
    static VoiceCommand teleport = new VoiceCommand(teleportPhrase, teleportAv, teleportLikely);

    static  VoiceCommand[] voiceCommands = { fire, water, light, dark, transformRed, transformBlue, destroyRightGate, destroyLeftGate, teleport };








}


