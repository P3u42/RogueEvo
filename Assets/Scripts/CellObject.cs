using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    protected Vector2Int _cell;

   public virtual void PlayerEntered()
    {
        
    }

    public virtual void Init(Vector2Int cell)
    {
        this._cell = cell;
    }

    public virtual bool PlayerWantsToEnter()
    {
        return true;
    }
   
}
