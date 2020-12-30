using UnityEngine;

public enum Team 
{
    Player,
    Enemy,
    Object
}

//not having this be an abtract monobehavior is probably dumb
//should just use the monobehavior as intended
// only benefit of this is defining everything in the json
public abstract class Entity : MonoBehaviour 
{
    
   // public GameObject prefab;
    public Node currentNode;
    public Team team; 
    public int MaxHealth { get; set; } = 1;
    public int Health { get; set; }
    public bool Visible { get; set; }
    private Renderer renderer;

    private void Awake() 
    {
        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
        Visible = false;
    }

    private void Start() 
    {
    }

    // public Entity(GameObject prefab, Node node, Team team, int maxHealth)
    // {
    //     this.prefab = prefab;
    //     this.currentNode = node;
    //     this.team = team;
    //     this.MaxHealth =  maxHealth;
    //     this.Health = maxHealth;
    // }

    public virtual void LoseHealth(int loss)
    {
        Health -= loss;
        if(Health <= 0)
        {
            DestroyEntity();
        }
    }

    protected virtual void DestroyEntity()
    {
        //TODO: handle animation, loot drop, etc
        currentNode.Leave();
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    public virtual void Draw()
    {
        renderer.enabled = true;
        Visible = true;
    }

    public virtual void Erase()
    {
        renderer.enabled = false;
        Visible = false;
    }

}