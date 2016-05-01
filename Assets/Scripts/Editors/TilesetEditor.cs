using UnityEngine;
using System.Collections;
using System.IO;

using UnityEditor;

[CustomEditor(typeof(Tileset))]
public class TilesetEditor : Editor
{
    private Vector2 scrollPosition;
    public override void OnInspectorGUI()
    {
        Tileset tileset = (Tileset)target;
        tileset.texture = (Texture2D)EditorGUILayout.ObjectField("Texture", tileset.texture, typeof(Texture2D), true);
        tileset.tileDim = (IntVector2)EditorGUILayout.Vector2Field("Tile Dimension", (Vector2)tileset.tileDim);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        for (int j = 0; j < tileset.tileCount.y; j++)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < tileset.tileCount.x; i++)
            {
                SimpleTile tile = (SimpleTile)tileset.tiles[i + j * tileset.tileCount.x];
                GUILayout.Box(new GUIContent(), GUILayout.Width(tileset.tileDim.x * 2), GUILayout.Height(tileset.tileDim.y * 2));
                GUI.DrawTextureWithTexCoords(GUILayoutUtility.GetLastRect(), tile.texture, tile.textureRect);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        if (GUI.changed)
        {
            AssetDatabase.SaveAssets();
        }
    }

    [MenuItem("Assets/Create/Tileset")]
    public static void CreateTileset()
    {
        Tileset asset = ScriptableObject.CreateInstance<Tileset>();
        ProjectWindowUtil.CreateAsset(asset, "New Tileset.asset");
    }
}
