using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

public class GameMaster : MonoBehaviour {

    public static GameMaster instance = null;    
    public Board board;
    private int level = 1;

    public bool playerTurn = true;
    public List<Item> items;
    public HashSet<EnemyController> enemies;
    //TODO: add list of past yous
    
    private LevelDataLoader _dataLoader;
    public Dictionary<int, Room> rooms;


    private void Awake() 
    {
        _dataLoader = GetComponent<LevelDataLoader>();    
        
        if(instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
        board = GetComponentsInChildren<Board>()[0];

    }

    private void Start() 
    {
        InitGame();
    }

    private void InitGame()
    {
        rooms = _dataLoader.GenerateRooms();

        enemies =  new HashSet<EnemyController>();//GameObject.FindObjectsOfType<EnemyController>().ToList();
        board.Draw(rooms[1]);
    }

    //TODO: manage rooms
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
            //enemies should choose next move/telegraph attacks before their turn
            enemy.MoveRand();
        }
        playerTurn = true;
    }
}