using UnityEngine;

public class PlayerController : MoveController
{
    private Board board;
    private GameMaster gm;

    private void Start()
    {
        gm = GameMaster.instance;
        board = Board.instance;
    }

    private void Update() 
    {
        if(gm.playerTurn)
        {   
            HandleInputs();           
        } 
    }

    private void HandleInputs()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Direction.Up);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Direction.Down);
        }
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Direction.Left);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Direction.Right);        
        }   
    }

    public void Move(Direction dir)
    {
        base.Move(dir);
        Debug.Log("current Node Room : " + currentNode.room);
        Debug.Log("board Room: " + board.currentRoom.Key);
        Debug.Log("current Node: " + this.currentNode.gridPos);
        if(this.currentNode.room != board.currentRoom.Key)
        {
            board.Erase();
            board.Draw(gm.rooms[this.currentNode.room]);
        }
        gm.playerTurn = false; //should probably separate out player turn sutff
    }
}