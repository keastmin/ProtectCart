using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Effect Data", menuName = "Scriptable Object/Effect Data", order = int.MaxValue)]
public class HitEffectInfo : ScriptableObject
{
    public AudioClip hitSound;
}
