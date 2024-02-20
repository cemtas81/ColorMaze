using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public float countdownTime = 60.0f;  // Set the initial countdown time in seconds
    public Text countdownText;  // Reference to the UI Text component to display the countdown
    private SimpleMazeGenerator m_Generator;
    public Animator anim;
    private bool over;
    private PlayerMovement player;
    void Start()
    {
        // Call the Countdown function when the script starts
        player=FindObjectOfType<PlayerMovement>();   
        StartCountdown();
        countdownText.GetComponent<Text>();
        m_Generator=FindAnyObjectByType<SimpleMazeGenerator>();
        over = false;
    }

    void Update()
    {
        if (!over)
        {
            // Update the countdown timer every frame
            countdownTime -= Time.deltaTime;

            // Ensure that the countdown doesn't go below zero
            if (countdownTime <= 0)
            {
                countdownTime = 0;
                // You can add any additional actions or logic when the countdown reaches zero here
               
                player.enabled = false;
                StartCoroutine(GameOver(1));

            }

            // Update the UI Text component with the current countdown time
            UpdateCountdownText();
        }
       
    }

    public IEnumerator GameOver(int anime)
    {
        over = true;
        if (anime==1)
        {
            anim.SetTrigger("GameOver");
        }
        else
        {
            anim.SetTrigger("Abducted");
        }
        yield return new WaitForSecondsRealtime(3);
        m_Generator.RestartLevel();
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
