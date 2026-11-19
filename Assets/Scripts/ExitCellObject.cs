using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class ExitCellObject : CellObject
{


    [SerializeField]private Tile exitTile;




    public override void Init(Vector2Int cell)
    {
        base.Init(cell);
        GameManager.Instace.GetBoardManager().SetCellTile(cell,exitTile);
    }


    public override void PlayerEntered()
    {
        base.PlayerEntered();
        Debug.Log("Reached the end of the dungeon Floor!");
        GameManager.Instace.NewLevel();
    }
}
