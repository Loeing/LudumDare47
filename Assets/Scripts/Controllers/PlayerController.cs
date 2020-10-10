using UnityEngine;

public class PlayerController : MoveController
{
    private GameMaster gm;

    private void Start()
    {
        gm = GameMaster.instance;
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
        gm.playerTurn = false;
    }
}