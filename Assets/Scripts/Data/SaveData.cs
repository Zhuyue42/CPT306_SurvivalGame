using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData 
{
    public PlayerData mPlayerData;

    public float Pos_X;
    public float Pos_Y;
    public float Pos_Z;
    public float Rot_X;
    public float Rot_Y;
    public float Rot_Z;

    public List<OBJData> PrizeLists = new List<OBJData>();
    public List<OBJData> MonsterLists = new List<OBJData>();
    public List<OBJData> BaseItemLists = new List<OBJData>();
    public OBJData VirusSpher;


}

[System.Serializable]
public class OBJData
{
    public int ID;
    public float Pos_X;
    public float Pos_Y;
    public float Pos_Z;
    public float Rot_X;
    public float Rot_Y;
    public float Rot_Z;
}