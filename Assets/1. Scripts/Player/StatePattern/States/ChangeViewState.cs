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
        Debug.Log("ChangeView ���� ����");
        player._fireInfo = FireInfo.Empty;
    }

    public void Execute()
    {
        ChangeViewExecute();
    }

    public void Exit()
    {
        Debug.Log("ChangeView ���� ��");
    }

    private void ChangeViewExecute()
    {
        player.AimTarget.position = Vector3.Lerp(player.AimTarget.position, player.AimTargetOriginPos, Time.deltaTime * 10f);
    }
}
