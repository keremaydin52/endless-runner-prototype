using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameState : AState
{
    public TrackManager trackManager;
    public TextMeshProUGUI scoreText;
    
    public override void Enter(AState from)
    {
        gameObject.SetActive(true);
        StartGame();
    }

    public override void Exit(AState to)
    {
        gameObject.SetActive(false);
    }

    public override void Tick()
    {
        UpdateUI();
    }

    public override string GetName()
    {
        return "Game";
    }

    public void StartGame()
    {
        trackManager.Begin();
    }

    public void GameOver()
    {
        manager.SwitchState("GameOver");
    }

    public void UpdateUI()
    {
        scoreText.text = "Score: " + trackManager.Score;
    }
}
