using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveCommand : Command
{
    private PlayerUnit playerUnit;
    private int prevXIndex = 0;
    private int prevYIndex = 0;
    
    public int newXIndex = 0;
    public int newYIndex = 0;    

    public GridMoveCommand(PlayerUnit associatedUnit, int newX, int newY)
    {
        this.playerUnit = associatedUnit;

        this.prevXIndex = 0;
        this.prevYIndex = 0;
        this.newXIndex = newX;
        this.newYIndex = newY;
    }

    public override void Execute()
    {
        this.prevXIndex = this.playerUnit.currentXIndex;
        this.prevYIndex = this.playerUnit.currentYIndex;

        this.playerUnit.MovePlayer(this.newXIndex, this.newYIndex, false);
    }

    public override void Undo()
    {
        this.playerUnit.MovePlayer(this.prevXIndex, this.prevYIndex, true);
    }
}
