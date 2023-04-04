using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton:
    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    public Dictionary<GameState, StateBase<GameState>> GameStates;
    [HideInInspector]
    public StateBase<GameState> CurrentState;

    private void Start()
    {
        GameStates = new Dictionary<GameState, StateBase<GameState>>();
        GameStates.Add(GameState.WaitMove, new WaitMoveState());


        CurrentState = GameStates[GameState.WaitMove];
        CurrentState.OnEnter();
    }

    private void Update()
    {
        CurrentState.OnUpdate();
    }

    public void ChangeState(GameState state)
    {
        CurrentState.OnExit();
        CurrentState = GameStates[state];
        CurrentState.OnEnter();
    }
}

public enum GameState
{
    WaitMove,
    ProcessMove,
    RefillGrid
}
