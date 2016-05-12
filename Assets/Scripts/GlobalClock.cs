using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DEngine.DayNightCycle
{
    public class GlobalClock : MonoBehaviour
    {

        public enum KeyTimeOfDay
        {
            MORNING,
            NIGHT,
            MIDNIGHT,
            MIDDAY
        }

        public static float SECONDS_IN_MINUTES = 60.0f;
        public static float SECONDS_IN_HOURS = 3600.0f;
        public static float SECONDS_IN_DAYS = 86400.0f;

        public static float MIDNIGHT = 0;
        public static float MIDDAY = 12 * SECONDS_IN_HOURS;

        [SerializeField]
        private float StartTime = 12 * SECONDS_IN_HOURS;

        [SerializeField]
        private float NightTime = 20 * SECONDS_IN_HOURS;

        [SerializeField]
        private float DayTime = 6 * SECONDS_IN_HOURS;

        private float m_time = 0;

        [SerializeField]
        private float m_speed = SECONDS_IN_MINUTES; // how many virtual seconds in one real second ?

        public float Speed
        {
            get
            {
                return m_speed;
            }
            set
            {
                m_speed = value;
            }
        }

        public delegate void AlarmHandler();
        private List<AlarmRequest> m_requests = new List<AlarmRequest>();

        private class AlarmRequest
        {
            public float m_timeUntilRepeat;
            public float m_timeSinceLastCall;
            public float m_nextCallTime;
            public bool m_loop;
            public int m_loopCount;
            public int m_callCount;
            public AlarmHandler m_callback;
        }

        public int Second
        {
            get { return Mathf.FloorToInt(m_time % 60); }
        }

        public int Minute
        {
            get { return Mathf.FloorToInt((m_time / SECONDS_IN_MINUTES) % 60); }
        }

        public int Hour
        {
            get { return Mathf.FloorToInt((m_time / SECONDS_IN_HOURS) % 24); }
        }

        public int Day
        {
            get { return Mathf.FloorToInt(m_time / SECONDS_IN_DAYS); }
        }

        private void Start()
        {
            m_time = StartTime;
        }

        private void Update()
        {
            m_time += Time.deltaTime * m_speed;
            foreach(var request in m_requests)
            {
                if(m_time >= request.m_nextCallTime)
                {
                    if (request.m_callback != null)
                        request.m_callback();
                    request.m_timeSinceLastCall = request.m_nextCallTime;
                    request.m_nextCallTime = request.m_timeSinceLastCall + request.m_timeUntilRepeat;
                    request.m_callCount++;
                }
            }

            // Remove anything in request that doesn't loop, or that doesn't loop infinitly and has looped enough times.
            m_requests.RemoveAll((request) => { return (request.m_loop == false && request.m_callCount > 0) || (request.m_loopCount > 0 && request.m_callCount >= request.m_loopCount); });
        }

        private void Reset()
        {
            m_time = StartTime;
        }

        public float GetSecondsSinceStartOfDay()
        {
            return Mathf.Repeat(m_time, SECONDS_IN_DAYS);
        }

        private void RequestAlarm(AlarmHandler _callback, float _seconds, bool _loop = false, int _loopCount = 0)
        {
            AlarmRequest request = new AlarmRequest();
            request.m_callback = _callback;
            request.m_loop = _loop;
            request.m_loopCount = _loopCount;
            request.m_callCount = 0;
            request.m_timeSinceLastCall = Day * SECONDS_IN_DAYS + _seconds;
            if (request.m_timeSinceLastCall > m_time) request.m_timeSinceLastCall -= SECONDS_IN_DAYS;
            request.m_timeUntilRepeat = SECONDS_IN_DAYS;
            request.m_nextCallTime = request.m_timeSinceLastCall + request.m_timeUntilRepeat;

            m_requests.Add(request);
        }

        // request an alarm to be setup at _hour:_minute:_second of the day, can be looping, if loop, lopped _loopCount times or infinitely if _loopCount = 0
        public void RequestAlarm(AlarmHandler _callback, int _hour, int _minute, int _second, bool _loop = false, int _loopCount = 0)
        {
            RequestAlarm(_callback, _hour * SECONDS_IN_HOURS + _minute * SECONDS_IN_MINUTES + _second, _loop, _loopCount);
        }

        public void RequestAlarm(AlarmHandler _callback, KeyTimeOfDay _tod, bool _loop = false, int _loopCount = 0)
        {
            RequestAlarm(_callback, TODToSeconds(_tod), _loop, _loopCount);
        }

        public float TODToSeconds(KeyTimeOfDay _tod)
        {
            switch(_tod)
            {
                case KeyTimeOfDay.MIDNIGHT:
                    return MIDNIGHT;
                case KeyTimeOfDay.MIDDAY:
                    return MIDDAY;
                case KeyTimeOfDay.MORNING:
                    return DayTime;
                case KeyTimeOfDay.NIGHT:
                    return NightTime;
                default:
                    return MIDNIGHT;
            }
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width - 150, 10, Screen.width, Screen.height));
            GUILayout.Label(Hour + "h" + Minute + ":" + Second);
            GUILayout.EndArea();
        }
    }
}
