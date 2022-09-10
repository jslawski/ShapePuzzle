using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : MonoBehaviour
{
    private float worldSpaceMoveIncrement = 1f;

    private int maxXIndex = 5;
    private int maxYIndex = 5;

    public int currentXIndex = 0;
    public int currentYIndex = 0;

    private GridMoveCommand currentCommand;
    private Stack<GridMoveCommand> moveHistory;
    
    private int maxMoves = 10;

    // Start is called before the first frame update
    void Start()
    {
        this.moveHistory = new Stack<GridMoveCommand>();
        this.MovePlayer(this.currentXIndex, this.currentYIndex);
    }

    // Update is called once per frame
    void Update()
    {
        this.currentCommand = this.HandleInput();

        if (this.currentCommand != null && this.moveHistory.Count < this.maxMoves)
        {
            this.currentCommand.Execute();

            //Only store the command if the move is valid (is still within bounds)
            if (this.IsWithinBounds(this.currentCommand.newXIndex, this.currentCommand.newYIndex))                
            {
                this.moveHistory.Push(this.currentCommand);
            }
        }
    }

    GridMoveCommand HandleInput()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            int newYIndex = this.currentYIndex + 1;
            return new GridMoveCommand(this, this.currentXIndex, newYIndex);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            int newYIndex = this.currentYIndex - 1;
            return new GridMoveCommand(this, this.currentXIndex, newYIndex);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            int newXIndex = this.currentXIndex - 1;
            return new GridMoveCommand(this, newXIndex, this.currentYIndex);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            int newXIndex = this.currentXIndex + 1;
            return new GridMoveCommand(this, newXIndex, this.currentYIndex);
        }
        if (Input.GetKeyUp(KeyCode.Backspace) && this.moveHistory.Count > 0)
        {            
            Command lastCommand = this.moveHistory.Pop();
            lastCommand.Undo();
        }

        return null;
    }

    private bool IsWithinBounds(int xValue, int yValue)
    {
        return (xValue > 0 && xValue <= this.maxXIndex &&
            yValue > 0 & yValue <= this.maxYIndex);
    }

    public void MovePlayer(int xIndex, int yIndex)
    {
        //Do not move the player if it would put them "out of bounds"
        if (this.IsWithinBounds(xIndex, yIndex) == false)
        {
            return;
        }

        this.currentXIndex = xIndex;
        this.currentYIndex = yIndex;

        float xIncrement = this.currentXIndex * this.worldSpaceMoveIncrement;
        float yIncrement = this.currentYIndex * this.worldSpaceMoveIncrement;

        this.transform.position = new Vector3(xIncrement, yIncrement, 0.0f);
    }
}
