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
    public StateBase<GameState> CurrentState { get => m_currentState; set => m_currentState = value; }
    public StateBase<GameState> PreviousState { get => m_previuosState; set => m_previuosState = value; }

    public Dictionary<GameState, StateBase<GameState>> GameStates;
    public CombosFinder CombosFinder;
    public RefillManager RefillManager;

    private StateBase<GameState> m_currentState;
    private StateBase<GameState> m_previuosState;

    private void Start()
    {
        GameStates = new Dictionary<GameState, StateBase<GameState>>();
        GameStates.Add(GameState.WaitMove, new WaitMoveState());
        GameStates.Add(GameState.ComputeCombos, new ComputeCombosState(CombosFinder));
        GameStates.Add(GameState.RefillGrid, new RefillGridState(RefillManager));


        m_currentState = GameStates[GameState.WaitMove];
        m_currentState.OnEnter();
    }

    private void Update()
    {
        m_currentState.OnUpdate();
    }

    public void ChangeState(GameState state)
    {
        m_previuosState = m_currentState;
        m_currentState.OnExit();
        m_currentState = GameStates[state];
        m_currentState.OnEnter();
    }
}

public enum GameState
{
    WaitMove,
    ComputeCombos,
    RefillGrid
}
