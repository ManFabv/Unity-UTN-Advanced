using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private TMP_Text UIScore;
    [SerializeField] private int scoreDigitsAmount = 3;
    [SerializeField] private char scoreDigitsFill = '0';
    [SerializeField] private GameLevelManager GameLevelManager;
#pragma warning restore 0649

    private int uiScore = 0;

    private void Awake()
    {
        if (GameLevelManager == null)
            Debug.LogError("EL " + typeof(GameLevelManager) + " ES NULO EN " + nameof(GameLevelManager));
        if (UIScore == null)
            Debug.LogError("EL " + typeof(TMP_Text) + " ES NULO EN " + nameof(UIScore));
        else
            Invoke("UpdateUIScore", 1.0f);
    }

    public int Score { get; private set; } = 0;

    public void SumarScore()
    {
        Score++;
        uiScore++;
        
        UpdateUIScore();
        CheckScoreForCurrentWave();
    }

    public void UpdateUIScore()
    {
        if (UIScore != null)
            UIScore.text = uiScore.ToString().PadLeft(scoreDigitsAmount, scoreDigitsFill) + " / " + GameLevelManager.CurrentWaveEnemies();
    }

    public void CheckScoreForCurrentWave()
    {
        if (Score >= GameLevelManager.TotalSpawnedEnemiesSoFar())
        {
            GameLevelManager.StartCooldownBetweenWaves();
            uiScore = 0;
        }
    }
}