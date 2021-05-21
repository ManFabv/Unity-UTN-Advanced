using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private int MaxScoreParaGanar = 10;
    [SerializeField] private float TimeToLoseLevel = 10;
    [SerializeField] private TimeManager TimeManager;
    [SerializeField] private ScoreManager ScoreManager;
    [SerializeField] private AudioManager AudioManager;
    [SerializeField] private string NextWinLevelName;
    [SerializeField] private string NextLoseLevelName;
    [SerializeField] private string WinLevelName = "Ganar";
    [SerializeField] private string LoseLevelName = "Perder";
    [SerializeField] private float TimeBeforeLoadNextLevel = 9;
    [SerializeField] private GameUI GameUI;
#pragma warning restore 0649

    private enum LevelEndState
    {
        PLAYING,
        WIN,
        LOSE
    }

    private bool finishedLevel = false;
    private LevelEndState levelEndState = LevelEndState.PLAYING;
    private bool jugadorMurio = false;

    public void OnJugadorMurio()
    {
        jugadorMurio = true;
    }

    private void Awake()
    {
        if (GameUI == null)
            Debug.LogError("EL " + typeof(GameUI) + " ES NULO EN " + nameof(GameUI));
        if (ScoreManager == null)
            Debug.LogError("EL " + typeof(ScoreManager) + " ES NULO EN " + nameof(ScoreManager));
        if (TimeManager == null)
            Debug.LogError("EL " + typeof(TimeManager) + " ES NULO EN " + nameof(TimeManager));
        if (AudioManager == null)
            Debug.LogError("EL " + typeof(AudioManager) + " ES NULO EN " + nameof(AudioManager));
        if(string.IsNullOrEmpty(NextWinLevelName))
            Debug.LogError("NO SE ESPECIFICO UN SIGUIENTE NIVEL EN " + nameof(NextWinLevelName));
        if(string.IsNullOrEmpty(NextLoseLevelName))
            Debug.LogError("NO SE ESPECIFICO UN SIGUIENTE NIVEL EN " + nameof(NextLoseLevelName));
        if(string.IsNullOrEmpty(WinLevelName))
            Debug.LogError("NO SE ESPECIFICO UN SIGUIENTE NIVEL EN " + nameof(WinLevelName));
        if(string.IsNullOrEmpty(LoseLevelName))
            Debug.LogError("NO SE ESPECIFICO UN SIGUIENTE NIVEL EN " + nameof(LoseLevelName));
    }

    private void Update()
    {
        if (finishedLevel == false)
        {
            bool win = (ScoreManager != null && ScoreManager.Score >= MaxScoreParaGanar);
            bool lose = (TimeManager != null && TimeManager.TimeElapsed >= TimeToLoseLevel) || jugadorMurio;

            if (win || lose)
            {
                if (win)
                {
                    levelEndState = LevelEndState.WIN;
                    AudioManager?.PlayWinMusic();
                    GameUI?.ShowWinLevel();
                }
                else if (lose)
                {
                    levelEndState = LevelEndState.LOSE;
                    AudioManager?.PlayLoseMusic();
                    GameUI?.ShowLoseLevel();
                }
                
                finishedLevel = true;
                DisableAllAction();
                Invoke("LoadNextLevel", TimeBeforeLoadNextLevel);
            }
        }
    }

    private void DisableAllAction()
    {
        DisableObjectsOfType<GeneradorOleadas>();
        DisableObjectsOfType<PlayerMovement>();
        DisableObjectsOfType<PlayerLook>();
        DisableObjectsOfType<PlayerAnimatorController>();
        DisableObjectsOfType<PlayerShoot>();
        DisableObjectsOfType<EnemyMovement>();
        DisableObjectsOfType<EnemyAnimationController>();
    }

    private void DisableObjectsOfType<T>() where T : MonoBehaviour
    {
        T[] entidades = GameObject.FindObjectsOfType<T>();
        if (entidades != null)
        {
            foreach (T entidad in entidades)
            {
                entidad.enabled = false;
            }
        }
    }

    private void LoadNextLevel()
    {
        string levelToLoad = string.Empty;
        if (levelEndState == LevelEndState.WIN)
        {
            levelToLoad = WinLevelName;
            LevelManager.NextLevel = NextWinLevelName;
            LevelManager.CurrentLevel = NextLoseLevelName;
        }
        else if (levelEndState == LevelEndState.LOSE)
        {
            levelToLoad = LoseLevelName;
            LevelManager.NextLevel = NextLoseLevelName;
            LevelManager.CurrentLevel = NextLoseLevelName;
        }

        if (!string.IsNullOrEmpty(levelToLoad))
            SceneManager.LoadScene(levelToLoad);
    }

    public void LevelQuit()
    {
        finishedLevel = true;
        DisableAllAction();
    }
}