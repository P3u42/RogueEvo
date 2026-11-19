using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionObject : CellObject
{
    [SerializeField]private int healthGranted =10;
    public override void PlayerEntered()
    {
        Destroy(gameObject);
        GameManager.Instace.ChangeHealth(healthGranted);
        Debug.Log("Health increased!");
    }
}
