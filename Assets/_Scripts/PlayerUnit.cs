using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class PlayerUnit : MonoBehaviour
{
    private float worldSpaceMoveIncrement = 2f;

    private float maxXPosition = 9;
    private float maxYPosition = 9;

    private GridMoveCommand currentCommand;
    private List<GridMoveCommand> moveHistory;

    private AudioSource playerMoveSound;
    private float pitchIncrement = 0.1f;

    //Max moves was originally part of the design,
    //but after playtesting it felt like an unnecessary limitation
    //Upped it to an arbitrarily large number so that it never triggers anything,
    //but the functionality is still there, if needed
    private int maxMoves = 100;

    [HideInInspector]
    public SpriteRenderer bgRenderer;
    [HideInInspector]
    public SpriteRenderer outlineRenderer;
    [HideInInspector]
    public SpriteRenderer fillRenderer;

    private GameTile nextTile;

    public bool undidLastMove = false;
    public bool levelFinished = false;

    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        this.moveHistory = new List<GridMoveCommand>();

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        this.bgRenderer = spriteRenderers[0];
        this.outlineRenderer = spriteRenderers[1];
        this.fillRenderer = spriteRenderers[2];

        this.playerMoveSound = GetComponent<AudioSource>();
        this.playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        this.currentCommand = this.HandleInput();

        if (this.currentCommand != null && this.moveHistory.Count < this.maxMoves)
        {
            this.currentCommand.Execute();

            if (this.IsValidMove(this.currentCommand.newXPosition, this.currentCommand.newYPosition))                
            {
                this.moveHistory.Add(this.currentCommand);
            }
        }
    }

    GridMoveCommand HandleInput()
    {
        if (this.levelFinished == false)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                float newYPosition = this.transform.position.y + this.worldSpaceMoveIncrement;
                return new GridMoveCommand(this, this.transform.position.x, newYPosition);
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                float newYPosition = this.transform.position.y - this.worldSpaceMoveIncrement;
                return new GridMoveCommand(this, this.transform.position.x, newYPosition);
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                float newXPosition = this.transform.position.x - this.worldSpaceMoveIncrement;
                return new GridMoveCommand(this, newXPosition, this.transform.position.y);
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                float newXPosition = this.transform.position.x + this.worldSpaceMoveIncrement;
                return new GridMoveCommand(this, newXPosition, this.transform.position.y);
            }
            if (Input.GetKeyUp(KeyCode.Backspace) && this.moveHistory.Count > 0)
            {
                Command lastCommand = this.moveHistory.Last();
                lastCommand.Undo();
                this.moveHistory.RemoveAt(this.moveHistory.Count - 1);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && this.levelFinished == true)
        {
            if (SceneManager.GetActiveScene().buildIndex < 2)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }

        return null;
    }

    private bool IsValidMove(float xValue, float yValue)
    {
        return (this.IsWithinBounds(xValue, yValue) &&
                this.IsNewMove(xValue, yValue) &&
                this.SharesAttribute(xValue, yValue));
    }

    //Player can only travel within the bounds of the game board
    private bool IsWithinBounds(float xValue, float yValue)
    {
        return (xValue > 0 && xValue <= this.maxXPosition &&
            yValue > 0 & yValue <= this.maxYPosition);
    }

    //Player can only travel to a space they haven't traveled to before
    private bool IsNewMove(float xValue, float yValue)
    {
        for (int i = 0; i < this.moveHistory.Count; i++)
        {
            if (xValue == moveHistory[i].newXPosition && yValue == moveHistory[i].newYPosition)
            {
                return false;
            }
        }

        return true;
    }

    //Player can only travel to a square that shares at least
    //one attribute with its current state
    //Attributes: Shape, Fill Color, Outline Color, Background Color
    private bool SharesAttribute(float xValue, float yValue)
    {
        this.nextTile = null;
        
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(xValue, yValue, -10.0f), Vector3.forward, out hit, Mathf.Infinity))
        {
            this.nextTile = hit.collider.gameObject.GetComponent<GameTile>();
        }
        
        if (this.nextTile == null)
        { 
            return false;
        }

        //Special case, prevents player from moving back to starting tile
        //without using Undo
        if (this.nextTile.traversed == true)
        {
            return false;
        }

        int attributeScore = 0;

        if (this.nextTile.fillRenderer.sprite.name == this.fillRenderer.sprite.name)
        {
            attributeScore++;
        }
        if (this.nextTile.bgRenderer.material.color == this.bgRenderer.material.color)
        {
            attributeScore++;
        }
        if (this.nextTile.fillRenderer.material.color == this.fillRenderer.material.color)
        {
            attributeScore++;
        }

        return (attributeScore > 1);
    }

    private void IndicateInvalidMove()
    {
        this.playerMoveSound.clip = Resources.Load<AudioClip>("Audio/InvalidMove");
        this.playerMoveSound.pitch = 1;
        this.playerMoveSound.Play();

        this.playerAnimator.SetTrigger("InvalidMoveTrigger");
    }

    public void MovePlayer(float xPosition, float yPosition, bool isUndo = false)
    {        
        //Do not move the player if it would put them "out of bounds"        
        if (this.IsWithinBounds(xPosition, yPosition) == false)
        {
            this.IndicateInvalidMove();
            return;
        }

        //Player cannot travel to tiles they've been to already
        //or tiles that don't share an attribute with their current game piece
        //UNLESS they are undoing a previous move
        if (isUndo == false)
        {
            if (this.IsNewMove(xPosition, yPosition) == false || 
                this.SharesAttribute(xPosition, yPosition) == false)
            {
                this.IndicateInvalidMove();
                return;
            }                
        }

        this.transform.position = new Vector3(xPosition, yPosition, -0.5f);
        this.playerMoveSound.clip = Resources.Load<AudioClip>("Audio/PlayerMove");
        this.playerMoveSound.pitch = (1 + (this.pitchIncrement * this.moveHistory.Count));
        this.playerMoveSound.Play();

        this.playerAnimator.SetTrigger("PlayerMoveTrigger");
    }

    private void UpdatePlayerAttributes(GameTile newTile)
    {        
        this.bgRenderer.material.color = newTile.bgRenderer.material.color;

        this.outlineRenderer.material.color = newTile.outlineRenderer.material.color;
        this.outlineRenderer.sprite = newTile.outlineRenderer.sprite;

        this.fillRenderer.material.color = newTile.fillRenderer.material.color;
        this.fillRenderer.sprite = newTile.fillRenderer.sprite;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "tile")
        {
            //Update Player Attributes
            GameTile collidedTile = other.GetComponent<GameTile>();


            this.UpdatePlayerAttributes(collidedTile);
            collidedTile.TraverseTile(this);
        }
    }

    //Special case, to reset the "traversed" state of the most
    //recent game tile on an Undo
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "tile" && this.undidLastMove == true)
        {
            other.GetComponent<GameTile>().ResetTraverseState();
        }
    }
}
