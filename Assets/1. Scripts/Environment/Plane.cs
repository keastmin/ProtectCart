using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    public float ScrollSpeed = 4f;
    private Renderer _rend;

    private Vector2 offset = Vector2.zero;

    private void Awake()
    {
        _rend = GetComponent<Renderer>();
    }

    void Update()
    {
        offset.x += Time.deltaTime * ScrollSpeed;
        offset.x %= 1f;
        _rend.material.mainTextureOffset = offset;
    }
}
