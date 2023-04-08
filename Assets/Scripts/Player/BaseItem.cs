using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : ItemBase
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.mPlayer != null)
        {
          if (other.gameObject.name == GameManager.Instance.mPlayer.name)
            {
                //AddIntegral();
                AddMoveSpeedState();
                GameManager.Instance.DesBaseItem(ID);
            }
        }
        if (other.gameObject.tag == "Monster")
        {
            CopyMonster(other.transform);
            GameManager.Instance.DesBaseItem(ID);
            //Destroy(gameObject);
        }
    }
    public void AddMoveSpeedState()
    {
        GameManager.Instance.mPlayer.AddMoveState(10);
    }

    public void CopyMonster(Transform tran)
    {
        GameManager.Instance.CopyNpc(tran);
    }

}
