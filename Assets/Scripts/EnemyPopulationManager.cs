using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.CodeDom.Compiler;

public class EnemyPopulationManager : MonoBehaviour
{
    [SerializeField]private Enemy enemyPrefab;
    [SerializeField] private int popSize = 10;
    [SerializeField]BoardManager boardManager;
    private int mutationRate;
    private int generation =1;

    List<Vector2Int> emptyCellsList = new List<Vector2Int>();

    List<Enemy> enemyPopulation =  new List<Enemy>();
    public static float elapsedTime;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime+=Time.deltaTime;
    }


    public void GenerateEnemyPopulation()
    {
        for(int i = 0; i < popSize; i++)
        {
        emptyCellsList = boardManager.GetEmptyCellList();
        var index =  Random.Range(0,emptyCellsList.Count);
        Vector2Int spawnCoord =  emptyCellsList[index];
        emptyCellsList.RemoveAt(index);
        Enemy newEnemy =  Instantiate(enemyPrefab);
        EnemyDNA enemyDNA =  newEnemy.GetComponent<EnemyDNA>();
        SetFenotype(enemyDNA);
        newEnemy.SetEnemyFenotype();
        boardManager.AddObject(newEnemy,spawnCoord);
        enemyPopulation.Add(newEnemy);
        }
        
        /*CellData spawnData = boardData[spawnCoord.x,spawnCoord.y];
        Enemy newEnemy =  Instantiate(enemy);
        newEnemy.Init(spawnCoord);
        newEnemy.transform.position=CellToWorld(spawnCoord);
        spawnData.ContaindObject =  newEnemy;
        spawnData.isPassable =false;
        */
        Debug.Log("Generating first population!");
    }

    private void SetFenotype(EnemyDNA dna)
    {
        dna.r = Random.Range(0.0f,1.0f);
        dna.g = Random.Range(0.0f,1.0f);
        dna.b = Random.Range(0.0f,1.0f);
        //dna.sCaleGene = Random.Range(0.09f,0.17f);
        dna.healthGene = Random.Range(25,50);
        dna.damageGene= Random.Range(0,20);
         dna.bodyLengthGene          = Random.Range(3, 10);
        dna.wiggleSpeedGene         = Random.Range(1f, 5f);
          
        dna.smoothSpeedGene         = Random.Range(0.05f, 0.3f);
        dna.targetDistGene          = Random.Range(0.3f, 0.8f);
        dna.chageDirectionTimeGene = Random.Range(0.5f, 5f);
    }

    public void GenerateNewPopulation()
    {
        List<Enemy> alivePopulation = enemyPopulation.Where(o => o != null).ToList();
        if (alivePopulation.Count == 0)
        {
            GenerateEnemyPopulation();
            return;
        }

        List<Enemy> sortedPopulation = alivePopulation.OrderBy(o => o.GetComponent<EnemyDNA>().GetFitness()).ToList();
        enemyPopulation.Clear();

        for(int i = Mathf.Max(0, (int)(sortedPopulation.Count / 2.0f) - 1); i < sortedPopulation.Count - 1; i++)
        {
            enemyPopulation.Add(Crossover(sortedPopulation[i],sortedPopulation[i+1]));
            enemyPopulation.Add(Crossover(sortedPopulation[i+1],sortedPopulation[i]));
        }

        for(int i = 0; i < sortedPopulation.Count; i++)
        {
            if (sortedPopulation[i] != null)
            {
                Destroy(sortedPopulation[i].gameObject);
            }
        }
        generation++;
        Debug.Log("Generating new population!");
    }

    public Enemy Crossover(Enemy parent1,Enemy parent2)
    {
        emptyCellsList = boardManager.GetEmptyCellList();
        var index =  Random.Range(0,emptyCellsList.Count);
        Vector2Int pos =  emptyCellsList[index];
        emptyCellsList.RemoveAt(index);
        Enemy child =  Instantiate(enemyPrefab);
        boardManager.AddObject(child,pos);

        EnemyDNA dna1 = parent1.GetComponent<EnemyDNA>();
        EnemyDNA dna2 = parent2.GetComponent<EnemyDNA>();

        EnemyDNA childDNA = child.GetComponent<EnemyDNA>();
        if (Random.Range(0, 10) < 5)
        {
            childDNA.r =  Random.Range(0,10)<5? dna1.r:dna2.r;
            childDNA.g =  Random.Range(0,10)<5? dna1.g:dna2.g;
            childDNA.b =  Random.Range(0,10)<5? dna1.b:dna2.b;
            childDNA.damageGene =  Random.Range(0,10)<5? dna1.damageGene:dna2.damageGene;
            childDNA.healthGene= Random.Range(0,10)<5? dna1.healthGene:dna2.healthGene;
            childDNA.sCaleGene = Random.Range(0,10)<5? dna1.sCaleGene:dna2.sCaleGene;
            childDNA.bodyLengthGene          = Random.Range(0,10)<5 ? dna1.bodyLengthGene : dna2.bodyLengthGene;
            childDNA.wiggleSpeedGene         = Random.Range(0,10)<5 ? dna1.wiggleSpeedGene : dna2.wiggleSpeedGene;
            
            childDNA.smoothSpeedGene         = Random.Range(0,10)<5 ? dna1.smoothSpeedGene : dna2.smoothSpeedGene;
            childDNA.targetDistGene          = Random.Range(0,10)<5 ? dna1.targetDistGene : dna2.targetDistGene;
            childDNA.chageDirectionTimeGene = Random.Range(0,10)<5 ? dna1.chageDirectionTimeGene : dna2.chageDirectionTimeGene;
            

        }
        else
        {
            Mutation(childDNA);
        }
        
        child.SetEnemyFenotype();
        return child;
    }

    private void Mutation(EnemyDNA dna)
    {
        dna.r = Random.Range(0.0f,1.0f);
        dna.g = Random.Range(0.0f,1.0f);
        dna.b = Random.Range(0.0f,1.0f);
        dna.healthGene = Random.Range(25,50);
        dna.damageGene= Random.Range(0,20);
        dna.sCaleGene = Random.Range(0.09f,0.17f);
         dna.bodyLengthGene          = Random.Range(3, 10);
        dna.wiggleSpeedGene         = Random.Range(1f, 5f);
        
        dna.smoothSpeedGene         = Random.Range(0.05f, 0.3f);
        dna.targetDistGene          = Random.Range(0.3f, 0.8f);
        dna.chageDirectionTimeGene = Random.Range(0.5f, 5f);
    }


}
