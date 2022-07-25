using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StateLock : MonoBehaviour
{
    /// <summary>
    /// Global public instance.
    /// </summary>
    public static StateLock Instance;

    /// <summary>
    /// Maximum number of objects on the waiting list.
    /// </summary>
    public const int maxWaitlistReferences = 3;

    /// <summary>
    /// List that saves all game states.
    /// </summary>
    [SerializeField] private List<State> states = new List<State>();

    [System.Serializable]
    public class State
    {
        public string key;
        public List<Behaviour> waitingList = new List<Behaviour>();
        public bool locked
        {
            get
            {
                return waitingList.Count > 0;
            }
        }

        public State(string senderKey)
        {
            key = senderKey;
        }

        /// <summary>
        /// Add to waiting list.
        /// </summary>
        public void AddWaitInList(Behaviour behaviour)
        {
            for (int i = 0; i < waitingList.Count; i++)
            {
                if (waitingList[i].Equals(behaviour))
                {
                    return;
                }
            }

            if (waitingList.Count <= maxWaitlistReferences)
            {
                waitingList.Add(behaviour);
            }
        }

        /// <summary>
        /// Remove to waiting list.
        /// </summary>
        public void RemoveAtWaitList(Behaviour behaviour)
        {
            for (int i = 0; i < waitingList.Count; i++)
            {
                if (waitingList[i].Equals(behaviour))
                {
                    waitingList.RemoveAt(i);
                    return;
                }
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Sets the current state of the state.
    /// </summary>
    /// <param name="key">Key to search state.</param>
    /// <param name="reference">Reference of the script that is blocking the state.</param>
    /// <param name="locking">Is it blocked or not?</param>
    public static void Lock(string key, Behaviour reference, bool locking)
    {
        if (locking)
        {
            for (int i = 0; i < Instance.states.Count; i++)
            {
                if (Instance.states[i].key.Equals(key))
                {
                    Instance.states[i].AddWaitInList(reference);
                    return;
                }
            }

            Instance.states.Add(new State(key));
        }
        else
        {
            for (int i = 0; i < Instance.states.Count; i++)
            {
                if (Instance.states[i].waitingList.Count > 0)
                {
                    if (Instance.states[i].key.Equals(key))
                    {
                        Instance.states[i].RemoveAtWaitList(reference);
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns whether the current state of the key is locked or not.
    /// </summary>
    /// <param name="key">Key to search state.</param>
    /// <returns></returns>
    public static bool IsLocked(string key)
    {
        for (int i = 0; i < Instance.states.Count; i++)
        {
            if (Instance.states[i].key.Equals(key))
            {
                return Instance.states[i].locked;
            }
        }

        return false;
    }
}
