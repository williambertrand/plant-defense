using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    public float DayTime;
    public bool gameActive;
    public int currentDay;
    public Text dayText;

    public float currentScore;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        gameActive = true;
        StartCoroutine(IncreaseDay());
        GameStats.Clear();
    }

    // TODO: Potentially only update score here and in the plant update after an interval of frames has elapsed:
    // https://learn.unity.com/tutorial/fixing-performance-problems-2019-3#5e85ad9dedbc2a0021cb81aa

    private void FixedUpdate()
    {
        if (!gameActive) return;
        foreach (Plant p in PlantManager.Instance.activePlants)
        {
            currentScore += p.newPoints;
            p.newPoints = 0;
        }

        scoreText.text = "" + (int)currentScore;
    }


    public static string FormatDay(int day)
    {

        if (day >= 10 &&  day <= 20)
        {
            return day + "th";
        }
        int d = day % 10;
        string suffix;
        switch (d)
        {
            case 1:
                suffix = "st";
                break;
            case 2:
                suffix = "nd";
                break;
            case 3:
                suffix = "rd";
                break;
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 0:
                suffix = "th";
                break;
            default:
                suffix = "";
                break;
        }

        return "" + day + suffix;
    }

    private void UpdateDayText()
    {
        if (!dayText) return; // Can be called late afteer scene unloads
        dayText.text = FormatDay(currentDay) + " day of Spring";
    }


    IEnumerator IncreaseDay()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(DayTime);
            currentDay += 1;
            EnemyManager.Instance.IncreaseLevel();
            PlantManager.Instance.IncreaseDay();
            UpdateDayText();
        }
    }


    public void OnGameOver()
    {
        gameActive = false;
        GameStats.Days = currentDay;
        GameStats.Points = (int)currentScore;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

}
