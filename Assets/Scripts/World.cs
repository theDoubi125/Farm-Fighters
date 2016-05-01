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

    // Property for m_dim, the set triggers an update of the children if needed
    public IntVector2 dim { get { return m_dim; } set { if (value != m_dim) { UpdateDimensions(value); } } }

    public void SetTile(IntVector2 pos, Tile tile)
    {
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
            instance.transform.localPosition = (Vector2)(new IntVector2(pos.x, -pos.y) * tile.dim);
            instance.name = "Tile " + pos.x + " " + pos.y;
            m_children[pos.x + pos.y * m_dim.x] = tileRenderer;
        }
    }

    public void OnEnable()
    {
        /*m_children = new TileRenderer[dim.x * dim.y];
        for(int i=0; i<dim.x; i++)
        {
            for(int j=0; j<dim.y; j++)
            {
                Tile tile = m_tiles[i + j * dim.x];
                if(tile != null)
                {
                    Transform instance = Instantiate<Transform>(tile.prefab);
                    TileRenderer tileRenderer = instance.GetComponent<TileRenderer>();
                    tileRenderer.Init(tile);
                    instance.transform.position = (Vector2)(new IntVector2(i, j) * dim);
                    m_children[i + j * m_dim.x] = tileRenderer;
                }
            }
        }*/
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
	
	void Update ()
    {

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.2f);
        for (int i = 0; i <= dim.x; i++)
        {
            Gizmos.DrawLine(transform.position + new Vector3(i * m_tiles[0].dim.x, m_tiles[0].dim.y), transform.position + new Vector3(i * m_tiles[0].dim.x, -(dim.y-1) * m_tiles[0].dim.y));
        }

    }

    private Rect Bounds()
    {
        Vector2 topLeftPos = (Vector2)transform.position - (Vector2)(m_tiles[0].dim * m_dim / 2);
        Vector2 rectDim = (Vector2)(m_tiles[0].dim * m_dim);
        return new Rect(topLeftPos, rectDim);
    }
}
