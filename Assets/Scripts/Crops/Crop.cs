using UnityEngine;
using System.Collections;

public class Crop : MonoBehaviour {

    public delegate void StageChangedEventHandler(object _sender, int _stage);
    public event StageChangedEventHandler StageChanged;

    [SerializeField]
    [Tooltip("The crop name.")]
    private string m_name;

    [SerializeField]
    [Tooltip("Grow rate of the crop, in percent per real world seconds")]
    private float m_growRate;

    [SerializeField]
    [Tooltip("Number of stage the plant will go through (must be at least 2).")]
    private int m_stageCount = 2;

    public string Name
    {
        get { return m_name; }
        private set { m_name = value; }
    }

    public float GrowRate
    {
        get { return m_growRate; }
        private set { m_growRate = value; }
    }

    public int StageCount
    {
        get { return m_stageCount; }
        private set { m_stageCount = Mathf.Min(value, 2); }
    }

    private float m_progress = 0;
    private int m_currentStage = 0;
    private bool m_growing = true;
    private bool m_harvestable = false;

    private void Start()
    {
        if (StageCount < 2)
            StageCount = 2;
    }

    private void Update()
    {
        if(!m_harvestable)
        {
            if(m_growing)
            {
                m_progress += Time.deltaTime * GrowRate / 100.0f;
                if(m_progress >= (m_currentStage + 1.0f) / (StageCount - 1.0f))
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
        return StageCount;
    }
}
