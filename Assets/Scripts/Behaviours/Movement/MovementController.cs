﻿using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour, IMove
{
    [Range(0, 1)]
    public float maxVelocity = 0.25f;
    public float timeToMaxVelocitySec = 0.05f;
    public float timeToZeroVelocitySec = 0.01f;
    public MovementState currentState;

    public float MaxVelocity => maxVelocity;
    public float TimeToMaxVelocitySec => timeToMaxVelocitySec;
    public float TimeToZeroVelocitySec => timeToZeroVelocitySec;
    public Transform Transform => gameObject.transform;
    public Rigidbody2D Rigidbody2d => m_rigidbody2d;
    public MovementState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    public Vector3 PreviousVelocity
    {
        get { return m_previousVelocity; }
        set { m_previousVelocity = value; }
    }

    private Rigidbody2D m_rigidbody2d;
    private Vector3 m_previousVelocity;
    private BehaviourStateMachine<IMove, MovementState> movementMachine;

    void Awake()
    {
        movementMachine = new BehaviourStateMachine<IMove, MovementState>(new Dictionary<MovementState, IBehaviourState<IMove, MovementState>>
        {
            { MovementState.NotMoving, new NotMovingState() },
            { MovementState.Acceleration, new AccelerationState() },
            { MovementState.MaxSpeed, new MaxSpeedState() },
            { MovementState.Deacceleration, new DeaccelerationState() }
        }, this);

        movementMachine.ChangeState(MovementState.NotMoving);

        m_rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        movementMachine.Update();
        currentState = movementMachine.currentState;
    }
}
