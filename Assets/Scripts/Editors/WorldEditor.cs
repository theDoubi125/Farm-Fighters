using UnityEngine;
using System.Collections;
using System.IO;

using UnityEditor;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor
{
    IntVector2 pos = new IntVector2(0, 0);
    IntVector2 paintStartPos = null;
    public override void OnInspectorGUI()
    {
        World world = (World)target;
        world.dim = (IntVector2)EditorGUILayout.Vector2Field("Dim", (Vector2)world.dim);
        world.tileDim = (IntVector2)EditorGUILayout.Vector2Field("Tile Dim", (Vector2)world.tileDim);
        pos = (IntVector2)EditorGUILayout.Vector2Field("Pos", (Vector2)pos);
        if(GUILayout.Button("Add"))
        {
            world.SetTile(pos, PaletteWindow.instance.GetSelectedTile(new IntVector2(0, 0)));
        }
    }

    public void OnSceneGUI()
    {
        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        World world = (World)target;
        if (Event.current.type == EventType.MouseMove)
        {
            Vector2 mousePos = Camera.current.ScreenToWorldPoint(new Vector2(Event.current.mousePosition.x, Camera.current.pixelHeight - Event.current.mousePosition.y));
            world.hoveredCell = world.GetTileAt(mousePos);
            EditorUtility.SetDirty(target);
            Event.current.Use();
        }
        if(Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag)
        {
            Vector2 mousePos = Camera.current.ScreenToWorldPoint(new Vector2(Event.current.mousePosition.x, Camera.current.pixelHeight - Event.current.mousePosition.y));
            IntVector2 cell = world.GetTileAt(mousePos);
            if(Event.current.button == 0)
            {
                if (Event.current.type == EventType.mouseDown)
                    paintStartPos = cell;
                world.SetTile(cell, PaletteWindow.instance.GetSelectedTile(cell-paintStartPos));
            }
            else
                world.SetTile(cell, null);
            Event.current.Use();
        }
        //if(Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
        //    Debug.Log(Event.current.type);
    }
}

