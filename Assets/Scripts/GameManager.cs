using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text SecondsLeftText;
    [SerializeField] TMP_Text ClicksDoneText;
    [SerializeField] TMP_Text HiScoreText;
    [SerializeField] TMP_Text EndGameScoreText;
    [SerializeField] GameObject TextToDisableOnStart;
    [SerializeField] GameObject EndGameMenu;
    [SerializeField] GameObject RewardAdMenu;
#if UNITY_ANDROID
    private DisplayAdds _DisplayAdds;
#endif
    private MenuManager _MenuManager;
    private string HighScorePlayerRefTag = "HighScore";
    private bool Playing;
    private int ClicksDone;
    private int Timer;
    private int DefaultClickTime = 10;
    private int ExtraTime = 12;
    // Start is called before the first frame update
    private void OnEnable()
    {
#if UNITY_ANDROID
        _DisplayAdds = GetComponent<DisplayAdds>();
        _DisplayAdds.OnRewardGiven += OnAddExtraTime;
#endif
        _MenuManager = GetComponent<MenuManager>();
    }
    private void OnDisable()
    {
#if UNITY_ANDROID
        _DisplayAdds.OnRewardGiven -= OnAddExtraTime;
#endif
    }
    void Start()
    {
#if UNITY_ANDROID
        _DisplayAdds.OnRewardGiven += OnAddExtraTime;
        
#endif
        SetHighScoreText();
        StatReset();
    }
    public void OnAddExtraTime()
    {
        Timer = ExtraTime;
        SecondsLeftText.SetText(Timer.ToString());
        _MenuManager.CloseMenu(RewardAdMenu);
    }
    public void OnButtonPress()
    {
        if (!Playing && !_MenuManager.IsMenuOpen)
        {
            Playing = true;
            TextToDisableOnStart.SetActive(false);
            StartCoroutine(ReduceTimer1Second());
            _MenuManager.IsGameOn = true;
        }
        AddClick();

    }

    private void AddClick()
    {
        ClicksDone++;
        ClicksDoneText.SetText(ClicksDone.ToString());
    }
    private IEnumerator ReduceTimer1Second()
    {
        SecondsLeftText.SetText(Timer.ToString());
        yield return new WaitForSeconds(1);
        Timer--;
        if (Timer > 0)
        {
            StartCoroutine(ReduceTimer1Second());
        }
        else
        {
            SecondsLeftText.SetText(Timer.ToString());
            GameEnd();
        }
    }

    private void GameEnd()
    {
        Playing = false;
        _MenuManager.IsGameOn = false;
        bool IsNewHighScore = SetHighScore();
#if UNITY_ANDROID
        if (!IsNewHighScore)
        {
            _DisplayAdds.ShowAd();
        }
#endif
        _MenuManager.OpenMenu(EndGameMenu);
        if (EndGameMenu.activeSelf)
        {
            EndGameScoreText.SetText(ClicksDone.ToString());
        }
        TextToDisableOnStart.SetActive(true);
        StatReset();
    }
    private void StatReset()
    {
        Timer = DefaultClickTime;
        ClicksDone = 0;
        SecondsLeftText.SetText(Timer.ToString());
        ClicksDoneText.SetText(ClicksDone.ToString());
    }
    private bool SetHighScore() 
    {
        if (!PlayerPrefs.HasKey(HighScorePlayerRefTag) || PlayerPrefs.HasKey(HighScorePlayerRefTag) && ClicksDone > PlayerPrefs.GetInt(HighScorePlayerRefTag))
        {
            PlayerPrefs.SetInt(HighScorePlayerRefTag, ClicksDone);
            PlayerPrefs.Save();
            SetHighScoreText();
            return true;
        }
        return false;
    }

    private void SetHighScoreText()
    {
        if (PlayerPrefs.HasKey(HighScorePlayerRefTag))
        {
            HiScoreText.SetText(PlayerPrefs.GetInt(HighScorePlayerRefTag).ToString());
        }
    }
    [ContextMenu("Reset High Score")]
    private void ResetHighScore()
    {
        if (PlayerPrefs.HasKey(HighScorePlayerRefTag))
        {
            PlayerPrefs.SetInt(HighScorePlayerRefTag, 0);
        }
    }
}
