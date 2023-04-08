using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData")]

public class GameData : ScriptableObject
{
    [Header("Number of Monster")]
    public int Monster_Count = 1;
    [Header("Number of Prize")]
    public int PrizeItem_Count = 10;
    [Header("Number of Base")]
    public int BaseItem_Count = 20;
    [Header("Speed")]
    public float PlayerBaseSpeed = 6;
    [Header("Score Time")]
    public float Survival_Time = 10;
    [Header("Score every time")]
    public int BaseIntegral = 1;
    public PlayerData playerData;
}
