using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class World : MonoBehaviour {

    // the dimension, in tile units, of the world.
    [SerializeField]
    private IntVector2 m_dim;

    // The tiles data (different from the actual GameObjects, contains only data)
    // It is what is serialized and saved (children GameObjects are created during scene load)
    [SerializeField]
    private Tile[] m_tiles;

    // Keep track of children tiles to be able to edit and destroy them
    [SerializeField]
    private TileRenderer[] m_children;

    [SerializeField]
    private IntVector2 m_tileDim;

    // Property for m_dim, the set triggers an update of the children if needed
    public IntVector2 dim { get { return m_dim; } set { if (value != m_dim) { UpdateDimensions(value); } } }
    // Property for m_dim, the set triggers an update of the children if needed
    public IntVector2 tileDim { get { return m_tileDim; } set { if (value != m_tileDim) { UpdateTileDimensions(value); } } }

    // Used only in the editor
    public IntVector2 hoveredCell;

    private static IntVector2[] dirVecs = { new IntVector2(1, 0), new IntVector2(1, -1), new IntVector2(0, -1), new IntVector2(-1, -1), new IntVector2(-1, 0), new IntVector2(-1, 1), new IntVector2(0, 1), new IntVector2(1, 1)};

    public void SetTile(IntVector2 pos, Tile tile)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= dim.x || pos.y >= dim.y)
            return;
        m_tiles[pos.x + pos.y * m_dim.x] = tile;
        if (m_children[pos.x + pos.y * m_dim.x] != null)
        {
            DestroyImmediate(m_children[pos.x + pos.y * m_dim.x].gameObject);
            m_children[pos.x + pos.y * m_dim.x] = null;
        }
        if (tile != null)
        {
            Transform instance = Instantiate<Transform>(tile.prefab);
            TileRenderer tileRenderer = instance.GetComponent<TileRenderer>();
            tileRenderer.Init(tile);
            instance.transform.SetParent(transform);
            instance.transform.position = GetTilePos(pos);
            instance.transform.localScale = 100 * new Vector2((float)tileDim.x / tile.dim.x, (float)tileDim.y / tile.dim.y);
            instance.name = "Tile " + pos.x + " " + pos.y;
            m_children[pos.x + pos.y * m_dim.x] = tileRenderer;
        }
        for(int i=0; i<8; i++)
        {
            IntVector2 neighbourPos = pos + dirVecs[i];
            if (GetChild(neighbourPos) != null)
                GetChild(neighbourPos).UpdateNeighbour((i + 4) % 8, GetTile(pos));
            if (GetChild(pos) != null)
                GetChild(pos).UpdateNeighbour(i, GetTile(neighbourPos));
        }
    }

    public TileRenderer GetChild(IntVector2 pos)
    {
        if (pos.x >= 0 && pos.y >= 0 && pos.x < m_dim.x && pos.y < m_dim.y)
            return m_children[pos.x + pos.y * m_dim.x];
        return null;
    }

    public Tile GetTile(IntVector2 pos)
    {
        if (pos.x >= 0 && pos.y >= 0 && pos.x < m_dim.x && pos.y < m_dim.y)
            return m_tiles[pos.x + pos.y * m_dim.x];
        return null;
    }

    public void OnEnable()
    {
    }

    public void UpdateDimensions(IntVector2 newDim)
    {
        //creation of the new tiles and children arrays
        Tile[] tilesBuffer = new Tile[newDim.x * newDim.y];
        TileRenderer[] childrenBuffer = new TileRenderer[newDim.x * newDim.y];
        for(int i=0; i< newDim.x; i++)
        {
            for(int j=0; j< newDim.y; j++)
            {
                if(i < dim.x && j < dim.y)
                {
                    tilesBuffer[i + j * newDim.x] = m_tiles[i + j * dim.x];
                    childrenBuffer[i + j * newDim.x] = m_children[i + j * dim.x];
                    if(childrenBuffer[i+j * newDim.x] != null)
                    {
                        childrenBuffer[i + j * newDim.x].transform.position = GetTilePos(new IntVector2(i, j), newDim);
                        childrenBuffer[i + j * newDim.x].transform.localScale = 100 * new Vector2((float)tileDim.x / tilesBuffer[i+j*newDim.x].dim.x, (float)tileDim.y / tilesBuffer[i + j * newDim.x].dim.y);
                    }
                }
            }
        }
        // deletion of unused tiles
        for(int i=newDim.x; i<m_dim.x; i++)
        {
            for(int j=newDim.y; j<m_dim.y; j++)
            {
                if(m_children[i + j*m_dim.x] != null)
                    DestroyImmediate(m_children[i + j * m_dim.x].gameObject);
            }
        }
        m_children = childrenBuffer;
        m_tiles = tilesBuffer;
        m_dim = newDim;
    }

    void UpdateTileDimensions(IntVector2 tileDim)
    {
        m_tileDim = tileDim;
    }
	
	void Update ()
    {

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.2f);
        Rect currentBounds = bounds;
        Vector2 tileDim = (Vector2)m_tileDim;
        Vector2 tileX = new Vector2(tileDim.x, 0);
        Vector2 tileY = new Vector2(0, tileDim.y);
        Vector2 topLeftPos = (Vector2)transform.position - new Vector2(tileDim.x * (float)dim.x/2, tileDim.y * (float)dim.y/2);

        for (int i = 0; i <= dim.x; i++)
            Gizmos.DrawLine(topLeftPos - tileDim/ 2 + tileX * i, topLeftPos + tileX * i + tileY * dim.y - tileDim / 2);
        for (int i = 0; i <= dim.y; i++)
            Gizmos.DrawLine(topLeftPos - tileDim / 2 + tileY * i, topLeftPos + tileY * i + tileX * dim.x - tileDim / 2);
        Gizmos.color = new Color(1, 1, 1, 0.3f);
        if(IsInsideBounds(hoveredCell))
            Gizmos.DrawCube(GetTilePos(hoveredCell), new Vector3(tileDim.x, tileDim.y, 1));
    }

    private Rect bounds
    {
        get
        {
            Vector2 topLeftPos = (Vector2)transform.position - new Vector2(tileDim.x * (float)dim.x / 2, tileDim.y * (float)dim.y / 2);
            Vector2 rectDim = (Vector2)(tileDim * dim);
            return new Rect(topLeftPos, rectDim);
        }
    }

    public Vector2 GetTilePos(IntVector2 pos)
    {
        Vector2 tileDim = (Vector2)m_tileDim;
        Vector2 tileX = new Vector2(tileDim.x, 0);
        Vector2 tileY = new Vector2(0, tileDim.y);
        Vector2 topLeftPos = (Vector2)transform.position - new Vector2(tileDim.x * (float)dim.x / 2, -tileDim.y * (float)dim.y / 2);
        return topLeftPos + tileX * pos.x - tileY * (pos.y+1);
    }

    public Vector2 GetTilePos(IntVector2 pos, IntVector2 dim)
    {
        Vector2 tileDim = (Vector2)m_tileDim;
        Vector2 tileX = new Vector2(tileDim.x, 0);
        Vector2 tileY = new Vector2(0, tileDim.y);
        Vector2 topLeftPos = (Vector2)transform.position - new Vector2(tileDim.x * (float)dim.x / 2, -tileDim.y * (float)dim.y / 2);
        return topLeftPos + tileX * pos.x - tileY * (pos.y + 1);
    }

    public IntVector2 GetTileAt(Vector2 pos)
    {
        Vector2 tileDim = (Vector2)m_tileDim;
        Vector2 tileX = new Vector2(tileDim.x, 0);
        Vector2 tileY = new Vector2(0, tileDim.y);
        Vector2 topLeftPos = (Vector2)transform.position - new Vector2(tileDim.x * (float)dim.x / 2, -tileDim.y * (float)dim.y / 2) - tileDim/2;
        Vector2 disp = pos - topLeftPos;
        IntVector2 result = new IntVector2((int)(disp.x / tileDim.x), (int)(-disp.y / tileDim.y));
        if (disp.x < 0)
            result.x--;
        if (disp.y > 0)
            result.y--;
        return result;
    }

    public bool IsInsideBounds(IntVector2 pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < dim.x && pos.y < dim.y;
    }
}
