using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameMaster : MonoBehaviour {

    public static GameMaster instance = null;    
    private Board board;
    private int level = 1;

    public List<Item> items;
    public List<EnemyController> enemies;
    

    private void Awake() {
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        board = GetComponentsInChildren<Board>()[0];

    }

    private void Start() {
        
        InitGame();
    }
   void InitGame()
    {
        board.InitGame(1);
    }
}