using System.Collections.Generic;
using UnityEngine;

public class StateBase
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

    protected List<State> states;

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
