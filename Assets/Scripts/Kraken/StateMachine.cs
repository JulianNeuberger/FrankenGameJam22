using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State startState;
    [HideInInspector]
    public State currentState;
    private readonly List<State> _states = new();

    // Start is called before the first frame update
    void Start()
    {
        currentState = startState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.Tick();
    }

    private void OnDrawGizmos()
    {
        if (currentState == null)
        {
            return;
        }
        Handles.Label(transform.position + Vector3.up * 2f, currentState.GetType().Name + ": " + currentState.Description());
    }
}
