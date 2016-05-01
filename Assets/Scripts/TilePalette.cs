using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TilePalette : ScriptableObject
{

    [SerializeField]
    private List<Tileset> m_tilesets;

    public List<Tileset> tilesets { get { return m_tilesets; } }

    public void AddTileset(Tileset tileset)
    {
        tilesets.Add(tileset);
    }

    public void RemoveTileset(Tileset tileset)
    {
        tilesets.Remove(tileset);
    }
}
