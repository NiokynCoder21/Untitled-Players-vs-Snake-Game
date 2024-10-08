using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager1 : MonoBehaviour
{
    public static ScoreManager1 Instance; //this allows me to call this scripts functions anywhere whithou having to assign in insepctor 

    public TMP_Text scoreText;

    public int score;
    public bool isKitchen = false;
    public bool isLivingRoom = false;
    public bool isDiningRoom = false;
    public ItemManager1 itemManager;

    public PlayerMovement1 movement;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        score = 0;
        UpdateScoreText();
    }


    public void AwardKitchenPoints(int more)
    {
        if (isKitchen == true)
        {
            if (itemManager != null)
            {
                if (itemManager.currentKitchenStuff > 0)
                {
                    score += more;
                    UpdateScoreText();
                }
            }
        }
    }

    public void AwardLivingPoints(int more)
    {
        if (isLivingRoom == true)
        {
            if (itemManager != null)
            {
                if (itemManager.currentLivingStuff > 0)
                {
                    score += more;
                    UpdateScoreText();
                }

            }
        }
    }

    public void AwardDiningPoints( int more)
    {
        if (isDiningRoom == true)
        {
            if (itemManager != null)
            {
                if (itemManager.currentDiningStuff > 0)
                {
                    score += more;
                    UpdateScoreText();
                }
            }
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "" + score; //update text to show the current score
    }

    public void SetIsLivingRoom(bool state)
    {
        isLivingRoom = state;
    }

    public void SetIsDiningRoom(bool state)
    {
        isDiningRoom = state;
    }

    public void SetIsKitchen(bool state)
    {
        isKitchen = state;
    }

}
