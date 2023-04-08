using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PrefabType
{
    MonsterPrefab,
    PrizeItemPrefab,
    BaseItemPrefab,
    PlayerPrefab,
    VirusSpherePrefab,
    PlayerCameraPrefab,
}

public class ResManager : MonoBehaviour
{
    public static ResManager Instance { get; private set; }

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
        mGameData = ResManager.Instance.LoadData<GameData>("GameData");
        mMonsterPrefab = ResManager.Instance.LoadModel("MonsterPrefab");
        mPrizeItemPrefab = ResManager.Instance.LoadModel("PrizeItemPrefab");
        mBaseItemPrefab = ResManager.Instance.LoadModel("BaseItemPrefab");
        mPlayerPrefab = ResManager.Instance.LoadModel("PlayerPrefab");
        mPlayerCameraPrefab = ResManager.Instance.LoadModel("PlayerCameraPrefab");
        mVirusSpherePrefab = ResManager.Instance.LoadModel("VirusSpherePrefab");
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    public GameObject GetPrefab(PrefabType prefabType)
    {
        switch (prefabType)
        {
            case PrefabType.MonsterPrefab:
                return mMonsterPrefab;
            case PrefabType.PrizeItemPrefab:
                return mPrizeItemPrefab;
            case PrefabType.BaseItemPrefab:
                return mBaseItemPrefab;
            case PrefabType.PlayerPrefab:
                return mPlayerPrefab;
            case PrefabType.VirusSpherePrefab:
                return mVirusSpherePrefab;
            case PrefabType.PlayerCameraPrefab:
                return mPlayerCameraPrefab;
            default:
                return null;
        }
    }
    public GameData GetData()
    {
        return mGameData;
    }
    // Update is called once per frame
    void Update()
    {

    }

    public GameObject LoadModel(string AssetName)
    {
        return Resources.Load<GameObject>("Model/" + AssetName);
    }
    public T LoadData<T>(string AssetName) where T : ScriptableObject
    {
        return Resources.Load<T>("Data/" + AssetName);
    }
}
