using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public string Name;
    [Header("Current blood level")]
    public int CurrentHP =100;
    [Header("Maximum blood volume")]
    public int MaxHP = 100;
    [Header("Current Endurance")]
    public int CurrentMP = 100;
    [Header("Maximum Endurance")]
    public int MaxMP = 100;
    [Header("Base Score")]
    public int Integral = 0;
    [Header("Is dead")]
    public bool Die =false;
}
