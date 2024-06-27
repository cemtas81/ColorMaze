using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class LeaderboardCreator2 : MonoBehaviour
{
    public string playFabTitleId = "D069B"; // Your PlayFab Title ID
    public Text leaderboardText; // Assign your UI Text element in the inspector
    private string playerName;
    public delegate void LoginSuccessAction();
    public event LoginSuccessAction OnLoginSuccess;
    
    private bool isLoggedIn = false;

    void Start()
    {
        playerName=leaderboardText.text;
        PlayFabSettings.TitleId = playFabTitleId;
        Login();
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true // Attempt to create an account if it doesn't exist
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccessCallback, OnLoginFailure);
    }

    private void OnLoginSuccessCallback(LoginResult result)
    {
        Debug.Log("Login successful!");
        isLoggedIn = true;
        playerName = leaderboardText.text;
        OnLoginSuccess?.Invoke();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.AccountAlreadyLinked)
        {
            Debug.LogWarning("Account already linked. Logging in without creating a new account.");
            var request = new LoginWithCustomIDRequest
            {
                CustomId = SystemInfo.deviceUniqueIdentifier,
                CreateAccount = false
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccessCallback, OnLoginFailure);
        }
        else
        {
            Debug.LogError("Login failed: " + error.GenerateErrorReport());
        }
    }

    public void SubmitScore(int score)
    {
        if (!isLoggedIn)
        {
            Debug.LogWarning("Cannot submit score: user is not logged in.");
            return;
        }

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "HighScores",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnScoreSubmitSuccess, OnScoreSubmitFailure);
    }

    private void OnScoreSubmitSuccess(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Score submitted successfully!");
    }

    private void OnScoreSubmitFailure(PlayFabError error)
    {
        Debug.LogError("Score submission failed: " + error.GenerateErrorReport());
    }

    public void GetLeaderboard()
    {
        if (!isLoggedIn)
        {
            Debug.LogWarning("Cannot get leaderboard: user is not logged in.");
            return;
        }

        var request = new GetLeaderboardRequest
        {
            StatisticName = "HighScores",
            StartPosition = 0,
            MaxResultsCount = 25
        };
        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
    }

    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        string leaderboard = "";
        int rank = 1;
        foreach (var entry in result.Leaderboard)
        {
            // Check if the entry is for the current player
            string displayName = (entry.DisplayName == playerName) ? "You" : entry.DisplayName;
            leaderboard += string.Format("Rank {0}:   {1}    Score: {2}\n", rank, displayName, entry.StatValue);
            rank++;
        }
        leaderboardText.text = leaderboard;
    }

    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError("Failed to get leaderboard: " + error.GenerateErrorReport());
    }
}
