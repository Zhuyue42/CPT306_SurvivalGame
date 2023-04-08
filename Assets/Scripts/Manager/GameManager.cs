using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public Player mPlayer;
    public GameObject mVirusSpher;

    private float tempGameTime;
    private float tempIntegralTime;
    private bool mGamePause = false;
    private string mGameDataPath;

    public List<GameObject> mListMonsters = new List<GameObject>();
    public List<GameObject> mListPrizes = new List<GameObject>();
    public List<GameObject> mListBaseItems = new List<GameObject>();
    public GameObject SceneCamera;


    private GameData mGameData;
    private GameObject mPlayerPrefab;
    private GameObject mMonsterPrefab;
    private GameObject mPrizeItemPrefab;
    private GameObject mBaseItemPrefab;
    private GameObject mPlayerCameraPrefab;
    private GameObject mVirusSpherePrefab;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        mGameDataPath = Application.persistentDataPath + App_Cont.GameDataFilePath;
        mGameData = ResManager.Instance.GetData();
        mPlayerPrefab = ResManager.Instance.GetPrefab(PrefabType.PlayerPrefab);
        mMonsterPrefab = ResManager.Instance.GetPrefab(PrefabType.MonsterPrefab);
        mPrizeItemPrefab = ResManager.Instance.GetPrefab(PrefabType.PrizeItemPrefab);
        mBaseItemPrefab = ResManager.Instance.GetPrefab(PrefabType.BaseItemPrefab);
        mPlayerCameraPrefab = ResManager.Instance.GetPrefab(PrefabType.PlayerCameraPrefab);
        mVirusSpherePrefab = ResManager.Instance.GetPrefab(PrefabType.VirusSpherePrefab);
    }
    void Update()
    {
        if (GameIsPause())
        {
            UpdateMP();
            UpdateHP();
            ShowRuningState();
            UpdateGameTime(true);
            GameTimeIn();
            UpdateMonsterCount();
        }
    }

    /// <summary>
    /// Game initialisation
    /// </summary>
    public void GameInit()
    {
        SceneCamera.gameObject.SetActive(false);
        GameObject player = Instantiate(mPlayerPrefab);
        mPlayer = player.GetComponent<Player>();
        mPlayer.InitPlayer();
        GameObject virusSpher = Instantiate(mVirusSpherePrefab);
        int Vx = Random.Range(8, 40);
        int Vy = Random.Range(-10, -15);
        int Vz = Random.Range(8, 40);
        virusSpher.transform.position = new Vector3(Vx, Vy, Vz);
        mVirusSpher = virusSpher;
        GameObject camera = Instantiate(mPlayerCameraPrefab);
        camera.GetComponent<CameriaTrack>().target = player.transform;
        player.name = "Player";
        int Px = Random.Range(10, 45);
        int Pz = Random.Range(10, 45);
        player.transform.position = new Vector3(Px, 2f, Pz);

        for (int i = 0; i < mGameData.Monster_Count; i++)
        {
            GameObject go = Instantiate(mMonsterPrefab);
            go.name = go.name + i;
            int x = Random.Range(2, 50);
            int z = Random.Range(2, 50);
            go.transform.position = new Vector3(x, 1f, z);
            mListMonsters.Add(go);
        }
        for (int i = 0; i < mGameData.PrizeItem_Count; i++)
        {
            GameObject go = Instantiate(mPrizeItemPrefab);
            ItemBase itemBase = go.transform.GetChild(0).GetComponent<ItemBase>();
            itemBase.ID = i;
            go.name = go.name + i;
            int x = Random.Range(5, 45);
            int z = Random.Range(5, 45);
            go.transform.position = new Vector3(x, 2f, z);
            mListPrizes.Add(go);
        }
        for (int i = 0; i < mGameData.BaseItem_Count; i++)
        {
            GameObject go = Instantiate(mBaseItemPrefab);
            ItemBase itemBase = go.transform.GetChild(0).transform.GetComponent<ItemBase>();
            itemBase.ID = i;
            go.name = go.name + i;
            int x = Random.Range(5, 45);
            int z = Random.Range(5, 45);
            go.transform.position = new Vector3(x, 2f, z);
            mListBaseItems.Add(go);
        }
        mGamePause = true;
    }

    /// <summary>
    /// Reset Reward
    /// </summary>
    /// <param name="prize"></param>
    public void DesPrize(int prize)
    {
        GameObject go = mListPrizes[prize];
        int x = Random.Range(2, 50);
        int z = Random.Range(2, 50);
        go.transform.position = new Vector3(x, 0.2f, z);
    }
    /// <summary>
    /// Reset Base
    /// </summary>
    /// <param name="baseItem"></param>
    public void DesBaseItem(int baseItem)
    {
        GameObject go = mListBaseItems[baseItem];
        int x = Random.Range(2, 50);
        int z = Random.Range(2, 50);
        go.transform.position = new Vector3(x, 0.2f, z);
    }
    /// <summary>
    /// Update monster count UI
    /// </summary>
    public void UpdateMonsterCount()
    {
        if (mListMonsters != null)
        {
            UIManager.Instance.UpdateMonsterCount(mListMonsters.Count);
        }
    }
    /// <summary>
    /// Update the game time UI
    /// </summary>
    /// <param name="isUpdate"></param>
    public void UpdateGameTime(bool isUpdate = false)
    {
        if (isUpdate)
        {
            if (tempGameTime >= mGameData.Survival_Time)
            {
                mPlayer.UpdateIntegral(mGameData.BaseIntegral);
                UIManager.Instance.GameIntegral(mPlayer.GetIntegral());
                tempGameTime = 0;
            }
            tempGameTime += Time.deltaTime;
        }
        else
        {
            UIManager.Instance.GameIntegral(mPlayer.GetIntegral());
        }
    }
    /// <summary>
    /// Cloning monsters
    /// </summary>
    /// <param name="tran"></param>
    public void CopyNpc(Transform tran)
    {
        GameObject go = Instantiate(mMonsterPrefab);
        go.transform.position = tran.position;
        mListMonsters.Add(go);
    }
    /// <summary>
    /// Mark
    /// Time multiplier
    /// </summary>
    public void GameTimeIn()
    {
        if (tempIntegralTime >= 60)
        {
            mGameData.BaseIntegral = mGameData.BaseIntegral * 2;
            tempIntegralTime = 0;
        }
        tempIntegralTime += Time.deltaTime;
    }
    /// <summary>
    /// Update MP
    /// </summary>
    public void UpdateMP()
    {
        UIManager.Instance.UpdateMP(mPlayer.GetPlayerData().CurrentMP, mPlayer.GetPlayerData().MaxMP);
    }
    /// <summary>
    /// Update HP
    /// </summary>
    public void UpdateHP()
    {
        UIManager.Instance.UpdateHP(mPlayer.GetPlayerData().CurrentHP, mPlayer.GetPlayerData().MaxHP);
    }
    /// <summary>
    /// Show Running Status UI
    /// </summary>
    public void ShowRuningState()
    {
        if (mPlayer.mPlayerState == PlayerMoveState.Runing)
        {
            UIManager.Instance.PlayerRuningState(true);
        }
        else
        {
            UIManager.Instance.PlayerRuningState(false);
        }
    }
    /// <summary>
    ///Game Over
    /// </summary>
    public void GameOver()
    {
        GamePause();
        UIManager.Instance.GameOver(mPlayer.GetIntegral());
    }
    /// <summary>
    /// Game Pause
    /// </summary>
    public void GamePause()
    {
        Time.timeScale = 0;
        mGamePause = false;
    }
    /// <summary>
    /// Game continue
    /// </summary>
    public void Resume()
    {
        Time.timeScale = 1;
        mGamePause = true;
    }
    /// <summary>
    /// Crative Save files
    /// </summary>
    /// <returns></returns>
    private string CreateSave()
    {
        SaveData saveData = new SaveData();
        saveData.mPlayerData = mPlayer.GetPlayerData();
        saveData.Pos_X = float.Parse(mPlayer.transform.position.x.ToString("F2"));
        saveData.Pos_Y = float.Parse(mPlayer.transform.position.y.ToString("F2"));
        saveData.Pos_Z = float.Parse(mPlayer.transform.position.z.ToString("F2"));
        saveData.Rot_X = float.Parse(mPlayer.transform.eulerAngles.x.ToString("F2"));
        saveData.Rot_Y = float.Parse(mPlayer.transform.eulerAngles.y.ToString("F2"));
        saveData.Rot_Z = float.Parse(mPlayer.transform.eulerAngles.z.ToString("F2"));
        saveData.MonsterLists = new List<OBJData>();
        saveData.BaseItemLists = new List<OBJData>();
        saveData.PrizeLists = new List<OBJData>();
        saveData.VirusSpher = new OBJData();
        saveData.VirusSpher.Pos_X = float.Parse(mVirusSpher.transform.position.x.ToString("F2"));
        saveData.VirusSpher.Pos_Y = float.Parse(mVirusSpher.transform.position.y.ToString("F2"));
        saveData.VirusSpher.Pos_Z = float.Parse(mVirusSpher.transform.position.z.ToString("F2"));
        saveData.VirusSpher.Rot_X = float.Parse(mVirusSpher.transform.eulerAngles.x.ToString("F2"));
        saveData.VirusSpher.Rot_Y = float.Parse(mVirusSpher.transform.eulerAngles.y.ToString("F2"));
        saveData.VirusSpher.Rot_Z = float.Parse(mVirusSpher.transform.eulerAngles.z.ToString("F2"));
        for (int i = 0; i < mListMonsters.Count; i++)
        {
            OBJData oBJ = new OBJData();
            oBJ.Pos_X = float.Parse(mListMonsters[i].transform.position.x.ToString("F2"));
            oBJ.Pos_Y = float.Parse(mListMonsters[i].transform.position.y.ToString("F2"));
            oBJ.Pos_Z = float.Parse(mListMonsters[i].transform.position.z.ToString("F2"));
            oBJ.Rot_X = float.Parse(mListMonsters[i].transform.eulerAngles.x.ToString("F2"));
            oBJ.Rot_Y = float.Parse(mListMonsters[i].transform.eulerAngles.y.ToString("F2"));
            oBJ.Rot_Z = float.Parse(mListMonsters[i].transform.eulerAngles.z.ToString("F2"));
            oBJ.ID = i;
            saveData.MonsterLists.Add(oBJ);
        }
        for (int i = 0; i < mListBaseItems.Count; i++)
        {
            OBJData oBJ = new OBJData();
            oBJ.Pos_X = float.Parse(mListBaseItems[i].transform.position.x.ToString("F2"));
            oBJ.Pos_Y = float.Parse(mListBaseItems[i].transform.position.y.ToString("F2"));
            oBJ.Pos_Z = float.Parse(mListBaseItems[i].transform.position.z.ToString("F2"));
            oBJ.Rot_X = float.Parse(mListBaseItems[i].transform.eulerAngles.x.ToString("F2"));
            oBJ.Rot_Y = float.Parse(mListBaseItems[i].transform.eulerAngles.y.ToString("F2"));
            oBJ.Rot_Z = float.Parse(mListBaseItems[i].transform.eulerAngles.z.ToString("F2"));
            oBJ.ID = mListBaseItems[i].transform.GetChild(0).GetComponent<ItemBase>().ID;
            saveData.BaseItemLists.Add(oBJ);
        }
        for (int i = 0; i < mListPrizes.Count; i++)
        {
            OBJData oBJ = new OBJData();
            oBJ.Pos_X = float.Parse(mListPrizes[i].transform.position.x.ToString("F2"));
            oBJ.Pos_Y = float.Parse(mListPrizes[i].transform.position.y.ToString("F2"));
            oBJ.Pos_Z = float.Parse(mListPrizes[i].transform.position.z.ToString("F2"));
            oBJ.Rot_X = float.Parse(mListPrizes[i].transform.eulerAngles.x.ToString("F2"));
            oBJ.Rot_Y = float.Parse(mListPrizes[i].transform.eulerAngles.y.ToString("F2"));
            oBJ.Rot_Z = float.Parse(mListPrizes[i].transform.eulerAngles.z.ToString("F2"));
            oBJ.ID = mListPrizes[i].transform.GetChild(0).GetComponent<ItemBase>().ID;
            saveData.PrizeLists.Add(oBJ);
        }
        return JsonUtility.ToJson(saveData);
    }
    /// <summary>
    /// Save Game
    /// </summary>
    public void SaveGame()
    {
        string save = CreateSave();

        using (StreamWriter sw = new StreamWriter(mGameDataPath))
        {
            sw.Write(save);
            //Write the JSON string to the stream parameter
            sw.Close();
        }
        Debug.Log(mGameDataPath);
    }
    /// <summary>
    /// Back to home page 
    /// Delete scene objects
    /// </summary>
    public void Back_MainGame()
    {
        for (int i = 0; i < mListMonsters.Count; i++)
        {
            DestroyImmediate(mListMonsters[i].gameObject);
        }
        mListMonsters.Clear();
        for (int i = 0; i < mListPrizes.Count; i++)
        {
            DestroyImmediate(mListPrizes[i].gameObject);
        }
        mListPrizes.Clear();
        for (int i = 0; i < mListBaseItems.Count; i++)
        {
            DestroyImmediate(mListBaseItems[i].gameObject);
        }
        tempGameTime = 0;
        mListBaseItems.Clear();
        Time.timeScale = 1;
        mGamePause = false;
        Destroy(mPlayer.gameObject);
        Destroy(mVirusSpher.gameObject);
        Destroy(Camera.main.gameObject);
        SceneCamera.gameObject.SetActive(true);
    }
    /// <summary>
    /// Read document files
    /// </summary>
    /// <returns></returns>
    private SaveData LoadByDeserialization()
    {
        //Check whether the file is created
        if (File.Exists(mGameDataPath))
        {
            Debug.Log(mGameDataPath);
            using (StreamReader sr = new StreamReader(mGameDataPath))
            {
                //Reads a string from the stream
                string JsonString = sr.ReadToEnd();
                //Deserialize and store the data to Save
                //(cast to the Save class because the return variable is of the wrong type
                sr.Close();
                return JsonUtility.FromJson<SaveData>(JsonString);
            }
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// Begin Game
    /// </summary>
    public void PlayGame()
    {
        GameInit();
    }
  
    public void GuideGame()
    {

    }
    /// <summary>
    /// Load archive game
    /// </summary>
    /// <returns></returns>
    public bool LoadGame()
    {
        SaveData saveData = LoadByDeserialization();
        if (saveData != null)
        {
            SceneCamera.gameObject.SetActive(false);
            GameObject player = Instantiate(mPlayerPrefab);
            mPlayer = player.gameObject.GetComponent<Player>();
            mPlayer.InitPlayer(saveData.mPlayerData);
            GameObject camera = Instantiate(mPlayerCameraPrefab);
            camera.GetComponent<CameriaTrack>().target = player.transform;
            player.transform.position = new Vector3(saveData.Pos_X, saveData.Pos_Y, saveData.Pos_Z);
            player.transform.eulerAngles = new Vector3(saveData.Rot_X, saveData.Rot_Y, saveData.Rot_Z);
            GameObject virusSpher = Instantiate(mVirusSpherePrefab);
            virusSpher.transform.position = new Vector3(saveData.VirusSpher.Pos_X, saveData.VirusSpher.Pos_Y, saveData.VirusSpher.Pos_Z);
            virusSpher.transform.eulerAngles = new Vector3(saveData.VirusSpher.Rot_X, saveData.VirusSpher.Rot_Y, saveData.VirusSpher.Rot_Z);
            mVirusSpher = virusSpher;
            for (int i = 0; i < saveData.MonsterLists.Count; i++)
            {
                GameObject go = Instantiate(mMonsterPrefab);
                go.name = mMonsterPrefab.name + saveData.MonsterLists[i].ID;
                go.transform.position = new Vector3(saveData.MonsterLists[i].Pos_X, saveData.MonsterLists[i].Pos_Y, saveData.MonsterLists[i].Pos_Z);
                go.transform.eulerAngles = new Vector3(saveData.MonsterLists[i].Rot_X, saveData.MonsterLists[i].Rot_Y, saveData.MonsterLists[i].Rot_Z);
                mListMonsters.Add(go);
            }
            for (int i = 0; i < saveData.PrizeLists.Count; i++)
            {
                GameObject go = Instantiate(mPrizeItemPrefab);
                go.transform.GetChild(0).GetComponent<ItemBase>().ID = saveData.PrizeLists[i].ID;
                go.name = mPrizeItemPrefab.name + saveData.PrizeLists[i].ID;
                go.transform.position = new Vector3(saveData.PrizeLists[i].Pos_X, saveData.PrizeLists[i].Pos_Y, saveData.PrizeLists[i].Pos_Z);
                go.transform.eulerAngles = new Vector3(saveData.PrizeLists[i].Rot_X, saveData.PrizeLists[i].Rot_Y, saveData.PrizeLists[i].Rot_Z);
                mListPrizes.Add(go);
            }
            for (int i = 0; i < saveData.BaseItemLists.Count; i++)
            {
                GameObject go = Instantiate(mBaseItemPrefab);
                go.transform.GetChild(0).GetComponent<ItemBase>().ID = saveData.BaseItemLists[i].ID;
                go.name = mBaseItemPrefab.name + saveData.BaseItemLists[i].ID;
                go.transform.position = new Vector3(saveData.BaseItemLists[i].Pos_X, saveData.BaseItemLists[i].Pos_Y, saveData.BaseItemLists[i].Pos_Z);
                go.transform.eulerAngles = new Vector3(saveData.BaseItemLists[i].Rot_X, saveData.BaseItemLists[i].Rot_Y, saveData.BaseItemLists[i].Rot_Z);
                mListBaseItems.Add(go);
            }
            mGamePause = true;
            return true;
        }
        else
        {
            UIManager.Instance.NotLoadFile();
            return false;
        }
    }

    public void AboutGame()
    {

    }
    /// <summary>
    /// Return to game status
    /// </summary>
    /// <returns></returns>
    public bool GameIsPause()
    {
        return mGamePause;
    }

}