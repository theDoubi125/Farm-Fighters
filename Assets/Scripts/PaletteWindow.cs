using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class PaletteWindow : EditorWindow
{
    private int selectedTileset, controlID;
    private Vector2 scrollPosition;
    private string[] assetGUIDs;
    private Tileset tileset;
    private IntVector2 selectedTileMin, selectedTileMax, hoveredTile;
    private IntVector2 mousePos = new IntVector2(0, 0);
    private static PaletteWindow m_instance;
    public static PaletteWindow instance { get { if (m_instance == null) m_instance = EditorWindow.GetWindow<PaletteWindow>("Palette", false); return m_instance; } }

    [MenuItem("Window/Palette")] 
    static void ShowWindow()
    {
        m_instance = EditorWindow.GetWindow<PaletteWindow>("Palette");
    }

    void OnEnable()
    {
        RefreshTilesetList();
    }

    void OnGUI()
    {
        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        GUIContent[] comboList = new GUIContent[assetGUIDs.Length];
        for (int i = 0; i < assetGUIDs.Length; i++)
        {
            comboList[i] = new GUIContent(Path.GetFileName(AssetDatabase.GUIDToAssetPath(assetGUIDs[i])).Split('.')[0]);
        }
        GUILayout.BeginHorizontal();
        int selectBuffer = EditorGUILayout.Popup(selectedTileset, comboList);
        if(selectBuffer != selectedTileset)
        {
            selectedTileset = selectBuffer;
            Debug.Log(AssetDatabase.GUIDToAssetPath(assetGUIDs[selectBuffer]));
            tileset = AssetDatabase.LoadAssetAtPath<Tileset>(AssetDatabase.GUIDToAssetPath(assetGUIDs[selectBuffer]));
        }
        if (GUILayout.Button("Refresh List"))
            RefreshTilesetList();
        GUILayout.EndHorizontal();

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        if (tileset != null)
        {
            IntVector2 boxSize = tileset.tileCount * (tileset.tileDim + new IntVector2(1, 1));
            GUILayout.Box(new GUIContent(), GUILayout.Width(boxSize.x), GUILayout.Height(boxSize.y));
            Rect rect = GUILayoutUtility.GetLastRect();
            Rect tileRect = new Rect(rect.position, (Vector2)tileset.tileDim);
            EditorGUI.DrawRect(rect, Color.black);
            
            for(int i=0; i<tileset.tileCount.x; i++)
            {
                for (int j = 0; j < tileset.tileCount.y; j++)
                {
                    tileRect.position = rect.position + (Vector2)((tileset.tileDim + new IntVector2(1, 1)) * new IntVector2(i, tileset.tileCount.y - 1 - j));
                    GUI.DrawTextureWithTexCoords(tileRect, tileset.GetTile(i, j).texture, tileset.GetTile(i, j).textureRect);
                }
            }
            Handles.BeginGUI();
            if(selectedTileMin != null && selectedTileMax != null)
                DrawCells(rect.position, selectedTileMin, selectedTileMax, Color.red);
            if(hoveredTile != null && mousePos.x >= 0 && hoveredTile.x < tileset.tileCount.x && mousePos.y >= 0 && hoveredTile.y < tileset.tileCount.y)
                FillCell(rect.position, hoveredTile, new Color(0.3f, 0.3f, 0.3f, 0.3f));
            Handles.EndGUI();
            mousePos = (IntVector2)(Event.current.mousePosition - rect.position);
            
            switch (Event.current.GetTypeForControl(controlID))
            {
                case EventType.Layout:
                    Repaint();
                    break;
                case EventType.Repaint:
                    mousePos = (IntVector2)(Event.current.mousePosition - rect.position);
                    if (hoveredTile == null)
                        hoveredTile = new IntVector2(0, 0);
                    hoveredTile.x = (int)mousePos.x / (tileset.tileDim.x+1);
                    hoveredTile.y = (int)mousePos.y / (tileset.tileDim.y+1);
                    break;
                case EventType.MouseDown:
                    if (mousePos.x >= 0 && hoveredTile.x < tileset.tileCount.x && mousePos.y >= 0 && hoveredTile.y < tileset.tileCount.y)
                    {
                        selectedTileMin = new IntVector2(hoveredTile.x, hoveredTile.y);
                        selectedTileMax = new IntVector2(hoveredTile.x, hoveredTile.y);
                    }
                    //handlePosition = (Event.current.mousePosition - imageRect.position);
                    break;
                case EventType.MouseDrag:
                    if (mousePos.x >= 0 && hoveredTile.x < tileset.tileCount.x && mousePos.y >= 0 && hoveredTile.y < tileset.tileCount.y)
                    {
                        selectedTileMax = new IntVector2(hoveredTile.x, hoveredTile.y);
                    }
                    //handlePosition = (Event.current.mousePosition - imageRect.position);
                    break;
            }
        }
        //GUILayout.Box(new GUIContent(), GUILayout.Width())
        GUILayout.EndScrollView();
    }

    public void RefreshTilesetList()
    {
        assetGUIDs = AssetDatabase.FindAssets("t:Tileset");
    }

    private void FillCell(Vector2 offset, IntVector2 pos, Color col)
    {
        Handles.DrawSolidRectangleWithOutline(new Rect(offset + new Vector2(pos.x * (tileset.tileDim.x+1), pos.y * (tileset.tileDim.y+1)), (Vector2)tileset.tileDim), col, new Color(0, 0, 0, 0));
    }

    private void DrawCells(Vector2 offset, IntVector2 A, IntVector2 B, Color col)
    {
        IntVector2 minPos = new IntVector2(Mathf.Min(A.x, B.x), Mathf.Min(A.y, B.y));
        IntVector2 maxPos = new IntVector2(Mathf.Max(A.x, B.x), Mathf.Max(A.y, B.y));
        Handles.DrawSolidRectangleWithOutline(new Rect(offset + new Vector2(minPos.x * (tileset.tileDim.x+1), minPos.y * (tileset.tileDim.y+1)), (Vector2)((tileset.tileDim + new IntVector2(1, 1)) * (maxPos - minPos + new IntVector2(1, 1)))), new Color(0, 0, 0, 0), col);
    }

    public Tile GetSelectedTile(IntVector2 pos)
    {
        IntVector2 selectionDim = selectedTileMax - selectedTileMin + new IntVector2(1, 1);
        while (pos.x < 0)
            pos.x += selectionDim.x;
        while (pos.y < 0)
            pos.y += selectionDim.y;
        if (tileset == null)
            return null;
        return tileset.GetTile(selectedTileMin + new IntVector2(pos.x % selectionDim.x, pos.y % selectionDim.y));
    }
}
