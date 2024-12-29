using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class AimingState : IState
{
    private PlayerController player;

    // Path Line
    private Transform _shootPoint;
    private LineRenderer _lineRenderer;

    // Aim
    private Transform _aimAnchor;
    private Transform _aimTarget;

    // Shoot
    private Vector3 _force;
    private GameObject _ball;

    public AimingState(PlayerController player)
    {
        this.player = player;
        _shootPoint = player.ShootPoint;
        _lineRenderer = player.PathLine;
        _aimAnchor = player.AimAnchor;
        _aimTarget = player.AimTarget;
        _ball = player.Ball;
    }

    public void Enter()
    {
        player._audioSource.PlayOneShot(player.audioClips[0]);
        player.BallEffect.SetActive(true);
        _lineRenderer.enabled = true;
        player.Ani.SetBool("Aiming", true);
        //Debug.Log("Aiming 상태 진입");
    }

    public void Execute()
    {
        AimingStateExcute();

        if (player._fireInfo.IsShooting)
        {
            player.PStateMachine.TransitionTo(player.PStateMachine.shootState);
        }
        if(!player._fireInfo.IsAiming)
        {
            player.PStateMachine.TransitionTo(player.PStateMachine.idleState);
        }
    }

    public void Exit()
    {
        player.BallEffect.SetActive(false);
        _lineRenderer.positionCount = 0;
        _lineRenderer.enabled = false;
        player.Ani.SetBool("Aiming", false);
        //Debug.Log("Aiming 상태 끝");
    }

    private void AimingStateExcute()
    {
        DrawPathLine();
        CanDragPositionMove();
    }

    private void CanDragPositionMove()
    {
        Vector3 aimTargetPos = _aimAnchor.position + (player._fireInfo.Direction * player.AimTargetOriginDistance);

        _aimTarget.position = Vector3.Lerp(_aimTarget.position, aimTargetPos, Time.deltaTime * 10f);
    }

    private void DrawPathLine()
    {
        List<Vector3> trajectory = new List<Vector3>();

        _force = player._fireInfo.Direction * (player.FireSpeed * player._fireInfo.Speed);
        Vector3 position = _shootPoint.position;
        Vector3 velocity = _force / player.FireMass;

        trajectory.Add(position);

        for(int i = 1; i <= player.PathCount; i++)
        {
            float timeElapsed = player.TimeStep * i;

            trajectory.Add(position +
                velocity * timeElapsed +
                Physics.gravity * (0.5f * timeElapsed * timeElapsed));

            if (CheckCollision(trajectory[i - 1], trajectory[i], out Vector3 hitPoint))
            {
                trajectory[i] = hitPoint;
                break;
            }
        }

        _lineRenderer.positionCount = trajectory.Count; // 점의 개수 설정
        for (int i = 0; i < trajectory.Count; i++)
        {
            _lineRenderer.SetPosition(i, trajectory[i]); // 각 점의 위치 설정
        }
    }

    private bool CheckCollision(Vector3 start, Vector3 end, out Vector3 hitPoint)
    {
        hitPoint = end;
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        if (Physics.Raycast(start, direction.normalized, out RaycastHit hit, distance, 1 << LayerMask.NameToLayer("Default")))
        {
            hitPoint = hit.point;
            return true;
        }

        return false;
    }
}
