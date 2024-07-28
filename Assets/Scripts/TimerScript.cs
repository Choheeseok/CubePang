using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    public float timeRemaining = 10; // 10분 (600초)
    public bool timerIsRunning = false;
    public Text timeText; // 타이머를 표시할 UI 텍스트
    public Text endText;

    private void Start()
    {
        // 타이머 시작
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                endText.gameObject.SetActive(true);
                timeRemaining = 0;
                timerIsRunning = false;
                StartCoroutine(WaitAndLoadScene(5)); // 5초 대기 후 씬 전환
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    IEnumerator WaitAndLoadScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("TitleScene");
    }
}