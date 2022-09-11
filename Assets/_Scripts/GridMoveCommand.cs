using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveCommand : Command
{
    private PlayerUnit playerUnit;
    private float prevXPosition = 0;
    private float prevYPosition = 0;
    
    public float newXPosition = 0;
    public float newYPosition = 0;    

    public GridMoveCommand(PlayerUnit associatedUnit, float newX, float newY)
    {
        this.playerUnit = associatedUnit;

        this.prevXPosition = 0f;
        this.prevYPosition = 0f;
        this.newXPosition = newX;
        this.newYPosition = newY;
    }

    public override void Execute()
    {
        this.prevXPosition = this.playerUnit.transform.position.x;
        this.prevYPosition = this.playerUnit.transform.position.y;

        this.playerUnit.MovePlayer(this.newXPosition, this.newYPosition, false);
    }

    public override void Undo()
    {
        this.playerUnit.MovePlayer(this.prevXPosition, this.prevYPosition, true);
    }
}
