using System;
using UnityEngine;

[Serializable]
public class CropTemplate
{
    
    [SerializeField]
    private string m_name;

    [SerializeField]
    private string m_description;

    [SerializeField]
    [Range(2, 10)]
    private int m_stageCount;

    [SerializeField]
    [Range(0, 360)]
    private float m_timeToGrow;

    public string Name
    {
        get { return m_name; }
        set { m_name = value; }
    }

    public string Description
    {
        get { return m_description; }
        set { m_description = value; }
    }

    public int StageCount
    {
        get
        {
            return m_stageCount;
        }
        set
        {
            m_stageCount = value >= 2 ? value : 2;
        }
    }
    public float TimeToGrow
    {
        get
        {
            return m_timeToGrow;
        }
        set
        {
            m_timeToGrow = value >= 0 ? value : 0;
        }
    }

    public CropTemplate()
    {
        Name = "";
        Description = "";
        StageCount = 0;
        TimeToGrow = 0;
    }

    public CropTemplate(string _name, string _description, int _stageCount, float _timeToGrow)
    {
        Name = _name;
        Description = _description;
        StageCount = _stageCount;
        TimeToGrow = _timeToGrow;
    }

}
