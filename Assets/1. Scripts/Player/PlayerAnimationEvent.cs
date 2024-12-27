using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] Transform BowString;
    [SerializeField] Transform RightHand;

    private void OnAnimatorMove()
    {
        BowString.position = RightHand.position;
    }
}
