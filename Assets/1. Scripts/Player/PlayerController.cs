using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Contoller")]
    [SerializeField] private JoyStick _joyStick;
    [SerializeField] private PerspVirtualCameraRotate _fpsController;

    [Header("Fire Info")]
    public float FireSpeed = 20f;
    public float FireMass = 1f;
    public FireInfo _fireInfo = FireInfo.Empty;

    [Header("Aim Anchor")]
    public Transform AimAnchor;
    public Transform AimTarget;
    public Vector3 AimTargetOriginPos;
    public float AimTargetOriginDistance = 3f;

    [Header("Shoot Path")]
    public Transform ShootPoint;
    public LineRenderer PathLine;
    public int PathCount = 10;
    public float TimeStep = 0.1f;

    [Header("Ball")]
    public GameObject Ball;

    public PlayerStateMachine PStateMachine => playerStateMachine;
    private PlayerStateMachine playerStateMachine;

    public Animator Ani => _ani;
    private Animator _ani;

    private void Awake()
    {
        _ani = GetComponent<Animator>();
        AimTargetOriginPos = AimAnchor.position + new Vector3(AimTargetOriginDistance, 0, 0);

        playerStateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        playerStateMachine.Initialize(playerStateMachine.idleState);
    }

    private void Update()
    {
        if(GameManager.Instance.CurrentGameState == GameManager.GameState.OrthoMode)
        {
            _fireInfo = _joyStick.PlayerFireInfo;
        }
        else
        {
            _fireInfo = _fpsController.PlayerFireInfo;
        }
        playerStateMachine.Execute();
    }

    public void ShootBall(Vector3 force)
    {
        GameObject go = Instantiate(Ball, ShootPoint.position, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody>();

        rb.mass = FireMass;
        rb.AddForce(force, ForceMode.Impulse);
        Destroy(go, 3.0f);
    }
}
