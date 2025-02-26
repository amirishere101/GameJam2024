using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerScript : MonoBehaviour
{

    // References various stats assigned to the player
    public PlayerStats pStats, p2Stats;

    // References to the different UI elements
    public TextMeshProUGUI scoreText, timer;
    public Image crankyIcon;
    public Slider crankyBar;

    // References to the canvases that each
    // display the 1P, 2P, and Results UI
    public GameObject singlePlayCanvas, twoPlayCanvas, resultsCanvas;

    // Holds the sprites for the Cranky Face (P1 and P2)
    public Sprite p1CrankyFace, p1CrankyNeutral, p1CrankyDevious;
    public Sprite p2CrankyFace, p2CrankyNeutral, p2CrankyDevious;

    [Header("Timer Values")]
    public int maxTime = 250;
    // Flag for whether time is up or not
    public bool isTimeUp = false;

    // Internal counter for the timer
    private int timerCounter;
    private float elapsedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize slider
        crankyBar.maxValue = pStats.numSquirrelsExcited;

        // Initialize timer
        timerCounter = maxTime;
        timer.text = maxTime.ToString();
        isTimeUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Seconds counter updates every frame with the value of deltaTime
        // Once it reaches one second, it is reset back to zero
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 1.0f && isTimeUp == false)
        {
            updateTimer();
            elapsedTime = 0;
            if (timerCounter == 0)
                isTimeUp = true;
        }

        if (isTimeUp)
        {
            resultsCanvas.SetActive(true);
        }
        else
        {
            resultsCanvas.SetActive(false);
        }

        // Calls and update for the UI every frame
        writeScore();
        setCrankyIcon(pStats.currentState);
        managePlayerScreens(isTimeUp);
    }

    // Updates the score text and changes the Cranky bar value tot he new Squirrel score
    private void writeScore()
    {
        scoreText.text = pStats.numSquirrels.ToString();
        crankyBar.value = pStats.numSquirrels;
    }

    /**
        Sets the icons of Cranky's face and the meter based on Cranky's state
        @param state: the state of Cranky in enum form
            accepts "Cranky", "Normal", and "Excited"
    */
    private void setCrankyIcon(PlayerStats.DogState state)
    {
        switch (state)
        {
            case PlayerStats.DogState.Cranky:
                crankyIcon.sprite = p1CrankyFace;
                break;

            case PlayerStats.DogState.Normal:
                crankyIcon.sprite = p1CrankyNeutral;
                break;

            case PlayerStats.DogState.Excited:
                crankyIcon.sprite = p1CrankyDevious;
                break;
        }
    }

    // Decreases the timer and changes the timer text to the new count
    private void updateTimer()
    {
        timerCounter--;
        timer.text = timerCounter.ToString();
    }

    private void managePlayerScreens(bool isTimeUp)
    {
        singlePlayCanvas.SetActive(!isTimeUp);
        twoPlayCanvas.SetActive(!isTimeUp);
    }
}
