using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameMaster : MonoBehaviour {

    public static GameMaster instance = null;    
    private Board board;
    private int level = 1;

    public bool playerTurn = true;
    public List<Item> items;
    public List<EnemyController> enemies;
    //TODO: add list of past yous
    

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
    private void InitGame()
    {
        board.InitGame(1);
    }

    private void Update()
    {
        if(!playerTurn)
        {
            MoveEnemies();
        }
    }

    private void MoveEnemies()
    {
        foreach(EnemyController enemy in enemies)
        {
            enemy.MoveRand();
        }
        playerTurn = true;
    }
}