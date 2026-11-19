
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : CellObject
{   
    [SerializeField]private EnemyDNA enemyDNA;
    [SerializeField] private CreatureBody creatureBody;
    [SerializeField] private HeadRotation headRotation;
    public int enemyHealth = 5;
    private int currentEnmyHealth;
    [SerializeField]private int damage;
    [SerializeField]SpriteRenderer enemySprite;
    PlayerController player; 

    public override void PlayerEntered()
    {
        GameManager.Instace.ChangeHealth(-damage);
        Debug.Log("Damage taken! Lost " + damage + "Health Points!");

    }


    private void Awake()
    {
        player = GameManager.Instace.GetPlayer();

        GameManager.Instace.turnManager.OnTickTac+=OnTurnHappenned;
        enemyDNA = this.GetComponent<EnemyDNA>();
        enemySprite = this.GetComponentInChildren<SpriteRenderer>();
        
       
        creatureBody  = GetComponent<CreatureBody>();   // ou GetComponentInChildren
        headRotation  = GetComponentInChildren<HeadRotation>();
        //SetEnemyFenotype();
       // enemyDNA.r =  Random.Range(0.0f,1.0f);
        //enemyDNA.g =  Random.Range(0.0f,1.0f);
        //enemyDNA.b =  Random.Range(0.0f,1.0f);
        //enemySprite.color = new Color(enemyDNA.r,enemyDNA.g,enemyDNA.b);
        
    }
    private void OnDestroy()
    {
        GameManager.Instace.turnManager.OnTickTac-=OnTurnHappenned;
    }
    public override void Init(Vector2Int cell)
    {   
     

        base.Init(cell);
        currentEnmyHealth = enemyHealth;
    }
    public override bool PlayerWantsToEnter()
    {
        currentEnmyHealth -=player.GetPlayerDamage();
        if (currentEnmyHealth <= 0)
        {
            //Destroy(gameObject);
            EnemyDeath();
            
        }
        //return base.PlayerWantsToEnter();
        return false;
    }

   


    void OnTurnHappenned()
    {
        var playerCell =  GameManager.Instace.GetPlayer().GetPlayerPos();
        int xDist =  playerCell.x  -_cell.x;
        int yDist =  playerCell.y -_cell.y;
        
        int absXDist =  Mathf.Abs(xDist);
        int absYDist =  Mathf.Abs(yDist);

        if((xDist==0&&absYDist==1)
        || (yDist == 0 && absXDist == 1))
        {
            GameManager.Instace.ChangeHealth(-damage);
            enemyDNA.damageCaused+=damage;
        }
        else
        {
            if (absXDist > absYDist)
            {
                if (!TryMoveInX(xDist))
                {
                    TryMoveInY(yDist);
                }
            }
            else
            {
                if (!TryMoveInY(yDist))
                {
                    TryMoveInX(xDist);
                }
            }
        }


    }

    private void MoveEnemy()
    {
     
    }
     private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player")
        {
            PlayerEntered();
        }
        
    }

    bool TryMoveInX(int xDist)
    {
        if (xDist > 0)
        {
            return MoveTo(_cell + Vector2Int.right);
        }
        return MoveTo(_cell+Vector2Int.left);
    }
    bool TryMoveInY(int yDist)
    {
        if (yDist > 0)
        {
            return MoveTo(_cell + Vector2Int.up);
        }
        return MoveTo(_cell+Vector2Int.down);
    }

    bool MoveTo(Vector2Int coord)
    {
        var board = GameManager.Instace.GetBoardManager();
        var targetCell = board.GetCellData(coord);
        if(targetCell==null
        ||!targetCell.isPassable
        || targetCell.ContaindObject != null)
        {
            return false;
        }
        var currentCell = board.GetCellData(_cell);
        currentCell.ContaindObject=null;
        targetCell.ContaindObject=this;
        _cell = coord;
        transform.position =board.CellToWorld(coord);
        return true;
    }

    private void EnemyDeath()
    {
            enemyDNA.isDead =  true;
            enemySprite.enabled = false;
            
            enemyDNA.deathTime = EnemyPopulationManager.elapsedTime;
            CalcFitness();
    }

    public void  CalcFitness()
    {
        int damageCaused =  enemyDNA.damageCaused;
        
        int fitness = (int)enemyDNA.deathTime + damageCaused;
        enemyDNA.SetFitness(fitness);
    }

    public void SetEnemyFenotype()
    {
        enemyHealth =  enemyDNA.healthGene;
        enemySprite.color = new Color(enemyDNA.r,enemyDNA.g,enemyDNA.b);
        damage =  enemyDNA.damageGene;
        creatureBody?.InitFromDNA(enemyDNA);
        headRotation?.InitFromDNA(enemyDNA);
        //var enemyS= enemyDNA.sCaleGene;
       //this.transform.localScale =  new Vector3(enemyS,enemyS,enemyS);
        
    }




}
