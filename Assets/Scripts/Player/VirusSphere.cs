using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusSphere : MonoBehaviour
{
    public float AttackTime = 2f;

    void Start()
    {
    }

    void Update()
    {
        tempAttackTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == GameManager.Instance.mPlayer.name)
        {
            if (tempAttackTime >= AttackTime)
            {
                Attack();
                tempAttackTime = 0;
            }
        }
    }

    private float tempAttackTime;
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == GameManager.Instance.mPlayer.name)
        {
            if (tempAttackTime >= AttackTime)
            {
                Attack();
                tempAttackTime = 0;
            }
        }
    }

    public void Attack()
    {
        GameManager.Instance.mPlayer.ChangeHP(-1);
    }
}
