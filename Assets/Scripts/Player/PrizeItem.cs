using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeItem : ItemBase
{
    void Start()
    {
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == GameManager.Instance.mPlayer.name)
        {
            AddIntegral();
            GameManager.Instance.DesPrize(ID);
            //Destroy(transform.parent.gameObject);
        }
    }

    public void AddIntegral()
    {
        GameManager.Instance.mPlayer.UpdateIntegral(10);
        GameManager.Instance.UpdateGameTime();
    }

}
