using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State startState;

    private State _currentState;
    private readonly List<State> _states = new();

    // Start is called before the first frame update
    void Start()
    {
        _currentState = startState;
    }

    // Update is called once per frame
    void Update()
    {
        _currentState = _currentState.Tick();
    }

    private void OnDrawGizmos()
    {
        if (_currentState == null)
        {
            return;
        }
        Handles.Label(transform.position + Vector3.up * 2f, _currentState.GetType().Name + ": " + _currentState.Description());
    }
}
