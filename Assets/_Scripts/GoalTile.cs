using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTile : GameTile
{    
    private GameObject victoryCanvas;

    protected override void Start()
    {
        this.victoryCanvas = GetComponentInChildren<Canvas>(true).gameObject;
        
        base.Start();
    }

    public override void TraverseTile(PlayerUnit player)
    {
        //Do victory logic here
        this.victoryCanvas.SetActive(true);
        player.levelFinished = true;

        this.victoryCanvas.GetComponent<VictoryScreen>().SetupVictoryScreenTile(this);

        base.TraverseTile(player);
    }
}
