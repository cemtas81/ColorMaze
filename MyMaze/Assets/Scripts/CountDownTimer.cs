using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public float countdownTime = 60.0f;  // Set the initial countdown time in seconds
    public Text countdownText;  // Reference to the UI Text component to display the countdown
    private SimpleMazeGenerator m_Generator;
    public Animator anim;
    public bool over;
    private PlayerMovement player;
    public TMP_Text bonus, deploy, diamond, totalDiamond;
    public int bonusTime;
    public static CountDownTimer instance;
    public RectTransform bonusPlace;
    void Start()
    {
        // Call the Countdown function when the script starts
        player = FindObjectOfType<PlayerMovement>();
        StartCountdown();
        countdownText.GetComponent<Text>();
        m_Generator = FindAnyObjectByType<SimpleMazeGenerator>();
        over = false;
        totalDiamond.text = PlayerPrefs.GetInt("Score").ToString();
        
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
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

            player.enabled = false;
            StartCoroutine(GameOver(1));

        }

        // Update the UI Text component with the current countdown time
        UpdateCountdownText();

    }

    public IEnumerator GameOver(int anime)
    {

        if (anime == 1)
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
        if (countdownTime < 0)
        {
            countdownTime = 0;
        }
        // Display the current countdown time as an integer

        countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
        diamond.text = countdownText.text;

    }
    public void ExtractOneSecond()
    {

        bonusTime++;
        StartCoroutine(EraseBonusText(deploy, "-"));
    }
    IEnumerator EraseBonusText(TMP_Text bonus, String symbol)
    {

        yield return new WaitForSeconds(.2f);
        //Instantiate(bonus, bonusPlace);
        bonus.gameObject.SetActive(true);
        bonus.text = symbol + bonusTime.ToString();

        yield return new WaitForSeconds(.2f);
        bonus.gameObject.SetActive(false);
        if (symbol == "-")

            countdownTime -= bonusTime;
        else
            countdownTime += bonusTime;

        bonusTime = 0;
        UpdateCountdownText();

    }
    public void AddSecond()
    {

        bonusTime++;
        StartCoroutine(EraseBonusText(bonus, "+"));

    }
    void StartCountdown()
    {
        // Optionally, you can add any initial setup or actions when the countdown starts
    }
}
