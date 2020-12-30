
using UnityEngine;
using System.Collections.Generic;

public class Passage
{
    //public int srcRoom;
    // public int srcX;
    // public int srcY;

    public int destRoom;
    public int destX;
    public int destY;

    public Passage(int destRoom, int destX, int destY)
    {
        this.destRoom = destRoom;
        this.destX = destX;
        this.destY = destY;
    }   
}