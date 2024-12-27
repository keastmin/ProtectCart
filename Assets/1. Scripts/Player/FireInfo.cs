using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FireInfo
{
    public static FireInfo Empty = new FireInfo();
    public bool IsAiming;
    public bool IsShooting;
    public Vector3 Direction;
    public float Speed;

    public FireInfo(bool isAiming = false, bool isShooting = false, Vector3 direction = default, float speed = 0)
    {
        IsAiming = isAiming;
        IsShooting = isShooting;
        Direction = direction;
        Speed = speed;
    }
}
