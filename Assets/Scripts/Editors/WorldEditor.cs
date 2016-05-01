using UnityEngine;
using System.Collections;
using System.IO;

using UnityEditor;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor
{
    IntVector2 pos = new IntVector2(0, 0);
    public override void OnInspectorGUI()
    {
        World world = (World)target;
        world.dim = (IntVector2)EditorGUILayout.Vector2Field("Dim", (Vector2)world.dim);
        pos = (IntVector2)EditorGUILayout.Vector2Field("Pos", (Vector2)pos);
        if(GUILayout.Button("Add"))
        {
            world.SetTile(pos, PaletteWindow.instance.GetSelectedTile());
        }
    }
}
