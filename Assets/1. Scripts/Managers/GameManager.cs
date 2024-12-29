using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        OrthoMode,
        PerspMode
    }

    [SerializeField] private GameObject _orthoPanel;
    [SerializeField] private GameObject _perspPanel;
    [SerializeField] private CinemachineVirtualCamera _orthoCamera;
    [SerializeField] private CinemachineVirtualCamera _perspCamera;

    [SerializeField] private GameObject _clearPanel;
    public GameState CurrentGameState => _gameState;
    private GameState _gameState = GameState.OrthoMode;

    private void Awake()
    {
        Instance = this;
        _clearPanel.SetActive(false);
    }

    void Start()
    {
        _orthoPanel.SetActive(true);
        _perspPanel.SetActive(false);
        _orthoCamera.Priority = 1;
        _perspCamera.Priority = 0;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    StartCoroutine(ChangingViewState(_gameState));
        //}
    }

    public IEnumerator ChangingViewState(GameState gameState)
    {
        ChangeView(gameState);
        Debug.Log(name+"코루틴 작동" + gameState);
        yield return new WaitForSeconds(0.8f);

        if(_gameState == GameState.OrthoMode)
        {
            _orthoPanel.SetActive(true);
        }
        else
        {
            _perspPanel.SetActive(true);
        }
    }

    private void ChangeView(GameState gameState)
    {
        Debug.Log(name+"ChangeView 작동" + gameState);
        switch (gameState)
        {
            case GameState.OrthoMode:
                Debug.Log("작동 OrthoState");
                _perspPanel.SetActive(false);
                _orthoCamera.Priority = 1;
                _perspCamera.Priority = 0;
                _gameState = GameState.OrthoMode;
                break;
            case GameState.PerspMode:
                Debug.Log("작동 PerspState");
                _orthoPanel.SetActive(false);
                _orthoCamera.Priority = 0;
                _perspCamera.Priority = 1;
                _gameState = GameState.PerspMode;
                break;
        }
    }

    public void MoveClearScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _clearPanel.SetActive(true);
    }

    public void MoveDefeatScene()
    {

    }
}