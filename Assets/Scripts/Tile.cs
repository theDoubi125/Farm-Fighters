using UnityEngine;
using System.Collections;

[System.Serializable]
public abstract class Tile : ScriptableObject
{
    public abstract Sprite GetSprite(bool[] neighbours);

    public abstract IntVector2 pos { get; set; }
    public abstract IntVector2 dim { get; set; }
    public abstract Texture2D texture { get; set; }
    public abstract Rect textureRect { get; }


    public abstract Transform prefab { get; }
}
