using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager _board;
    
    private Vector2Int _cellPosition;
    private bool hasMoved;
    private bool gameOver =  false;
    [SerializeField]private int playerDamage = 40;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
    }
    public void SpawnCharacter(BoardManager boardManager,Vector2Int cell){
        _board =boardManager;
        _cellPosition =cell;
        //transform.position = _board.CellToWorld(cell);
        MoveTo(cell);
    }


    private void MoveCharacter()
    {
        if (gameOver)
        {
            return;
        }
        Vector2Int newCellTarget =  _cellPosition;
        hasMoved =  false;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            newCellTarget.y+=1;
            hasMoved=true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            newCellTarget.y-=1;
            hasMoved =true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            newCellTarget.x+=1;
            hasMoved =true;
        }
         else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            newCellTarget.x-=1;
            hasMoved =true;
        }
        if (hasMoved)
        {
            BoardManager.CellData cellData = _board.GetCellData(newCellTarget);
            if (cellData!= null && cellData.isPassable )
            {
                GameManager.Instace.turnManager.TickTac();
                MoveTo(newCellTarget);
                if (cellData.ContaindObject == null)
                {
                    //cellData.ContaindObject.PlayerEntered();
                    MoveTo(newCellTarget);
                } 
                else if (cellData.ContaindObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                    cellData.ContaindObject.PlayerEntered();
                    cellData.isPassable =false;
                }



                
            }
               
        }



        
        
    }

    public void MoveTo(Vector2Int cell)
    {
        _cellPosition = cell;
        transform.position=_board.CellToWorld(_cellPosition);
        
    }

    public Vector2Int GetPlayerPos()
    {
        return _cellPosition;
    }

    public bool GetGameOverState()
    {
        return gameOver;
    }

  
    public void  GameOver()
    {
        gameOver =  true;
    }
    public int GetPlayerDamage()
    {
        return this.playerDamage;
    }


}
