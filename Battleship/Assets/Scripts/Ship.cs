using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Ship : MonoBehaviour
{
    public string shipName;
    public int length;

    public int health;

    public float rotation;
    public SpriteRenderer spriteRenderer;
    public Color normalColor;
    public Color alternateColor;
    
    // Start is called before the first frame update
    void Start()
    {
        health = length;
    }
}
