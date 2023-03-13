using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : AState
{
    public override void Enter(AState from)
    {
        gameObject.SetActive(true);
        TrackManager.Instance.CancelGenerateMethods();
    }

    public override void Exit(AState to)
    {
        gameObject.SetActive(false);
    }

    public override void Tick()
    {
        
    }

    public override string GetName()
    {
        return "GameOver";
    }
    
    public void GoToLoadout()
    {
        manager.SwitchState("LoadOut");
    }
    
    public void RunAgain()
    {
        manager.SwitchState("Game");
    }
}
