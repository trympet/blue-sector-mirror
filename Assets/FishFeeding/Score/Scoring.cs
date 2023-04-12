using System.Collections;
using System.Collections.Generic;
using TMPro;
using BNG;
using UnityEngine;
using UnityEngine.UI;

public class Scoring : MonoBehaviour
{
    private int score = 0;
    public int Score => score;
    private GameObject[] merds;
    /*[SerializeField]
    private int time = 60;
    private float gameTimeLeft;*/
    private int deadFish;
    public int DeadFish => deadFish;
    private float foodWasted;
    public float FoodWasted => foodWasted;
    private float foodWastedPercentage;
    public float FoodWastedPercentage => foodWastedPercentage;
/*    private TextMeshProUGUI endScoreText;
    private TextMeshProUGUI timeLeft;
    private TextMeshProUGUI currentScore;
    private TextMeshProUGUI deadFishText;
    private TextMeshProUGUI foodWasteText;
    private UnityEngine.UI.Slider foodWasteSlider;*/
    private MerdCameraController merdCameraController;
    [SerializeField]
    private GameObject MerdCameraHost;


    // Start is called before the first frame update
    void Start()
    {
        merds = GameObject.FindGameObjectsWithTag("Fish System");
        /*endScoreText = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<TextMeshProUGUI>();
        GameObject canvas = GameObject.FindGameObjectWithTag("MonitorMerdCanvas");
        timeLeft = canvas.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        currentScore = canvas.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
        deadFishText = canvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteText = canvas.transform.GetChild(3).gameObject.GetComponent<TextMeshProUGUI>();
        foodWasteSlider = canvas.transform.GetChild(4).gameObject.GetComponent<UnityEngine.UI.Slider>();*/
        merdCameraController = MerdCameraHost.GetComponent<MerdCameraController>();
    }

    public void StartScoring()
    {
        score = 0;
        InvokeRepeating(nameof(UpdateScore), 1.0f, 1.0f);
        Debug.Log("Score: " + score);
    }

    public void StopScoring()
    {
        CancelInvoke("UpdateScore");
    }

    /* Goes through every merd and checks if it is full, hungry or dead. Based on the status to the merd 
     * add or subtract from the total score of the game. */
    void UpdateScore()
    {
        foreach (GameObject i in merds)
        {
            FishSystemScript script = i.GetComponent<FishSystemScript>();
            if (script.state == FishSystemScript.FishState.Full)
            {
                score += 1 * script.amountOfFish;
                Debug.Log("State Full, amount of fish: " + script.amountOfFish);
            }
            else if (script.state == FishSystemScript.FishState.Hungry)
            {
                score -= (int)(0.5 * script.amountOfFish);
                Debug.Log("State Hungry, amount of fish: " + script.amountOfFish);
            }
            else
            {
                score -= (int)(0.5 * (script.amountOfFish - script.fishKilled));
                score -= 1 * script.fishKilled;
                Debug.Log("State Dying, amount of fish: " + script.fishKilled);
            }

            Debug.Log("FishSystemScript status: " + script.state);
            float wastedFoodPoints = script.foodWasted / 10;
            score -= (int)(wastedFoodPoints + 0.5f); // Rounds wastedFoodPoints to the nearest int.
            Debug.Log("Wasted food points: " + wastedFoodPoints);
            Debug.Log("Score after food waste: " + score);
        }

        UpdateFoodWasteAndDeadFish();
    }

    /* Updates the food waste, food waste percentage and the amount of dead fish depending on which merd is being shown on the screen. */
    void UpdateFoodWasteAndDeadFish()
    {
        foreach (GameObject i in merds)
        {
            FishSystemScript script = i.GetComponent<FishSystemScript>();
            Debug.Log("if setning" + (merdCameraController.SelectedFishSystem == script));
            if (merdCameraController.SelectedFishSystem == script)
            {
                foodWasted = script.foodWasted;
                foodWastedPercentage = script.foodWasted / (script.foodBase * 5 / 3);
                deadFish = script.fishKilled;
            }
        }
    }

    /* Updates the timer, score, food waste and the amount of dead fish on the merd screen. *//*
    public void UpdateScreenStats()
    {
        timeLeft.text = "Time left: " + Mathf.FloorToInt(gameTimeLeft / 60) + ":" +
            Mathf.FloorToInt(gameTimeLeft % 60).ToString("00");
        currentScore.text = "Score: " + score;
        foodWasteText.text = "Food wastage: " + foodWasted + " / Sec.";
        foodWasteSlider.value = foodWastedPercentage;
        deadFishText.text = "Dead fish: " + deadFish;
    }*/

}
