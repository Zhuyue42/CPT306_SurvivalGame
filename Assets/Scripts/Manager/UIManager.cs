using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private Canvas mCanvas;
    private RectTransform HeadUI;
    private Image mPlayer_HP;
    private Text mPlayer_HP_Text;
    private Image mPlayer_MP;
    private Text mPlayer_MP_Text;
    private Image RuningState;
    private Image GameMenu_UI;
    private Image MainMenu_UI;
    private Image Setting_UI;
    private Toggle Music_Toggle;
    private Button Play_Btn;
    private Button Load_Btn;
    private Button Guide_Btn;
    private Button About_Btn;
    private Button Save_Button;
    private Button Resume_Button;
    private Button Back_Button;
    private Button Quit_Button;
    private Image IntegralUI;
    private Text Integral_Text;
    private Image GameOverUI;
    private Text GameOver_Text;
    private RectTransform MonsterUI;
    private Text MonsterCount_Text;
    private Image Tip_UI;
    private Text Tip_Text;
    private Button OverBack_Button;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        mCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        HeadUI = mCanvas.transform.Find("HeadUI").GetComponent<RectTransform>();
        mPlayer_HP = mCanvas.transform.Find("HeadUI/Player/HP").GetComponent<Image>();
        mPlayer_HP_Text = mCanvas.transform.Find("HeadUI/Player/HP/HPValue_Txet").GetComponent<Text>();

        mPlayer_MP = mCanvas.transform.Find("HeadUI/Player/MP").GetComponent<Image>();
        mPlayer_MP_Text = mCanvas.transform.Find("HeadUI/Player/MP/MPValue_Txet").GetComponent<Text>();

        RuningState = mCanvas.transform.Find("HeadUI/PlaerState/RuningState").GetComponent<Image>();

        Tip_UI = mCanvas.transform.Find("Tip_UI").GetComponent<Image>();
        Tip_Text = mCanvas.transform.Find("Tip_UI/Text").GetComponent<Text>();

        Setting_UI = mCanvas.transform.Find("Setting_UI").GetComponent<Image>();
        Music_Toggle = mCanvas.transform.Find("Setting_UI/Group/Toggle").GetComponent<Toggle>();

        MainMenu_UI = mCanvas.transform.Find("MainMenu_UI").GetComponent<Image>();
        Play_Btn = mCanvas.transform.Find("MainMenu_UI/Play_Btn").GetComponent<Button>();
        Load_Btn = mCanvas.transform.Find("MainMenu_UI/Load_Btn").GetComponent<Button>();
        Guide_Btn = mCanvas.transform.Find("MainMenu_UI/Guide_Btn").GetComponent<Button>();
        About_Btn = mCanvas.transform.Find("MainMenu_UI/About_Btn").GetComponent<Button>();
        Quit_Button = mCanvas.transform.Find("MainMenu_UI/Quit_Button").GetComponent<Button>();

        GameMenu_UI = mCanvas.transform.Find("GameMenu_UI").GetComponent<Image>();
        Save_Button = mCanvas.transform.Find("GameMenu_UI/Save_Button").GetComponent<Button>();
        Resume_Button = mCanvas.transform.Find("GameMenu_UI/Resume_Button").GetComponent<Button>();
        Back_Button = mCanvas.transform.Find("GameMenu_UI/Back_Button").GetComponent<Button>();



        IntegralUI = mCanvas.transform.Find("IntegralUI").GetComponent<Image>();
        Integral_Text = mCanvas.transform.Find("IntegralUI/Integral_Text").GetComponent<Text>();

        GameOverUI = mCanvas.transform.Find("GameOver").GetComponent<Image>();
        GameOver_Text = mCanvas.transform.Find("GameOver/Text").GetComponent<Text>();
        OverBack_Button = mCanvas.transform.Find("GameOver/Back_Button").GetComponent<Button>();

        MonsterUI = mCanvas.transform.Find("MonsterUI").GetComponent<RectTransform>();
        MonsterCount_Text = mCanvas.transform.Find("MonsterUI/Image/MonsterCount_Text").GetComponent<Text>();

        Save_Button.onClick.AddListener(Save_Event);
        Resume_Button.onClick.AddListener(Resume_Event);
        Back_Button.onClick.AddListener(Back_Event);
        OverBack_Button.onClick.AddListener(Back_Event);
        Quit_Button.onClick.AddListener(Quit_Event);

        Play_Btn.onClick.AddListener(Play_Event);
        Load_Btn.onClick.AddListener(Load_Event);
        Guide_Btn.onClick.AddListener(Guide_Event);
        About_Btn.onClick.AddListener(About_Event);

        Music_Toggle.onValueChanged.AddListener(Music_TogEvent);
    }
    // Update is called once per frame
    void Update()
    {

    }

    #region µã»÷ÊÂ¼þ
    private void Play_Event()
    {
        GameManager.Instance.PlayGame();
        MainMenu_UI.gameObject.SetActive(false);
        IntegralUI.gameObject.SetActive(true);
        HeadUI.gameObject.SetActive(true);
        MonsterUI.gameObject.SetActive(true);
    }
    private void Music_TogEvent(bool isOn)
    {
        AudioManager.Instance.Mute(!isOn);
    }
    private void About_Event()
    {
        GameManager.Instance.AboutGame();
    }
    private void Guide_Event()
    {
        GameManager.Instance.GuideGame();
    }
    private void Load_Event()
    {
        if (GameManager.Instance.LoadGame())
        {
            MainMenu_UI.gameObject.SetActive(false);
            IntegralUI.gameObject.SetActive(true);
            HeadUI.gameObject.SetActive(true);
            MonsterUI.gameObject.SetActive(true);
        }
    }
    private void Save_Event()
    {
        GameManager.Instance.SaveGame();
        SaveFileSucceed();
    }
    private void Back_Event()
    {
        MainMenu_UI.gameObject.SetActive(true);
        GameMenu_UI.gameObject.SetActive(false);
        IntegralUI.gameObject.SetActive(false);
        HeadUI.gameObject.SetActive(false);
        MonsterUI.gameObject.SetActive(false);
        GameManager.Instance.Back_MainGame();
    }
    private void Quit_Event()
    {
        Application.Quit();
    }
    private void Resume_Event()
    {
        GameMenu_UI.gameObject.SetActive(false);
        GameManager.Instance.Resume();
    }
    #endregion

    /// <summary>
    /// Uodate mark
    /// </summary>
    /// <param name="v"></param>
    public void GameIntegral(int v)
    {
        Integral_Text.text = v.ToString();
    }
    /// <summary>
    /// Update MP
    /// </summary>
    /// <param name="cuMP"></param>
    /// <param name="maxMP"></param>
    public void UpdateMP(float cuMP, float maxMP)
    {
        mPlayer_MP.fillAmount = cuMP / maxMP;
        mPlayer_MP_Text.text = $"{cuMP}/{maxMP}";
    }
    /// <summary>
    /// Update hp
    /// </summary>
    /// <param name="cuHP"></param>
    /// <param name="maxHP"></param>
    public void UpdateHP(float cuHP, float maxHP)
    {
        mPlayer_HP.fillAmount = cuHP / maxHP;
        mPlayer_HP_Text.text = $"{cuHP}/{maxHP}";
    }
    /// <summary>
    /// Player running status
    /// </summary>
    /// <param name="isShow"></param>
    public void PlayerRuningState(bool isShow)
    {
        RuningState.gameObject.SetActive(isShow);
    }
    /// <summary>
    /// Game over
    /// </summary>
    /// <param name="Integral"></param>
    public void GameOver(int Integral)
    {
        GameOverUI.gameObject.SetActive(true);
        if (Integral >= 50)
        {
            GameOver_Text.text = String.Format(App_Cont.GameOver_Text1, Integral);
        }
        if (Integral < 50)
        {
            GameOver_Text.text = String.Format(App_Cont.GameOver_Text2, Integral); 
        }
    }
    /// <summary>
    /// Update monster count UI
    /// </summary>
    /// <param name="MonsterCount"></param>
    public void UpdateMonsterCount(int MonsterCount)
    {
        MonsterCount_Text.text = MonsterCount.ToString();
    }
    /// <summary>
    /// No document file
    /// </summary>
    public void NotLoadFile()
    {
        Tip_UI.gameObject.SetActive(true);
        Tip_Text.text = App_Cont.NotLoadFile;
    }
    /// <summary>
    /// Save document successfully
    /// </summary>
    public void SaveFileSucceed()
    {
        Tip_UI.gameObject.SetActive(true);
        Tip_Text.text = App_Cont.SaveFileSucceed;
    }
}