using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private TMP_Text UIScore;
    [SerializeField] private int scoreDigitsAmount = 8;
    [SerializeField] private char scoreDigitsFill = '0';
#pragma warning restore 0649

    private void Awake()
    {
        if (UIScore == null)
            Debug.LogError("EL " + typeof(TMP_Text) + " ES NULO EN " + nameof(UIScore));
        else
            UpdateUIScore();
    }

    public int Score { get; private set; } = 0;

    public void SumarScore(int score)
    {
        Score += score;

        UpdateUIScore();
    }

    private void UpdateUIScore()
    {
        if (UIScore != null)
            UIScore.text = Score.ToString().PadLeft(scoreDigitsAmount, scoreDigitsFill);
    }
}