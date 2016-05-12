using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DayNightEffect : MonoBehaviour {

    [Range(0, 1)]
    // Time of Day between 0 (night) and 1 (day)
    public float m_TOD;

    private Material m_material;

    private void Awake()
    {
        m_material = new Material(Shader.Find("Hidden/DayNightCycleEffect"));
    }

    private void OnRenderImage(RenderTexture _source, RenderTexture _destination)
    {
        m_material.SetFloat("_TOD", m_TOD);
        Graphics.Blit(_source, _destination, m_material);
    }
}
