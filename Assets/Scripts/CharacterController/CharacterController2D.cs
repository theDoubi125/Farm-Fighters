using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour {

    public enum CharacterState
    {
        IDLE,
        WALKING
    }

    public delegate void CharacterStateChangedEventHandler(object _sender, CharacterState _old, CharacterState _new);
    public event CharacterStateChangedEventHandler StateChanged;

    [SerializeField]
    private float m_MaxSpeed = 10.0f;

    [SerializeField]
    private float m_MaxAcceleration = 1000.0f;

    private Rigidbody2D m_rigidbody;
    private Vector2 m_TargetSpeed;
    private Vector2 m_Speed;
    private CharacterState m_state = CharacterState.IDLE;

    public CharacterState OldState { get; private set; }

    public CharacterState State
    {
        get { return m_state; }
        private set
        {
            OldState = m_state;
            m_state = value;
            OnStateChanged();
        }
    }

    public float MaxSpeed
    {
        get { return m_MaxSpeed; }
    }

    public float MaxAcceleration
    {
        get { return m_MaxAcceleration; }
    }

    public Vector2 Speed
    {
        get { return m_Speed; }
    }

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Vector2 deltaSpeed = m_TargetSpeed - m_Speed;
        m_Speed += deltaSpeed.normalized * Mathf.Min(deltaSpeed.magnitude, MaxAcceleration);
        if(m_Speed.sqrMagnitude > m_MaxSpeed*m_MaxSpeed)
            m_Speed = m_Speed.normalized * m_MaxSpeed;


        if(State == CharacterState.IDLE)
        {
            if (m_Speed.sqrMagnitude > 0.001f)
                State = CharacterState.WALKING;
        }

        if(State == CharacterState.WALKING)
        {
            ApplySpeed();
            if (m_Speed.sqrMagnitude <= 0.001f)
                State = CharacterState.IDLE;
        }
    }

    private void ApplySpeed()
    {
        Vector3 newPosition = transform.position + new Vector3(Speed.x, Speed.y, 0) * Time.fixedDeltaTime;
        transform.position = newPosition;
    }

    private void OnStateChanged()
    {
        if (StateChanged != null)
            StateChanged(this, OldState, State);
    }

    public void Move(Vector2 _targetSpeed)
    {
        m_TargetSpeed = _targetSpeed;
    }
}
