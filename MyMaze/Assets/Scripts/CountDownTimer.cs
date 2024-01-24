using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public float countdownTime = 60.0f;  // Set the initial countdown time in seconds
    public Text countdownText;  // Reference to the UI Text component to display the countdown
    private SimpleMazeGenerator m_Generator;
    void Start()
    {
        // Call the Countdown function when the script starts
        StartCountdown();
        countdownText.GetComponent<Text>();
        m_Generator=FindAnyObjectByType<SimpleMazeGenerator>();
    }

    void Update()
    {
        // Update the countdown timer every frame
        countdownTime -= Time.deltaTime;

        // Ensure that the countdown doesn't go below zero
        if (countdownTime <= 0)
        {
            countdownTime = 0;
            // You can add any additional actions or logic when the countdown reaches zero here
            m_Generator.RestartLevel();
        }
       
        // Update the UI Text component with the current countdown time
        UpdateCountdownText();
    }

    void UpdateCountdownText()
    {
        // Display the current countdown time as an integer
        countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
    }
    public void ExtractOneSecond(float extract)
    {
        countdownTime -= extract;

        // Ensure that the countdown doesn't go below zero
        if (countdownTime < 0)
        {
            countdownTime = 0;
            // You can add any additional actions or logic when the countdown reaches zero here
        }

        // Update the UI Text component with the current countdown time
        UpdateCountdownText();
    }
    void StartCountdown()
    {
        // Optionally, you can add any initial setup or actions when the countdown starts
    }
}
