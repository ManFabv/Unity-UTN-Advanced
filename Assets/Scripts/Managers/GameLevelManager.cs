using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private ScoreManager ScoreManager;
    [SerializeField] private AudioManager AudioManager;
    [SerializeField] private string NextWinLevelName;
    [SerializeField] private string NextLoseLevelName;
    [SerializeField] private string WinLevelName = "Ganar";
    [SerializeField] private string LoseLevelName = "Perder";
    [SerializeField] private float TimeBeforeLoadNextLevel = 9;
    [SerializeField] private GameUI GameUI;
    [SerializeField] private GeneradorOleadas[] GeneradorOleadas;
    [SerializeField] private EnemyWave[] EnemyWaves;
    [SerializeField] private float CooldownBetweenWaves = 10.0f;
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
    private bool castleDestroyed = false;
    private bool IsFinishedCurrentWave = true;
    private int CurrentWaveIndex = -1;

    public void OnJugadorMurio()
    {
        jugadorMurio = true;
    }

    public void OnCastleDestroyed()
    {
        castleDestroyed = true;
    }

    private void Awake()
    {
        if (GeneradorOleadas == null)
        {
            Debug.LogError("EL " + typeof(GeneradorOleadas) + " ES NULO EN " + nameof(GeneradorOleadas));
        }
        else
        {
            if (EnemyWaves == null)
            {
                Debug.LogError("EL " + typeof(EnemyWave) + " ES NULO EN " + nameof(EnemyWaves));
            }
            else
            {
                StartCooldownBetweenWaves();
                StartCoroutine(DelayedUIUpdate());
            }
        }
        if (GameUI == null)
            Debug.LogError("EL " + typeof(GameUI) + " ES NULO EN " + nameof(GameUI));
        if (ScoreManager == null)
            Debug.LogError("EL " + typeof(ScoreManager) + " ES NULO EN " + nameof(ScoreManager));
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

    private IEnumerator DelayedUIUpdate()
    {
        yield return new WaitForSeconds(TimeBeforeLoadNextLevel);
        GameUI.UpdateWaveNumber(CurrentWaveIndex);
    }

    private void Update()
    {
        if (finishedLevel == false)
        {
            bool win = CurrentWaveIndex >= EnemyWaves.Length;
            bool lose = jugadorMurio || castleDestroyed;

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
        DisableObjectsOfType<Damage>();
        DisableObjectsOfType<MovimientoContinuo>();
        StopAllParticleSystems();
    }

    private void StopAllParticleSystems()
    {
        ParticleSystem[] entidades = GameObject.FindObjectsOfType<ParticleSystem>();
        if (entidades != null)
        {
            foreach (ParticleSystem entidad in entidades)
            {
                entidad.Stop();
            }
        }
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

    public void StartCooldownBetweenWaves()
    {
        IsFinishedCurrentWave = true;
        CurrentWaveIndex++;
        if (CurrentWaveIndex < EnemyWaves.Length)
        {
            if(CurrentWaveIndex > 0)
                GameUI.ShowFinishedWaveText();
            
            Invoke("StartedCurrentSpawningWave", CooldownBetweenWaves);
        }
        else
        {
            CancelInvoke("StartedCurrentSpawningWave");
        }
    }

    public void StartedCurrentSpawningWave()
    {
        if(!IsFinishedCurrentWave) return;
        
        IsFinishedCurrentWave = false;
        
        if(CurrentWaveIndex > 0)
        {
            GameUI.UpdateWaveNumber(CurrentWaveIndex);
            ScoreManager.UpdateUIScore();
        }
            
        foreach (GeneradorOleadas generadorOleadas in GeneradorOleadas)
        {
            generadorOleadas.SpawnNewWaveEnemies(EnemyWaves[CurrentWaveIndex]);
        }
    }
    
    public void FinishedSpawningCurrentWave()
    {
        if(IsFinishedCurrentWave) return;
        
        IsFinishedCurrentWave = true;
    }

    public int TotalSpawnedEnemiesSoFar()
    {
        int totalSpawnedEnemiesSoFar = 0;
        for (int i = 0; i <= CurrentWaveIndex && i < EnemyWaves.Length; i++)
        {
            totalSpawnedEnemiesSoFar += EnemyWaves[i].MaxNumberOfSpawnedEnemies;
        }
        return totalSpawnedEnemiesSoFar;
    }

    public int CurrentWaveEnemies()
    {
        int wave = Math.Min(EnemyWaves.Length - 1, CurrentWaveIndex);
        return EnemyWaves[wave].MaxNumberOfSpawnedEnemies;
    }
}