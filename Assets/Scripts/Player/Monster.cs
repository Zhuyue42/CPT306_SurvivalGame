using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : NpcBase
{
    private NavMeshAgent m_agent;

    public float AttackTime = 2f;

    void Start()
    {
        m_agent = this.GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        tempAttackTime += Time.deltaTime;
        m_agent.destination = GameManager.Instance.mPlayer.transform.position - new Vector3(0.5f, 0.5f, 0.5f);
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
        GameManager.Instance.mPlayer.ChangeHP(-20);
    }
}
