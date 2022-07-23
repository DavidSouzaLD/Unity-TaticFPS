using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StateLock : MonoBehaviour
{
    public static StateLock Global;

    [System.Serializable]
    public class LockingClass
    {
        public string name;
        public bool locked;

        public LockingClass(string _key, bool _locked)
        {
            name = _key;
            locked = _locked;
        }
    }

    public List<LockingClass> LockList = new List<LockingClass>();

    public static bool IsLocked(string _keyName)
    {
        string keyName = _keyName.ToUpper();

        for (int i = 0; i < Global.LockList.Count; i++)
        {
            if (Global.LockList[i].name.Equals(keyName))
            {
                return Global.LockList[i].locked;
            }
        }

        Lock(keyName, false);
        return false;
    }

    public static void Lock(string _key, bool _locked = true)
    {
        string newName = _key.ToUpper();

        for (int i = 0; i < Global.LockList.Count; i++)
        {
            if (Global.LockList[i].name.ToUpper().Equals(newName))
            {
                Global.LockList[i].name = newName;
                Global.LockList[i].locked = _locked;

                return;
            }
        }

        Global.LockList.Add(new LockingClass(newName, _locked));
    }

    private void Awake()
    {
        if (Global == null)
        {
            Global = this;
        }
    }
}
