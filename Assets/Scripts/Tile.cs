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

    public static bool operator ==(Tile A, Tile B)
    {
        if (System.Object.ReferenceEquals(A, B))
            return true;

        if (((object)A == null) || ((object)B == null))
            return false;
        return A.texture == B.texture && A.pos == B.pos && A.dim == B.dim && A.GetType() == B.GetType();
    }

    public static bool operator !=(Tile A, Tile B)
    {
        return !(A == B);
    }
}
