using UnityEngine;

public class ScoreOnDeath : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private int score = 10;
    [SerializeField] private ScoreManager ScoreManager;
#pragma warning restore 0649

    public void SetScoreManager(ScoreManager scoreManager)
    {
        if(scoreManager == null)
            Debug.LogError("EL " + typeof(ScoreManager) + " ES NULO EN " + nameof(ScoreManager));
        else
            ScoreManager = scoreManager;
    }

    public void Murio()
    {
        ScoreManager?.SumarScore(score);
    }
}