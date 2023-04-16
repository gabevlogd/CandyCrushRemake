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
    public Destroyer Destroyer;
    public RefillManager RefillManager;
    public PostRefillCombosManager PostRefillCombosManager;
    public PlayerScoresManager PlayerScoresManager;
    public HUDManager HUDManager;

    private StateBase<GameState> m_currentState;
    private StateBase<GameState> m_previuosState;

    private void Start()
    {
        GameStates = new Dictionary<GameState, StateBase<GameState>>();
        GameStates.Add(GameState.WaitMove, new WaitMoveState());
        GameStates.Add(GameState.ComputeCombos, new ComputeCombosState(Destroyer));
        GameStates.Add(GameState.RefillGrid, new RefillGridState(RefillManager));
        GameStates.Add(GameState.PostRefill, new PostRefillState(PostRefillCombosManager));
        GameStates.Add(GameState.GameWon, new GameWonState());
        GameStates.Add(GameState.GameLost, new GameLostState());



        m_currentState = GameStates[GameState.WaitMove];
        m_currentState.OnEnter();
    }

    private void Update()
    {
        m_currentState.OnUpdate();
        if (LoseCondition()) ChangeState(GameState.GameLost);
        if (WinCondition()) ChangeState(GameState.GameWon);
    }

    public void ChangeState(GameState state)
    {
        m_previuosState = m_currentState;
        m_currentState.OnExit();
        m_currentState = GameStates[state];
        m_currentState.OnEnter();
    }

    /// <summary>
    /// Returns true if the player loses the game
    /// </summary>
    /// <returns></returns>
    private bool LoseCondition()
    {
        if (m_currentState.StateID == GameState.WaitMove)
        {
            if (PlayerScoresManager.Data.Moves <= 0 && PlayerScoresManager.Data.Points < PlayerScoresManager.TargetPoints) return true;
        }

        return false;
    }

    /// <summary>
    /// Returns true if the player wins the game
    /// </summary>
    /// <returns></returns>
    private bool WinCondition()
    {
        if (m_currentState.StateID == GameState.WaitMove)
        {
            if (PlayerScoresManager.Data.Points >= PlayerScoresManager.TargetPoints) return true;
        }

        return false;
    }
}

public enum GameState
{
    WaitMove,
    ComputeCombos,
    RefillGrid,
    PostRefill,
    GameWon,
    GameLost
}
