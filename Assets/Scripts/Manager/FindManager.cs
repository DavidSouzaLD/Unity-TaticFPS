using UnityEngine;

[DisallowMultipleComponent]
public class FindManager : MonoBehaviour
{
    private static FindManager Instance;
    private Transform Player;
    private Transform[] transforms;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        if (Player == null)
        {
            DebugManager.DebugAssignedError("Player");
        }
        else
        {
            transforms = Player.GetComponentsInChildren<Transform>();
        }
    }

    public static Transform Find(string name)
    {
        for (int i = 0; i < Instance.transforms.Length; i++)
        {
            if (name == "Player")
            {
                return Instance.Player;
            }

            if (Instance.transforms[i].name == name)
            {
                return Instance.transforms[i];
            }
        }
        return null;
    }
}
