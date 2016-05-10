using UnityEngine;
using System.Collections;
using DEngine.DayNightCycle;

public class GameManager : MonoBehaviour {

    private static GameManager s_Instance = null;
    public static GameManager Instance
    {
        get { return s_Instance; }
        private set { s_Instance = value; }
    }

    private GlobalClock m_GlobalClock;

    public GlobalClock GlobalClock
    {
        get { return m_GlobalClock; }
    }

	private void Awake()
    {
        m_GlobalClock = GetComponent<GlobalClock>();

        s_Instance = this;
    }
}
