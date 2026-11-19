using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDNA : MonoBehaviour
{
    public float r;
    public float g;
    public float b;
    public int healthGene;
    public float sCaleGene;
    public bool isDead= false;
    public float deathTime = 0;
    public float bodySize;
    public float speed;
    public int damageGene;
    public int damageCaused;
    private int enemyFitness;
    public int bodyLengthGene;
    public float smoothSpeedGene;
    public float targetDistGene;
    public float wiggleSpeedGene;

    public float chageDirectionTimeGene;
    
    [SerializeField]SpriteRenderer enemySprite;
    // Start is called before the first frame update
    void Start()
    {
        enemySprite =GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  int  GetFitness()
    {
        return enemyFitness;
    }
    public void SetFitness(int fitness)
    {
        this.enemyFitness =  fitness;
    }
}
