using UnityEngine;
using System.Collections;
using System;

public class GroundTile : Tile
{
    private Transform tilePrefab;
    private Sprite[] sprites;

    [SerializeField]
    private IntVector2 m_pos, m_dim;

    [SerializeField]
    private Texture2D m_texture;

    public GroundTile Init(Texture2D texture, IntVector2 pos, IntVector2 dim)
    {
        m_pos = pos;
        m_dim = dim;
        this.texture = texture;
        return this;
    }

    public void UpdateSprites()
    {
        sprites = new Sprite[18];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                IntVector2 tilePos = new IntVector2(pos.x + i, pos.y + j);
                sprites[i + j*3] = Sprite.Create(texture, new Rect(new Vector2(tilePos.x * dim.x, tilePos.y * dim.y + texture.height % dim.y), (Vector2)dim), new Vector2(0.5f, 0.5f));
            }
        }
    }

    public override Sprite GetSprite(bool[] neighbours)
    {
        // Count diag and direct neighbours
        int diagCount = 0, dirCount = 0;
        for(int i=0; i<4; i++)
        {
            if (neighbours[2 * i]) dirCount++;
            if (neighbours[2 * i + 1]) diagCount++;
        }
        // Corner tile
        if(dirCount == 2)
        {
            int spriteX = 0, spriteY = 0;
            if (neighbours[2]) spriteY = 2;
            if(neighbours[4]) spriteX = 2;

            return GetSprite(spriteX, spriteY);
        }
        // Border tile
        if(dirCount == 3)
        {
            if (!neighbours[0]) return GetSprite(2, 1);
            if (!neighbours[2]) return GetSprite(1, 0);
            if (!neighbours[4]) return GetSprite(0, 1);
            if (!neighbours[6]) return GetSprite(1, 2);
        }
        // Inside corner tile
        if (diagCount == 3)
        {
            if (!neighbours[1]) return GetSprite(0, 5);
            if (!neighbours[3]) return GetSprite(2, 5);
            if (!neighbours[5]) return GetSprite(2, 3);
            if (!neighbours[7]) return GetSprite(0, 3);
        }
        return GetSprite(1, 1);
    }

    private Sprite GetSprite(int x, int y)
    {
        return sprites[x + (5-y) * 3];
    }

    public override IntVector2 pos
    {
        get
        {
            return m_pos;
        }

        set
        {
            if(m_pos != pos)
            {
                m_pos = pos;
                UpdateSprites();
            }
        }
    }
    public override IntVector2 dim
    {
        get
        {
            return m_dim;
        }
        set
        {
            if(m_dim != dim)
            {
                m_dim = dim;
                UpdateSprites();
            }
        }
    }

    public override Texture2D texture
    {
        get
        {
            return m_texture;
        }
        set
        {
            if (m_texture != value)
            {
                m_texture = value;
                UpdateSprites();
            }
        }
    }

    public override Rect textureRect
    {
        get
        {
            if (texture == null)
                return new Rect(0, 0, 0, 0);
            IntVector2 tilesCount = new IntVector2(texture.width / dim.x, texture.height / dim.y);
            IntVector2 tilesetDim = tilesCount * dim;
            Vector2 UVTileDim = new Vector2((float)dim.x / texture.width, (float)dim.y / texture.height);
            Vector2 UVTilePos = new Vector2((float)pos.x * dim.x / texture.width, (float)(texture.height - (pos.y + 1) * dim.y) / texture.height);

            return new Rect(UVTilePos, UVTileDim);
        }
    }

    public override Transform prefab
    {
        get
        {
            if (tilePrefab == null)
                tilePrefab = Resources.Load<GameObject>("Tile Prefabs/SimpleTile").transform;
            return tilePrefab;
        }
    }
}
