using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private PlayerController player;

    // Aim
    private Vector3 _aimTargetOriginPos;
    private Transform _aimTarget;

    public IdleState(PlayerController player)
    {
        this.player = player;
        _aimTargetOriginPos = player.AimTargetOriginPos;
        _aimTarget = player.AimTarget;
    }

    public void Enter()
    {
        //Debug.Log("Idle 상태 진입");
    }

    public void Execute()
    {
        IdleStateExecute();

        if(player._fireInfo.IsAiming)
        {
            player.PStateMachine.TransitionTo(player.PStateMachine.aimingState);
        }
    }

    public void Exit()
    {
        //Debug.Log("Idle 상태 끝");
    }

    private void IdleStateExecute()
    {
        _aimTarget.position = Vector3.Lerp(_aimTarget.position, _aimTargetOriginPos, Time.deltaTime * 10f);
    }
}
