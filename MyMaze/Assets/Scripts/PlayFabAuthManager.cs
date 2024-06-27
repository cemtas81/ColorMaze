using UnityEngine;

public class PlayFabAuthManager : MonoBehaviour
{
    public LeaderboardCreator2 leaderboardManager;
    private int score;

    private void Start()
    {
        leaderboardManager.OnLoginSuccess += OnLoginSuccess;
        leaderboardManager.Login();
    }

    private void OnLoginSuccess()
    {
        OnSubmitScore();
        OnGetLeaderboard();
    }

    public void OnSubmitScore()
    {
        score = PlayerPrefs.GetInt("MainScore");
        leaderboardManager.SubmitScore(score);
    }

    public void OnGetLeaderboard()
    {
        leaderboardManager.GetLeaderboard();
    }
}
