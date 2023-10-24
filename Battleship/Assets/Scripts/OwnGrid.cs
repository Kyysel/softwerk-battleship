using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnGrid : GridManager
{
    // Update is called once per frame
    void Update()
    {
        if (_playerManager.placingShip != null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _playerManager.placingShip.transform.Rotate(new Vector3(0,0,90));
                _playerManager.placingShip.rotation =
                    _playerManager.placingShip.transform.rotation.eulerAngles.z;
            }
        }
    }

    public override void HandleHoverEvent(Tile tile)
    {
        if (_playerManager.placingShip != null)
        {
            _playerManager.placingShip.transform.position = tile.position;
            UpdateShipColor(tile, _playerManager.placingShip);
        }
    }

    private void UpdateShipColor(Tile tile, Ship ship)
    {
        // change the ship color if the player can place it or not
        if (CanPlaceShip(tile, _playerManager.placingShip))
        {
            ship.spriteRenderer.color = ship.normalColor;
        }
        else
        {
            ship.spriteRenderer.color = ship.alternateColor;
        }
    }
    
    public override void HandleClickEvent(Tile tile)
    {
        if (_playerManager.placingShip != null && CanPlaceShip(tile, _playerManager.placingShip))
        {
            _playerManager.shipsPlaced[_playerManager.placingShip.shipName] = true;
            UpdateShipColor(tile, _playerManager.placingShip);
            
            // call the method to prevent having red ships on the grid
            UpdateShipGrid(tile, _playerManager.placingShip);

            _playerManager.placingShip = null;
            _playerManager.CheckEndPlacement();
        }
    }

    /**
     * Update the grid with the newly added ship
     */
    private void UpdateShipGrid(Tile tile, Ship ship)
    {
        switch (ship.rotation)
        {
            // right
            case 0:
                for (int i = 0; i < ship.length; i++)
                {
                    gridDictionnary[new Vector2(i, 0) + (tile.position)].ship = ship;
                }
                break;
            // up
            case 90:
                for (int i = 0; i < ship.length; i++)
                {
                    gridDictionnary[new Vector2(0, i) + (tile.position)].ship = ship;
                }
                break;
            // left
            case 180:
                for (int i = 0; i < ship.length; i++)
                {
                    gridDictionnary[new Vector2(-i, 0) + (tile.position)].ship = ship;
                }
                break;
            // down
            case 270:
                for (int i = 0; i < ship.length; i++)
                {
                    gridDictionnary[new Vector2(0, -i) + (tile.position)].ship = ship;
                }
                break;
        }
    }

    private bool CanPlaceShip(Tile tile, Ship ship)
    {
        switch (ship.rotation)
        {
            // right
            case 0:
                // check coordinates
                if (tile.position.x + ship.length> _width)
                    return false;
                //check if another ship is present on selected tiles
                for (int i = 0; i < ship.length; i++)
                {
                    if (gridDictionnary[new Vector2(i, 0) + (tile.position)].ship != null)
                    {
                        return false;
                    }
                }
                break;
            // up
            case 90:
                if (tile.position.y + ship.length> _height)
                    return false;
                for (int i = 0; i < ship.length; i++)
                {
                    if (gridDictionnary[new Vector2(0, i) + (tile.position)].ship != null){
                        return false;
                    }
                }
                break;
            // left
            case 180:
                if (tile.position.x - (ship.length - 1) < 0)
                    return false;
                for (int i = 0; i < ship.length; i++)
                {
                    if (gridDictionnary[new Vector2(-i, 0) + (tile.position)].ship != null){
                        return false;
                    }
                }
                break;
            // down
            case 270:
                if (tile.position.y - (ship.length - 1) < 0)
                    return false;
                for (int i = 0; i < ship.length; i++)
                {
                    if (gridDictionnary[new Vector2(0, -i) + (tile.position)].ship != null){
                        return false;
                    }
                }
                break;
        }

        return true;
    }

    public void UpdateShipHitEvent(Vector2 position)
    {
        gridDictionnary[position].UpdateShipHitColor();
    }

    public void UpdateMissHitEvent(Vector2 position)
    {
        gridDictionnary[position].UpdateMissHitColor();
    }
}
