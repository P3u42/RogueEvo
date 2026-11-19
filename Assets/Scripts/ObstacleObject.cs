using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObstacleObject : CellObject
{
    [SerializeField] private Tile obstacleTile;
    [SerializeField]private int healthpoints;
    [SerializeField]public int maxHealth =3;
    private Tile originalTile;

    
    public override void Init(Vector2Int cell)
    {
        //By defalut allways call the base overriden function 
        base.Init(cell);
        healthpoints =maxHealth;
        originalTile = GameManager.Instace.GetBoardManager().GetCellTile(cell);
        GameManager.Instace.GetBoardManager().SetCellTile(cell,obstacleTile);

    }
    public override bool PlayerWantsToEnter()
    {   healthpoints-=1;
        if (healthpoints > 0)
        {
            return false;
        
        }
        
        GameManager.Instace.GetBoardManager().SetCellTile(_cell,originalTile);
        Destroy(gameObject);
        return true;
        
    }




    public Tile GetObstacleTile()
    {
        return this.obstacleTile;
    }
}
