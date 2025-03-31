using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TimerScript : MonoBehaviour
{
    private const float GAME_DURATION = 600f; // 10분
    private const float SCENE_TRANSITION_DELAY = 5f;
    private const string TITLE_SCENE_NAME = "TitleScene";

    [SerializeField] private Text timeText;
    [SerializeField] private Text endText;

    private float timeRemaining;
    private bool isTimerRunning;

    private void Start()
    {
        InitializeTimer();
    }

    private void Update()
    {
        if (!isTimerRunning) return;

        if (timeRemaining > 0)
        {
            UpdateTimer();
        }
        else
        {
            HandleTimerEnd();
        }
    }

    private void InitializeTimer()
    {
        timeRemaining = GAME_DURATION;
        isTimerRunning = true;
    }

    private void UpdateTimer()
    {
        timeRemaining -= Time.deltaTime;
        DisplayTime(timeRemaining);
    }

    private void HandleTimerEnd()
    {
        endText.gameObject.SetActive(true);
        timeRemaining = 0;
        isTimerRunning = false;
        StartCoroutine(WaitAndLoadScene(SCENE_TRANSITION_DELAY));
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private IEnumerator WaitAndLoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(TITLE_SCENE_NAME);
    }
}