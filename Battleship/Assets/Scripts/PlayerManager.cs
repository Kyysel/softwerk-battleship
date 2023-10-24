using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject OwnGrid, EnemyGrid, PlaceShipsUI;
    [HideInInspector] public OwnGrid ownGrid;
    [HideInInspector] public EnemyGrid enemyGrid;

    public Ship placingShip;
    public Dictionary<string, bool> shipsPlaced;
    public Dictionary<string, bool> shipsSunk;
    public bool hasPlayed;

    public string playerName;
    public PlayerManager otherPlayer;

    // Start is called before the first frame update
    void Start()
    {
        shipsPlaced = new Dictionary<string, bool>();
        shipsPlaced.Add("Carrier", false);
        shipsPlaced.Add("Battleship", false);
        shipsPlaced.Add("Submarine", false);
        shipsPlaced.Add("Destroyer", false);
        shipsPlaced.Add("Patrol Boat", false);

        shipsSunk = new Dictionary<string, bool>();
        shipsSunk.Add("Carrier", false);
        shipsSunk.Add("Battleship", false);
        shipsSunk.Add("Submarine", false);
        shipsSunk.Add("Destroyer", false);
        shipsSunk.Add("Patrol Boat", false);
        hasPlayed = false;
        
        enemyGrid = EnemyGrid.GetComponent<EnemyGrid>();
        ownGrid = OwnGrid.GetComponent<OwnGrid>();
    }

    public void PrepareSetup()
    {
        OwnGrid.SetActive(true);
        EnemyGrid.SetActive(false);
        PlaceShipsUI.SetActive(true);
    }

    public void PrepareFight()
    {
        OwnGrid.SetActive(true);
        EnemyGrid.SetActive(true);
        PlaceShipsUI.SetActive(false);
    }

    public void PlaceShip(Ship ship)
    {
        if (!shipsPlaced[ship.shipName])
        {
            // If a ship was being placed but not confirmed
            if (placingShip != null)
            {
                Destroy(placingShip.gameObject);
            }
            
            // Instantiated out of vision
            placingShip = Instantiate(ship, new Vector3(-100, -100, 0), Quaternion.identity);
            placingShip.transform.parent = transform;
        }
    }

    /**
     * Check if all ships have been placed
     */
    public void CheckEndPlacement()
    {
        foreach (var ship in shipsPlaced.Values)
        {
            // go next turn only if all ships are placed
            if (!ship)
            {
                return;
            }
        }
        StartCoroutine(GameManager.Instance.NextTurn(otherPlayer));
    }

    public void CheckEndFight()
    {
        foreach (var ship in shipsSunk.Values)
        {
            // end game only if all ships are sunk
            if (!ship)
            {
                StartCoroutine(GameManager.Instance.NextTurn(otherPlayer));
                return;
            }
        }
        GameManager.Instance.EndGame(playerName);
    }

    public void UpdateHitOwnGrid(Vector2 position)
    {
        ownGrid.UpdateShipHitEvent(position);
    }
    public void UpdateMissOwnGrid(Vector2 position)
    {
        ownGrid.UpdateMissHitEvent(position);
    }

    public void RemoveTileHighlights()
    {
        ownGrid.RemoveTileHighlights();
        enemyGrid.RemoveTileHighlights();
    }
}
