using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StateLock : MonoBehaviour
{
    public static StateLock Instance;
    public const int maxWaitlistReferences = 3;

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

    public List<State> states = new List<State>();

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
