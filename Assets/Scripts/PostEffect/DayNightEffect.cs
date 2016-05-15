using UnityEngine;
using System.Collections;
using DEngine.DayNightCycle;

[ExecuteInEditMode]
public class DayNightEffect : MonoBehaviour {

    [Range(0, 1)]
    // Time of Day between 0 (night) and 1 (day)
    public float m_TOD;

    public Gradient m_SkyColorGradient;
    public DayNightLight m_light;

    public RenderTexture m_lightmap;
    private Material m_material;
    private Material m_lightMaterial;

    private void Awake()
    {
        m_material = new Material(Shader.Find("Hidden/DayNightCycleEffect"));
        m_lightMaterial = new Material(Shader.Find("Hidden/LightEffect"));
        if (m_lightmap != null)
            m_lightmap.Release();

        m_lightmap = new RenderTexture(Screen.width, Screen.height, 0);
    }

    private void OnRenderImage(RenderTexture _source, RenderTexture _destination)
    {
        float seconds_since_start = GameManager.Instance.GlobalClock.GetSecondsSinceStartOfDay();
        if(seconds_since_start <= GlobalClock.MIDDAY)
        {
            m_TOD = seconds_since_start / GlobalClock.MIDDAY;
        }
        else
        {
            m_TOD = seconds_since_start / (GlobalClock.MIDDAY - GlobalClock.MIDNIGHT2) + (GlobalClock.MIDNIGHT2) / (GlobalClock.MIDDAY - GlobalClock.MIDNIGHT);
        }

        Vector3 lightPosition = Camera.main.WorldToScreenPoint(m_light.transform.position);
        Vector2 lightPosNormalized = new Vector2(lightPosition.x / Screen.width, 1.0f - (lightPosition.y / Screen.height));
        m_lightMaterial.SetVector("_LightPosition", new Vector4(lightPosNormalized.x, lightPosNormalized.y));
        m_lightMaterial.SetColor("_LightColor", m_light.color);
        m_lightMaterial.SetFloat("_LightRange", m_light.range);
        Graphics.Blit(m_lightmap, m_lightmap, m_lightMaterial);

        m_material.SetColor("_Color", m_SkyColorGradient.Evaluate(m_TOD));
        m_material.SetTexture("_LightMap", m_lightmap);
        Graphics.Blit(_source, _destination, m_material);
    }
}
