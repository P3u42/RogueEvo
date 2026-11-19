using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoardManager : MonoBehaviour
{
    private List<Vector2Int> emptyCellsList;
    
    [SerializeField]private ObstacleObject obstacle;
    [SerializeField]private int potionTypesAmount = 5;
    [SerializeField]private GameObject[]potionTypes;
    [SerializeField]private int potionMaxAmount;
    [SerializeField]private int potionMinAmount;
    [SerializeField]private Enemy enemy;
    public  ExitCellObject exitTile;
    [SerializeField] EnemyPopulationManager populationManager;
    [SerializeField] private int potionAmount;
    public class CellData{
        public bool isPassable;

        public CellObject ContaindObject;
    }
    [SerializeField]private PlayerController player;
    [SerializeField] private CellObject potionPrefab;

    private Grid _grid;

    private Tilemap tileMap;
    [SerializeField] private int boardWidth;
    [SerializeField]private int boardHeight;
    [SerializeField]Tile[] groundTiles;
    [SerializeField]private Tile[]wallTiles;
    private CellData[,] boardData;



    // Start is called before the first frame update
    public void Init()
    { 
        _grid =  GetComponentInChildren<Grid>();
        SetTiles();
       
        //GenerateObstacles();
        GeneratePotion();

        if (GameManager.Instace.GetCurrentLevel() <= 0)
        {
            SpawnEnemys();
        }
        else
        {
            populationManager.GenerateNewPopulation();
        }
        
        //player.SpawnCharacter(this,new Vector2Int(1,1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetTiles(){
        tileMap =  GetComponentInChildren<Tilemap>();

        
        emptyCellsList =  new List<Vector2Int>();
        boardData = new CellData[boardWidth,boardHeight];

        for(int y=0;y<boardHeight;++y){
            for(int x=0;x<boardWidth;++x){


                Tile tile;
                boardData[x,y]= new CellData();
                if(x==0||y==0||x==boardWidth-1||y==boardHeight-1){


                    tile= wallTiles[Random.Range(0,wallTiles.Length)];
                    boardData[x,y].isPassable=false;

                }
                else
                {
                    tile =  groundTiles[Random.Range(0,groundTiles.Length)];
                    boardData[x,y].isPassable =true;
                    emptyCellsList.Add(new Vector2Int(x,y));
                }
                //int tileNumb = Random.Range(0,groundTiles.Length);
                tileMap.SetTile(new Vector3Int(x,y,0),tile);
            }
        }
        emptyCellsList.Remove(new Vector2Int(1,1));
        Vector2Int endCoord =  new Vector2Int(boardWidth-2,boardHeight-2);
        AddObject(Instantiate(exitTile),endCoord);
        emptyCellsList.Remove(endCoord);
        
    }


    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return _grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }

    public CellData GetCellData(Vector2Int cellIndex)
    {
        if(cellIndex.x<0||cellIndex.x>=boardWidth
        || cellIndex.y < 0 || cellIndex.y >= boardHeight)
        {
            return null;
        }
        return boardData[cellIndex.x,cellIndex.y];
    }

       private void GeneratePotion()
    {
         potionAmount = Random.Range(potionMinAmount,potionMaxAmount);
        for(int i = 0; i < potionAmount; i++)
        {
            //int randomX =  Random.Range(0,);
            //int randomY =  Random.Range(1,boardHeight-1);
            int randomIndex =  Random.Range(0,emptyCellsList.Count);
            Vector2Int coord =  emptyCellsList[randomIndex];
            emptyCellsList.RemoveAt(randomIndex);
            CellData data = boardData[coord.x,coord.y];
            /*if(data.isPassable && data.ContaindObject == null)
            {
                
            }
            */

                PotionObject newPotion =  Instantiate((PotionObject)potionPrefab);
                newPotion.transform.position=CellToWorld(coord);
                data.ContaindObject = newPotion;
        }
    }

    private void SpawnEnemys()
    {
        populationManager.GenerateEnemyPopulation();
        
        /*
        var index =  Random.Range(0,emptyCellsList.Count);
        Vector2Int spawnCoord =  emptyCellsList[index];
        emptyCellsList.RemoveAt(index);
        CellData spawnData = boardData[spawnCoord.x,spawnCoord.y];
        Enemy newEnemy =  Instantiate(enemy);
        newEnemy.Init(spawnCoord);
        newEnemy.transform.position=CellToWorld(spawnCoord);
        spawnData.ContaindObject =  newEnemy;
        spawnData.isPassable =false;
        */



    }

    private void GenerateObstacles()
    {
        int obstacleAmount =  Random.Range(6,10);


        for(int i = 0; i < obstacleAmount; i++)
        {
            var randomIndex =  Random.Range(0,emptyCellsList.Count);
            Vector2Int coord =  emptyCellsList[randomIndex];
            emptyCellsList.RemoveAt(randomIndex);
            //CellData data =  boardData[coord.x,coord.y];
            ObstacleObject newObstacle =  Instantiate(obstacle);
            //newObstacle.Init(coord);
            //newObstacle.transform.position =CellToWorld(coord);
            //data.ContaindObject=newObstacle;
            AddObject(newObstacle,coord);
        
        }
    }
    public void SetCellTile(Vector2Int cellIndex,Tile tile)
    {
        tileMap.SetTile(new Vector3Int(cellIndex.x,cellIndex.y,0),tile);
    }

    public void AddObject(CellObject obj,Vector2Int coord)
    {
        CellData data =  boardData[coord.x,coord.y];
        obj.transform.position =CellToWorld(coord);
        data.ContaindObject = obj;
        obj.Init(coord);
    }
    public Tile GetCellTile(Vector2Int cellIndex)
    {
        return tileMap.GetTile<Tile>(new Vector3Int(cellIndex.x,cellIndex.y,0));
    }



    public void CleanLevel()
    {
        if (boardData == null)
        {
            return;
        }

        for(int y = 0; y < boardHeight; y++)
        {
            for(int x = 0; x < boardWidth; x++)
            {
                var cellData =  boardData[x,y];
                if (cellData.ContaindObject != null)
                {
                    Destroy(cellData.ContaindObject.gameObject);
                }
                SetCellTile(new Vector2Int(x,y),null);
            }
        }
    }
    public List<Vector2Int> GetEmptyCellList()
    {
        return emptyCellsList;
    }



   
}
