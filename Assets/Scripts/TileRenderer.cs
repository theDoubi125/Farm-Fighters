﻿using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class TileRenderer : MonoBehaviour
{
    public Tile tile;
    private SpriteRenderer spriteRenderer;
    public bool[] neighbours = new bool[8];

	void OnEnable ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (tile != null)
            spriteRenderer.sprite = tile.GetSprite(neighbours);
    }

    public void Init(Tile tile)
    {
        this.tile = tile;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = tile.GetSprite(neighbours);
    }

    public void UpdateNeighbour(int dir, Tile tile)
    {
        if (tile == null)
            neighbours[dir] = false;
        else neighbours[dir] = tile == this.tile;
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if(this.tile != null)
            spriteRenderer.sprite = this.tile.GetSprite(neighbours);
    }
}
