using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameOverMenu : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI daysText;
    public TextMeshProUGUI bugsText;
    public TextMeshProUGUI plantsText;
    public TextMeshProUGUI plantsLostText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "" + GameStats.Points;
        daysText.text = "" + GameStats.Days;
        bugsText.text = "" + GameStats.Bugs;
        plantsText.text = "" + GameStats.Plants;
        plantsLostText.text = "" + GameStats.PlantsLost;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(GameConstants.Scenes.Game);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(GameConstants.Scenes.Menu);
    }
}
