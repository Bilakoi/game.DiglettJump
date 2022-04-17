using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UISystem : MonoBehaviour
{
    public Text pointsText;
    public Text deathText;
    public Text timerText;
    public Text highscoreText;
    public GameObject youWin;
    public GameObject theEnd;
    int highscoreNumber;
    public static UISystem instance;
    public static int pointsNumber;
    public static int deathNumber;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    public void Start()
    {
        int pointsNumber = 0;
        pointsText.text = pointsNumber + " points";

        int deathNumber = 0;
        deathText.text = deathNumber + " deaths";


        youWin.SetActive(false); ;
        theEnd.SetActive(false);

        highscoreNumber = PlayerPrefs.GetInt("highestPoints", 0);

        
    }

    private void Update()
    {
        float timer = Time.timeSinceLevelLoad;
        int clock = (int)timer;
        int seconds = clock % 60;
        int minutes = (clock / 60) % 60;
        timerText.text = minutes.ToString("0") + ":" + seconds.ToString("00");

        pointsText.text = pointsNumber.ToString() + " points";

        if (highscoreNumber < pointsNumber)
        {
            PlayerPrefs.SetInt("highestPoints", pointsNumber);
        }
        highscoreText.text = "Highscore: " + highscoreNumber.ToString();

        
    }
    public void AddPoint()
    {
        pointsNumber += 1;

    }

    public void Add1MPoint()
    {
        pointsNumber += 50;
    }
    public void AddDeath()
    {
        deathNumber += 1;
        pointsNumber -= 5;
        deathText.text = deathNumber.ToString() + " deaths";
    }
    public void Text()
    {
        youWin.SetActive(true); ;
               
    }

    public void WinTextOff()
    {
        youWin.SetActive(false);  
        
    }

    public void TheEnd()
    {
        theEnd.SetActive(true);
        Time.timeScale = 0;
    }
    
}
