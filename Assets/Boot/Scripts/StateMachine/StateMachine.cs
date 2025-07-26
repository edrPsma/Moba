using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TState> where TState : struct
{
    public TState? CurrentState { get; private set; }

    Dictionary<TState, IState> _states = new Dictionary<TState, IState>();

    public void AddState(TState state, IState stateInstance)
    {
        if (!_states.ContainsKey(state))
        {
            _states[state] = stateInstance;
        }
    }

    public void ChangeState(TState newState)
    {
        if (_states.ContainsKey(newState))
        {
            if (CurrentState != null && _states.ContainsKey(CurrentState.Value))
            {
                _states[CurrentState.Value].Exit();
            }

            CurrentState = newState;
            _states[CurrentState.Value].Enter();
        }
        else
        {
            Debug.LogError($"State {newState} not found in the state machine.");
        }
    }

    public void Update()
    {
        if (CurrentState != null && _states.ContainsKey(CurrentState.Value))
        {
            _states[CurrentState.Value].Update();
        }
    }

    public void Run(TState state)
    {
        if (_states.ContainsKey(state))
        {
            ChangeState(state);
        }
        else
        {
            Debug.LogError($"State {state} not found in the state machine.");
        }
    }

    public void Stop()
    {
        if (CurrentState != null && _states.ContainsKey(CurrentState.Value))
        {
            _states[CurrentState.Value].Exit();
            CurrentState = null;
        }
    }
}
