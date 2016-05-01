using UnityEditor;
using UnityEngine;
using System;
using System.Collections;

public class EditorTools
{
    public static int SliderIntValue(int value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Tile Width", GUILayout.Width(100));
        String fieldResult =  GUILayout.TextField("" + value, GUILayout.Width(50));
        int result;
        result = (int)GUILayout.HorizontalSlider(value, 1, 100);
        int parseResult;
        if (Int32.TryParse(fieldResult, out parseResult) && parseResult != value)
            result = parseResult;

        GUILayout.EndHorizontal();
        return result;
    }
}
