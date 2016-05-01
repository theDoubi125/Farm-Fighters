using UnityEngine;
using System.Collections;

[System.Serializable]
public class IntVector2{

    public int x, y;

    public IntVector2(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static IntVector2 operator +(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x + b.x, a.y + b.y);
    }

    public static IntVector2 operator -(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x - b.x, a.y - b.y);
    }

    public static IntVector2 operator *(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x * b.x, a.y * b.y);
    }

    public static IntVector2 operator /(IntVector2 a, IntVector2 b)
    {
        return new IntVector2(a.x / b.x, a.y / b.y);
    }

    public static IntVector2 operator *(IntVector2 a, float f)
    {
        return new IntVector2((int)(a.x * f), (int)(a.y * f));
    }

    public static IntVector2 operator /(IntVector2 a, float f)
    {
        return new IntVector2((int)(a.x / f), (int)(a.y / f));
    }

    public static explicit operator Vector2(IntVector2 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static explicit operator IntVector2(Vector2 v)
    {
        return new IntVector2((int)v.x, (int)v.y);
    }

    public static bool operator ==(IntVector2 a, IntVector2 b)
    {
        if (System.Object.ReferenceEquals(a, b))
        {
            return true;
        }
        
        if (((object)a == null) || ((object)b == null))
        {
            return false;
        }

        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(IntVector2 a, IntVector2 b)
    {
        return !(a == b);
    }

    public override string ToString()
    {
        return "(" + x + ", " + y + ")";
    }
}
