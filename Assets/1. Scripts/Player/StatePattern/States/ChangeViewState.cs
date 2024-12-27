using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeViewState : IState
{
    PlayerController player;

    public ChangeViewState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("ChangeView 상태 진입");
        player._fireInfo = FireInfo.Empty;
    }

    public void Execute()
    {
        ChangeViewExecute();
    }

    public void Exit()
    {
        Debug.Log("ChangeView 상태 끝");
    }

    private void ChangeViewExecute()
    {
        player.AimTarget.position = Vector3.Lerp(player.AimTarget.position, player.AimTargetOriginPos, Time.deltaTime * 10f);
    }
}
