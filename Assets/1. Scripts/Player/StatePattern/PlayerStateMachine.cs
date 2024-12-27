using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    public IState CurrentState { get; private set; }

    public IdleState idleState;
    public AimingState aimingState;
    public ShootState shootState;
    public ChangeViewState changeViewState;

    public event Action<IState> stateChanged;

    public PlayerStateMachine(PlayerController player)
    {
        this.idleState = new IdleState(player);
        this.aimingState = new AimingState(player);
        this.shootState = new ShootState(player);
        this.changeViewState = new ChangeViewState(player);
    }

    public void Initialize(IState state)
    {
        CurrentState = state;
        state.Enter();

        stateChanged?.Invoke(state);
    }

    public void TransitionTo(IState nextState)
    {
        CurrentState.Exit();
        CurrentState = nextState;
        nextState.Enter();

        stateChanged?.Invoke(nextState);
    }

    public void Execute()
    {
        if(CurrentState != null)
        {
            CurrentState.Execute();
        }
    }
}
