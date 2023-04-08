using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcBase : MonoBehaviour
{
    public int ID;
    public virtual float GetHp()
    {
        return 0;
    }



}
