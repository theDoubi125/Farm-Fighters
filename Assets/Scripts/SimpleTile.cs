using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class SimpleTile : Tile
{
    private Sprite sprite;

    [SerializeField]
    private IntVector2 m_pos, m_dim;

    [SerializeField]
    private Texture2D m_texture;

    public override IntVector2 pos { get { return m_pos; } set { if (m_pos != value) { m_pos = value; UpdateSprite(); } } }
    public override IntVector2 dim { get { return m_dim; } set { if (m_dim != value) { m_dim = value; UpdateSprite(); } } }
    public override Texture2D texture { get { return m_texture; } set { if (m_texture != value) { m_texture = value; UpdateSprite(); } } }

    private static Transform tilePrefab;

    public SimpleTile Init(Texture2D texture, IntVector2 pos, IntVector2 dim)
    {
        m_pos = pos;
        m_dim = dim;
        m_texture = texture;
        UpdateSprite();
        return this;
    }

    public void UpdateSprite()
    {
        sprite = Sprite.Create(texture, new Rect((Vector2)(pos * dim), (Vector2)dim), new Vector2(0, 0));
    }

    public override void Update ()
    {

    }

    public override Sprite GetSprite()
    {
        if (sprite == null && texture != null)
            UpdateSprite();
        return sprite;
    }

    public override void NeighbourChanged(IntVector2 pos)
    {

    }

    public override Rect textureRect
    {
        get
        {
            IntVector2 tilesCount = new IntVector2(texture.width / dim.x, texture.height / dim.y);
            IntVector2 tilesetDim = tilesCount * dim;
            Vector2 UVTileDim = new Vector2((float)dim.x / texture.width, (float)dim.y / texture.height);
            Vector2 UVTilePos = new Vector2((float)pos.x * dim.x / texture.width, (float)(texture.height - (pos.y+1) * dim.y) / texture.height);

            return new Rect(UVTilePos, UVTileDim);
        }
    }

    public override Transform prefab
    {
        get
        {
            if(tilePrefab == null)
                tilePrefab = Resources.Load<GameObject>("Tile Prefabs/SimpleTile").transform;
            return tilePrefab;
        }
    }
}
