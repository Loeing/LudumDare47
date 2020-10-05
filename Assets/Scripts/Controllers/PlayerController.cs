using UnityEngine;

public class PlayerController : MoveController
{
    private void Update() {
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
            Move(Direction.Right);        }
    }
}