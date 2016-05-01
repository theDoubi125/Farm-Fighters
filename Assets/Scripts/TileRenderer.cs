using UnityEngine;
using System.Collections;

public class TileRenderer : MonoBehaviour
{
    public Tile tile;
    private SpriteRenderer spriteRenderer;

	void OnEnable ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (tile != null)
            spriteRenderer.sprite = tile.GetSprite();
    }

    public void Init(Tile tile)
    {
        this.tile = tile;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = tile.GetSprite();
    }
	
	void Update ()
    {
	    
	}
}
