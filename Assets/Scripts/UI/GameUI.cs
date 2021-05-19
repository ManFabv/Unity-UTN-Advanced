using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float SecondsToTransition = 3;
    [SerializeField] private AudioManager AudioManager;
    [SerializeField] private string Menu_Level_Name = "Menu";
    [SerializeField] private UIAnimator LoaderScreen;
    [SerializeField] private GameLevelManager GameManager;
    [SerializeField] private TMP_Text WinText;
    [SerializeField] private TMP_Text LoseText;
#pragma warning restore 0649
    
    private void Awake()
    {
        if(WinText == null)
            Debug.LogError("EL " + typeof(TMP_Text) + " ES NULO EN " + nameof(WinText));
        if(LoseText == null)
            Debug.LogError("EL " + typeof(TMP_Text) + " ES NULO EN " + nameof(LoseText));
        if(GameManager == null)
            Debug.LogError("EL " + typeof(GameLevelManager) + " ES NULO EN " + nameof(GameManager));
        if(string.IsNullOrEmpty(Menu_Level_Name))
            Debug.LogError("EL " + typeof(string) + " ES NULO EN " + nameof(Menu_Level_Name));
        if(LoaderScreen == null)
            Debug.LogError("EL " + typeof(UIAnimator) + " ES NULO EN " + nameof(LoaderScreen));
        if(AudioManager == null)
            Debug.LogError("EL " + typeof(AudioManager) + " ES NULO EN " + nameof(AudioManager));
    }
    
    public void ReturnToMainMenu()
    {
        AudioManager?.TransitionToMuteSnapshot();
        FadeOutLoaderScreen();
        GameManager?.LevelQuit();
        Invoke("LoadScene", SecondsToTransition);
    }

    public void ShowWinLevel()
    {
        if(WinText != null)
            WinText.enabled = true;
    }
    
    public void ShowLoseLevel()
    {
        if(LoseText != null)
            LoseText.enabled = false;
    }

    private void FadeOutLoaderScreen()
    {
        LoaderScreen?.FadeOut();
    }
    
    private void LoadScene()
    {
        if (string.IsNullOrEmpty(Menu_Level_Name))
            Debug.LogError("NOT SPECIFIED SCENE TO LOAD ON " + nameof(Menu_Level_Name));
        else
            SceneManager.LoadScene(Menu_Level_Name);
    }
}
