using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PlayerMoveState
{
    None = 0,
    Idle,
    Run,
    Jump,
    Runing
}

public class Player : MonoBehaviour
{
    private GameObject self_Player;
    private PlayerData playerData;
    private PlayerController controller;
    public bool IsRun = true;
    public float tmepSRuningTime = 0;
    public float tmepERuningTime = 0;
    public PlayerMoveState mPlayerState = PlayerMoveState.None;
    private float tempMoveStateTime = 0;
    private bool tempMoveState;

    // Start is called before the first frame update
    void Start()
    {
        self_Player = GetComponent<GameObject>();
        controller = GetComponent<PlayerController>();
        controller.playerMoveStartRuning_Event += Controller_playerMoveLeftShift_Event;
        controller.playerMoveEndRuning_Event += Controller_playerMoveEndRuning_Event;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameIsPause())
        {
            if (playerData.CurrentHP <= 0)
            {
                Player_Die();
            }
            //if (playerData.CurrentMP >= 100)
            //{
            //    IsRun = true;
            //}
            //else
            //{
            //    IsRun = false;
            //}
            if (mPlayerState != PlayerMoveState.Runing)
            {
                if (tmepERuningTime >= 5)
                {
                    ChangeMP(20);
                    tmepERuningTime = 0;
                }
                tmepERuningTime += Time.deltaTime;
            }
            else
            {
                tmepERuningTime = 0;
            }
            if (tempMoveStateTime <= 0 && tempMoveState)
            {
                controller.m_moveSpeed = ResManager.Instance.GetData().PlayerBaseSpeed;
                tempMoveState = false;
            }
            if (tempMoveStateTime > 0 && tempMoveState)
            {
                controller.m_moveSpeed = ResManager.Instance.GetData().PlayerBaseSpeed * 2;
                tempMoveStateTime -= Time.deltaTime;
            }
        }
    }

    /// <summary>
    /// Player initialisation
    /// </summary>
    /// <param name="mPlayerData"></param>
    public void InitPlayer(PlayerData mPlayerData = null)
    {
        if (mPlayerData == null)
        {
            playerData = new PlayerData();
            playerData.Name = "";
            playerData.MaxHP = ResManager.Instance.GetData().playerData.MaxHP;
            playerData.MaxMP = ResManager.Instance.GetData().playerData.MaxMP;
            playerData.CurrentMP = ResManager.Instance.GetData().playerData.CurrentMP;
            playerData.CurrentHP = ResManager.Instance.GetData().playerData.CurrentHP;
            playerData.Die = ResManager.Instance.GetData().playerData.Die;
            playerData.Integral = ResManager.Instance.GetData().playerData.Integral;
        }
        else
        {
            playerData = mPlayerData;
        }
    }
    /// <summary>
    ///  Sprint
    /// </summary>
    private void Controller_playerMoveLeftShift_Event()
    {
        if (tmepSRuningTime >= 1)
        {
            ChangeMP(-10);
            tmepSRuningTime = 0;
        }
        tmepSRuningTime += Time.deltaTime;
    }
    /// <summary>
    /// Stop Sprint
    /// </summary>
    private void Controller_playerMoveEndRuning_Event()
    {
        IsRun = false;
        controller.IsShiftOver(IsRun);
    }
    /// <summary>
    /// Return Player data
    /// </summary>
    /// <returns></returns>
    public PlayerData GetPlayerData()
    {
        return playerData;
    }
    /// <summary>
    /// Return Mark
    /// </summary>
    /// <returns></returns>
    public int GetIntegral()
    {
        return playerData.Integral;
    }
    /// <summary>
    /// MP whether enough
    /// </summary>
    /// <returns></returns>
    public bool Runing()
    {
        return IsRun;
    }
    /// <summary>
    /// Add acceleration status
    /// </summary>
    /// <param name="time"></param>
    public void AddMoveState(float time)
    {
        tempMoveState = true;
        tempMoveStateTime += time;
    }
    /// <summary>
    /// Update individual scores
    /// </summary>
    /// <param name="value"></param>
    public void UpdateIntegral(int value)
    {
        playerData.Integral += value;
    }
    /// <summary>
    /// Check Hp
    /// </summary>
    /// <param name="hp"></param>
    public void ChangeHP(int hp)
    {
        playerData.CurrentHP += hp;
        if (playerData.CurrentHP >= playerData.MaxHP)
        {
            playerData.CurrentHP = playerData.MaxHP;
        }
        if (playerData.CurrentHP <= 0)
        {
            playerData.CurrentHP = 0;
        }
    }
    /// <summary>
    /// Check mp
    /// </summary>
    /// <param name="mp"></param>
    public void ChangeMP(int mp)
    {
        playerData.CurrentMP += mp;
        if (playerData.CurrentMP >= playerData.MaxMP)
        {
            playerData.CurrentMP = playerData.MaxMP;
        }
        if (playerData.CurrentMP <= 0)
        {
            playerData.CurrentMP = 0;
        }
        if (playerData.CurrentMP <= 0)
        {
            IsRun = false;
            controller.IsShiftOver(IsRun);
        }
        else if (playerData.CurrentMP >= 100)
        {
            IsRun = true;
        }
    }
    /// <summary>
    /// Die
    /// </summary>
    public void Player_Die()
    {
        playerData.Die = true;
        GameManager.Instance.GameOver();
        StartCoroutine(PlayerDie_Admin());

    }
  
    public IEnumerator PlayerDie_Admin()
    {
        float temp = 0;
        while (temp < 1)
        {
            transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(90, 0, 0), temp);
            temp += 0.02f;
            yield return 0;
        }
    }
}