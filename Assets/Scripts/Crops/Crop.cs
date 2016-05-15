using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour {

    public delegate void StageChangedEventHandler(object _sender, int _stage);
    public event StageChangedEventHandler StageChanged;

    [SerializeField]
    private CropTemplate m_template;

    private float m_progress = 0;
    private int m_currentStage = 0;
    private bool m_growing = true;
    private bool m_harvestable = false;

    public CropTemplate Template
    {
        get { return m_template; }
        set
        {
            m_template = value;
            Reset();
        }
    }

    private void Reset()
    {
        m_progress = 0;
        m_currentStage = 0;
        m_growing = true;
        m_harvestable = false;
    }

    private void Update()
    {
        if (Template == null)
            return;

        if(!m_harvestable)
        {
            if(m_growing)
            {
                m_progress += Time.deltaTime / m_template.TimeToGrow;
                if(m_progress >= (m_currentStage + 1.0f) / (m_template.StageCount - 1.0f))
                {
                    // Make some checks to see if we're still growing ?

                    m_currentStage++;

                    if (m_progress >= 1)
                    {
                        m_progress = 1;
                        m_growing = false;
                        m_harvestable = true;
                    }

                    OnStageChanged();
                }
            }
            else
            {
                // Check something to be growing again ?
            }
        }
    }

    private void OnStageChanged()
    {
        if (StageChanged != null)
            StageChanged(this, m_currentStage);
    }

    public int GetStageCount()
    {
        return m_template.StageCount;
    }
}
