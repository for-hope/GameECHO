using System;

public enum Availability
{
    UNLOCKED_FIRE,
    HAS_FIRE_AMMO,
    NOT_IN_ICE,

    UNLOCKED_WATER,
    HAS_WATER_AMMO,
    NOT_IN_FIRE,

    UNLOCKED_LIGHT,
    HAS_LIGHT_ENERGY,
    NOT_IN_SUN,

    UNLOCKED_DARK,
    HAS_DARK_ENERGY,
    NOT_IN_DARKNESS,

    UNLOCKED_RED,
    IS_NOT_RED,

    UNLOCKED_BLUE,
    IS_NOT_BLUE,

    CAN_TELEPORT,
    NOT_NEAR_PLAYER,

    RIGHT_GATE,

    LEFT_GATE,

    UNLOCKED_ULT,
    HAS_ULT_ENERGY,

    ENEMY_NEAR,

    RIGHT_ENEMY,

    LEFT_ENEMY,

    MULTIPLE_ENEMIES,

    CAN_MOVE,

    CAN_UNLOCK


}


public enum Likelyhood
{
    NEAR_WATER_ENEMY,
    NEAR_WATERFALL_GATE,
    JUST_LEARNT_FIRE,

    NEAR_FIRE_ENEMY,
    NEAR_FIREWALL_GATE,
    JUST_LEARNT_WATER,

    IN_DARK_PLACE,
    JUST_LEART_LIGHT,

    IN_LIT_AREA,
    JUST_LEARNT_DARK,

    VS_BLUE,

    VS_RED,

    IS_DYING,

    IS_STUCK,

    IS_BOSS,
    IS_MANY_ENEMIES,
    IS_ULT_JUST_CHARGED,

    IS_LEFT_DANGEROUS,

    IS_RIGHT_DANGEROUS,

    IS_EASY_ENEMIES,

    IS_LEFT_PATH_OPEN,
    IS_PLAYER_LOOKING_LEFT,

    IS_RIGHT_PATH_OPEN,
    IS_PLAYER_LOOKING_RIGHT,

    IS_GATE_LOCKED,
    IS_LOOKING_GATE,

}

public class VoiceCommand
{
   

    public String phrase;
    public int id;
    public Availability[] availabilityList;
    public Likelyhood[] likelyhoodList;

    //constructor
    public VoiceCommand(String phrase, Availability[] availabilityList, Likelyhood[] likelyhoodList)
    {
        this.phrase = phrase;
        this.availabilityList = availabilityList;
        this.likelyhoodList = likelyhoodList;
    }
}
/*
  
    use a fire element
    use a water element
    let there be light
    let there be dark
    transform to a red color
    transform to a blue color
    teleport back to me
    destroy the right gate
    destroy the left gate
    use the ultimate attack
    attack the left enemy
    attack the right enemy
    attack all the enemies
    comeback
    go left
    go right
    unlock the gate
 */
