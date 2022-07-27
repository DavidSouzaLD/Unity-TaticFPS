using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    public class State
    {
        [HideInInspector]
        public string name;
        public bool value;

        public State(string _name, bool _value = false)
        {
            name = _name;
            value = _value;
        }
    }

    private List<State> states;

    public PlayerState()
    {
        states = new List<State>()
            {
                new State("Walking"),
                new State("Running"),
                new State("Crouching"),
                new State("Jumping"),
                new State("GroundArea"),
                new State("GroundCollision"),
                new State("Covering"),
                new State("Sloping"),
                new State("Aiming"),
                new State("Graviting"),
            };
    }

    public void SetState(string stateName, bool value)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].name.ToUpper().Equals(stateName.ToUpper()))
            {
                if (states[i].value != value)
                {
                    states[i].value = value;
                }
                return;
            }
        }
        Debug.LogError("(PlayerController) State not finded! + " + stateName);
    }

    public bool GetState(string stateName)
    {
        for (int i = 0; i < states.Count; i++)
        {
            if (states[i].name.ToUpper().Equals(stateName.ToUpper()))
            {
                return states[i].value;
            }
        }
        return false;
    }
}
