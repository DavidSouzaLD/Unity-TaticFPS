using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* BASE STATE STYLE /*
[System.Serializable]
public class Base : States
{
    public Base()
    {
        states = new List<State>()
        {
            new State("Test1"),
            new State("Test2"),
            new State("Test3")
        };
    }
}
*/

namespace Game.Utilities
{
    /// <summary>
    ///It manages all the simple state schema of a script, just create 
    ///an instance of it where you want to create states and call its 
    ///voids for actions.
    /// </summary>
    public class States
    {
        /// <summary>
        /// State structure
        /// </summary>
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

        /// <summary>
        /// Current list of states
        /// </summary>
        protected List<State> states;

        /// <summary>
        /// Sets the current state value.
        /// </summary>
        /// <param name="_stateName">State name to be modified.</param>
        /// <param name="_value">Value that the state.</param>
        public void SetState(string _stateName, bool _value)
        {
            var getState =
                from state in states
                where state.name == _stateName
                select state;

            getState.ToArray()[0].value = _value;
        }

        /// <summary>
        /// Returns the current state value.
        /// </summary>
        /// <param name="_stateName">State name to be modified.</param>
        /// <returns></returns>
        public bool GetState(string _stateName)
        {
            var getState =
                from state in states
                where state.name == _stateName
                select state;

            return getState.ToArray()[0].value;
        }
    }
}