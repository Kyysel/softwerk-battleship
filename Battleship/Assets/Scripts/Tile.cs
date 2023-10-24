using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private Color _baseColor, _oddColor, _hitColor, _missColor;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private GridManager _attachedGrid;

    public Vector2 position;
    // If a ship is on the tile
    public Ship ship;
    // If the tile has been hit
    public bool hit;
    
    private void OnMouseEnter()
    {
        _highlight.SetActive(true);
        _attachedGrid.HandleHoverEvent(this);
    }

    private void OnMouseExit()
    {
        _highlight.SetActive(false);
    }

    private void OnMouseDown()
    {
        _attachedGrid.HandleClickEvent(this);
    }

    public void InitColor(bool isOffset)
    {
        _spriteRenderer.color = isOffset ? _baseColor : _oddColor;
    }

    public void UpdateShipHitColor()
    {
        _spriteRenderer.color = _hitColor;
    }

    public void UpdateMissHitColor()
    {
        _spriteRenderer.color = _missColor;
    }

    public void InitTile(int x, int y, GridManager grid)
    {
        hit = false;
        ship = null;
        _attachedGrid = grid;
        name = $"tile {x},{y}";
        position = new Vector2(x, y);
    }

    public void RemoveHighlight()
    {
        _highlight.SetActive(false);
    }
}
