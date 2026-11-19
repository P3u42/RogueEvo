using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private int currentLevel =0;
    [SerializeField]private int health = 100;
    [SerializeField]private UIDocument gameUi;
    private Label healthlabel;
    private VisualElement gameOverScreen;
    private Label gameOverMessage;
    
    public static GameManager Instace {get;private set;}
    [SerializeField]private BoardManager boardManager;
    [SerializeField]private  PlayerController player;
    public  TurnManager turnManager{get;private set;}
    [SerializeField]EnemyPopulationManager populationManager;
    // Start is called before the first frame update
 
    private void Awake()
    {
        if(Instace != null)
        {
            Destroy(gameObject);
            return;
        }
        Instace = this;
    }
    void Start()
    {
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartGame()
    {
        SetGameUi();
        turnManager = new TurnManager();
        turnManager.OnTickTac+=OnTurnHappen;
        //boardManager.Init();
        //player.SpawnCharacter(boardManager,new Vector2Int(1,1));
        NewLevel();
    }
    public void OnTurnHappen()
    {
        //ChangeHealth(-1);
        Debug.Log("Loosing health for the weather, drink some vodka!!: "+ health);
        healthlabel.text = "Health :"+health;
    }


    private void SetGameUi()
    {
        gameOverScreen= gameUi.rootVisualElement.Q<VisualElement>("GameOverScreen");
        gameOverMessage= gameUi.rootVisualElement.Q<Label>("GameOverMessage");
        gameOverScreen.style.visibility= Visibility.Hidden;
        healthlabel = gameUi.rootVisualElement.Q<Label>("HealthLabel");
        healthlabel.text ="Health: "+ health;
    }

    public void ChangeHealth(int amount)
    {
        health+= amount;
        if (health >= 100)
        {
            health =100;
        }
        else if (health <= 0)
        {
            health = 0;
            GameOver();
        }
        healthlabel.text  = "Health: "+ health;
    }


    public PlayerController GetPlayer()
    {
        return player;
    }
    public BoardManager GetBoardManager()
    {
        return this.boardManager;
    }

    public void NewLevel()
    {
        boardManager.CleanLevel();
        boardManager.Init();
        player.SpawnCharacter(boardManager,new Vector2Int(1,1));
        health = 100;
        healthlabel.text = "Health: " + health;
        currentLevel++;
    }

    private void GameOver()
    {
        player.GameOver();
         gameOverScreen.style.visibility= Visibility.Visible;
         gameOverMessage.text ="Game over!\n\n You travled throght "+ currentLevel + " levels";
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }


 
}
