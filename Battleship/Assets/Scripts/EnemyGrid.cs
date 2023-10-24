using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrid : GridManager
{
    public override void HandleClickEvent(Tile tile)
    {
        if (!_playerManager.hasPlayed)
        {
            Ship hitShip = HitShip(_playerManager.otherPlayer.ownGrid.gridDictionnary[tile.position]);
            if (hitShip != null)
            {
                GameManager.Instance.statusMessage.text = "The " + hitShip.shipName + " has been hit!";
                if (hitShip.health <= 0)
                {
                    GameManager.Instance.statusMessage.text = "The " + hitShip.shipName + " has sunk!";
                    _playerManager.shipsSunk[hitShip.shipName] = true;
                }
                tile.UpdateShipHitColor();
                _playerManager.otherPlayer.UpdateHitOwnGrid(tile.position);
            }
            else
            {
                tile.UpdateMissHitColor();
                _playerManager.otherPlayer.UpdateMissOwnGrid(tile.position);
            }
            
            _playerManager.hasPlayed = true;
            _playerManager.CheckEndFight();
        }
    }

    public override void HandleHoverEvent(Tile tile)
    {
        
    }
    
    /**
    * Return ship type that was on the cell, or null if tile is empty
    */
    private Ship HitShip(Tile tile)
    {
        if (tile.hit)
        {
            return null;
        }
        
        if (tile.ship != null)
        {
            tile.ship.health--;
            tile.hit = true;
            return tile.ship;
        }

        return null;
    }
}
