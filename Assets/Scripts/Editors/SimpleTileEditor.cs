using UnityEngine;
using System.Collections;

using UnityEditor;

[CustomEditor(typeof(SimpleTile))]
public class SimpleTileEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SimpleTile tile = (SimpleTile)target;
        tile.texture = (Texture2D)EditorGUILayout.ObjectField("Texture", tile.texture, typeof(Texture2D), true);
        tile.pos = (IntVector2)EditorGUILayout.Vector2Field("Pos", (Vector2)tile.pos);
        tile.dim = (IntVector2)EditorGUILayout.Vector2Field("Dim", (Vector2)tile.dim);
        Debug.Log(tile.textureRect);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Preview : ");
        GUILayout.Box(new GUIContent(), GUILayout.Width(100), GUILayout.Height(100));
        GUI.DrawTextureWithTexCoords(GUILayoutUtility.GetLastRect(), tile.texture, tile.textureRect);
        GUILayout.EndHorizontal();
    }
}
