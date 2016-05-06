using UnityEngine;
using System.Collections;
using UnityEditor;

public class GroundTileset : Tileset
{
    public override void ResetTiles()
    {
        if (texture == null || m_tileDim.x == 0 || m_tileDim.y == 0)
        {
            m_tileCount.x = 0;
            m_tileCount.y = 0;
            if (m_tiles.Length > 0)
                m_tiles = new Tile[0];
            return;
        }
        int w = texture.width / m_tileDim.x / 3;
        int h = texture.height / m_tileDim.y / 6;
        Tile[] result = new Tile[w * h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                result[i + j * w] = ScriptableObject.CreateInstance<GroundTile>().Init(texture, new IntVector2(i*3, j*6), m_tileDim);
            }
        }

        m_tileCount.x = w;
        m_tileCount.y = h;
        m_tiles = result;
        EditorUtility.SetDirty(this);
    }

    public override void UpdateTiles()
    {
        if (texture == null || m_tileDim.x == 0 || m_tileDim.y == 0)
        {
            m_tileCount.x = 0;
            m_tileCount.y = 0;
            if (m_tiles.Length > 0)
                m_tiles = new Tile[0];
            return;
        }
        int w = texture.width / m_tileDim.x / 3;
        int h = texture.height / m_tileDim.y / 6;
        Tile[] result = new Tile[w * h];
        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                if (i >= m_tileCount.x || j >= m_tileCount.y || m_tiles[i + j * m_tileCount.x] == null)
                    result[i + j * w] = ScriptableObject.CreateInstance<GroundTile>().Init(texture, new IntVector2(i*3, j*6), m_tileDim);
                else
                {
                    result[i + j * w] = ((SimpleTile)m_tiles[i + j * m_tileCount.x]).Init(texture, new IntVector2(i*3, j*6), m_tileDim);
                }
            }
        }

        m_tileCount.x = w;
        m_tileCount.y = h;
        m_tiles = result;
        EditorUtility.SetDirty(this);
    }
}
