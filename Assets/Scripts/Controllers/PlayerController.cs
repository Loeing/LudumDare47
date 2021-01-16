using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    private Board board;
    private GameMaster gm;
    private Entity entity;
    private MoveComponent moveComponent;

    private void Start()
    {
        gm = GameMaster.instance;
        board = Board.instance;
        //Hopefully this is enough
        entity = GetComponent<Entity>();
        moveComponent = GetComponent<MoveComponent>();
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
        moveComponent.Move(dir);
        // Debug.Log("current Node Room : " + entity.currentNode.room);
        // Debug.Log("board Room: " + board.currentRoom.Key);
        // Debug.Log("current Node: " + entity.currentNode.gridPos);
        if(entity.currentNode.room != board.currentRoom.Key)
        {
            //TODO: add somesort of screen wipe
            board.Erase();
            board.Draw(gm.rooms[entity.currentNode.room]);
        }
        gm.playerTurn = false; //should probably separate out player turn sutff
    }
}