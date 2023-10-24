using System;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;

/**
 * Handles the game logic
 */
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum gameState
    {
        start,
        setup,
        fight,
        end,
    }

    public gameState currentState;
    public PlayerManager player1;
    public PlayerManager player2;

    public TextMeshProUGUI statusMessage;
    public GameObject visionBlocker;
    public float waitTimeForNextTurn;

    [SerializeField] private GameObject _restartButton;
    
    private void Awake()
    {
        // we only have one game manager
        if (Instance == null)
        {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player1.gameObject.SetActive(true);
        player2.gameObject.SetActive(false);
        
        PrepareSetup();

        statusMessage.text = player1.playerName + " is playing.";
        visionBlocker.SetActive(false);
        _restartButton.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RestartGame();
        }
    }


    /**
     * Called after each turn to handle switching UI elements
     * player : the player that has to play
     */
    public IEnumerator NextTurn(PlayerManager player)
    {
        // We wait set amount of time before going to next player
        yield return new WaitForSeconds(waitTimeForNextTurn);
        player.RemoveTileHighlights();
        
        statusMessage.text = "Player " + player.playerName + " is playing";
        
        visionBlocker.SetActive(true);
        // wait for enter key to be pressed before showing other player’s boards
        StartCoroutine(HandleWaitScreen());
    }

    public void EndGame(string playerName)
    {
        SwitchToNextState();
        statusMessage.text = playerName + " WON!";
    }
    
    private void PrepareSetup()
    {
        player1.PrepareSetup();
        player2.PrepareSetup();
        currentState = gameState.setup;
    }
    
    private void SwitchToNextState()
    {
        switch (currentState)
        {
            case gameState.start:
                currentState = gameState.setup;
                break;
            case gameState.setup:
                currentState = gameState.fight;
                player1.PrepareFight();
                player2.PrepareFight();
                break;
            case gameState.fight:
                currentState = gameState.end;
                _restartButton.SetActive(true);
                break;
            default :
                currentState = gameState.start;
                break;
        }
    }

    public IEnumerator HandleWaitScreen()
    {
        while (!Input.GetKeyDown(KeyCode.Return))
        {
            yield return null;    
        }
        //  if the second player is active at this moment, end of placement for both players
        if (currentState == gameState.setup && player2.gameObject.activeSelf)
        {
            SwitchToNextState();
        }

        // go next turn
        visionBlocker.SetActive(false);
        player1.gameObject.SetActive(!player1.gameObject.activeSelf);
        player2.gameObject.SetActive(!player2.gameObject.activeSelf);
        player1.hasPlayed = false;
        player2.hasPlayed = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
