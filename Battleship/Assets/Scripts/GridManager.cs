using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/**
 * Grid events are handled differently if they are the player’s grid or the enemy’s
 */
public abstract class GridManager : MonoBehaviour
{
    [SerializeField] protected PlayerManager _playerManager;
    [SerializeField] protected int _width;
    [SerializeField] protected int _height;
    public int[] gridOrigin;
    public Tile tilePrefab;
    public Dictionary<Vector2, Tile> gridDictionnary;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        gridDictionnary = new Dictionary<Vector2, Tile>();
    }

    void Start()
    {
        GenerateGrid(gridOrigin[0], gridOrigin[1]);
    }

    void GenerateGrid(int originX, int originY)
    {
        for (int x=originX; x < originX+_width; x++)
        {
            for (int y=originY; y < originY+_height; y++)
            {
                var spawnedTile = Instantiate(tilePrefab, new Vector3(x, y), quaternion.identity);
                
                spawnedTile.InitTile(x-originX,y-originY,this);
                spawnedTile.InitColor(IsOffset(x,y));
                spawnedTile.transform.SetParent(transform);
                gridDictionnary.Add(new Vector2(x-originX,y-originY), spawnedTile);
            }
        }
    }

    /**
     * Offset the tile colors to differentiate them
     */
    private bool IsOffset(int x, int y)
    {
        return (x + y) % 2 == 1;
    }

    /**
     * Remove all tile highlights to prevent keeping them between turns
     */
    public void RemoveTileHighlights()
    {
        foreach (var tile in gridDictionnary.Values)
        {
            tile.RemoveHighlight();
        }
    }

    public abstract void HandleHoverEvent(Tile tile);
    public abstract void HandleClickEvent(Tile tile);
}
