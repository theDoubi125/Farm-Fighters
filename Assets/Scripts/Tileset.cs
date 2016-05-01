using UnityEngine;
using System.Collections;
using System;

using UnityEditor;

[Serializable]
public class Tileset : UnityEngine.ScriptableObject
{
    [SerializeField]
    private Texture2D m_texture;
    public Texture2D texture { get { return m_texture; } set { if (m_texture != value) { m_texture = value; ResetTiles(); } } }

    [SerializeField]
    private IntVector2 m_tileDim = new IntVector2(16, 16);

    private Tile[] m_tiles;

    [SerializeField]
    private IntVector2 m_tileCount = new IntVector2(0, 0);

    public virtual Tile[] tiles { get { if (m_tiles == null) ResetTiles(); return m_tiles; } }

    public IntVector2 tileDim { get { return m_tileDim; } set { if (value != m_tileDim) { m_tileDim = value; UpdateTiles(); } } }
    public IntVector2 tileCount
    {
        get
        {
            return m_tileCount;
        }
    }

    public void OnEnable()
    {

    }

    public void ResetTiles()
    {
        if (texture == null || m_tileDim.x == 0 || m_tileDim.y == 0)
        {
            m_tileCount.x = 0;
            m_tileCount.y = 0;
            if (m_tiles.Length > 0)
                m_tiles = new Tile[0];
            return;
        }
        int w = texture.width / m_tileDim.x;
        int h = texture.height / m_tileDim.y;
        Tile[] result = new Tile[w * h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[i + j * w] = ScriptableObject.CreateInstance<SimpleTile>().Init(texture, new IntVector2(i, j), m_tileDim);
            }
        }

        m_tileCount.x = w;
        m_tileCount.y = h;
        m_tiles = result;
        EditorUtility.SetDirty(this);
    }

    public void UpdateTiles()
    {
        if (texture == null || m_tileDim.x == 0 || m_tileDim.y == 0)
        {
            m_tileCount.x = 0;
            m_tileCount.y = 0;
            if (m_tiles.Length > 0)
                m_tiles = new Tile[0];
            return;
        }
        int w = texture.width / m_tileDim.x;
        int h = texture.height / m_tileDim.y;
        Tile[] result = new Tile[w * h];
        for(int i=0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (i >= m_tileCount.x || j >= m_tileCount.y || m_tiles[i + j * m_tileCount.x] == null)
                    result[i + j * w] = ScriptableObject.CreateInstance<SimpleTile>().Init(texture, new IntVector2(i, j), m_tileDim);
                else
                {
                    result[i + j * w] = ((SimpleTile)m_tiles[i + j * m_tileCount.x]).Init(texture, new IntVector2(i, j), m_tileDim);
                }
            }
        }

        m_tileCount.x = w;
        m_tileCount.y = h;
        m_tiles = result;
        EditorUtility.SetDirty(this);
    }

    public Tile GetTile(IntVector2 pos)
    {
        if (m_tiles == null)
            ResetTiles();
        return m_tiles[pos.x + (m_tileCount.y - 1 - pos.y) * m_tileCount.x];
    }

    public Tile GetTile(int x, int y)
    {
        if (m_tiles == null)
            ResetTiles();
        return m_tiles[x + (m_tileCount.y - 1 - y) * m_tileCount.x];
    }
}
