using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootState : IState
{
    PlayerController player;

    public ShootState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        //Debug.Log("Shoot ���� ����");
        player._audioSource.PlayOneShot(player.audioClips[1]);
        player.ShootBall(CalculateForce());
    }

    public void Execute()
    {
        player.PStateMachine.TransitionTo(player.PStateMachine.idleState);
    }

    public void Exit()
    {
        //Debug.Log("Shoot ���� ��");
    }

    private Vector3 CalculateForce()
    {
        Vector3 force = player._fireInfo.Direction * (player.FireSpeed * player._fireInfo.Speed);
        return force;
    }
}
