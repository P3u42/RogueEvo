using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager 
{
    public event System.Action OnTickTac;
   private int _turnCount;
   public TurnManager(){
    _turnCount = 1;
   }


   public void TickTac()
    {
        _turnCount+=1;
        Debug.Log("Current turn count is: " + _turnCount);
        OnTickTac?.Invoke();
    }
}
