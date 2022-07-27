using UnityEngine;

[DisallowMultipleComponent]
public class FindManager : MonoBehaviour
{
    private static FindManager Instance;
    private Transform playerTransform;
    private Transform[] transforms;

    private void Awake()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        if (playerTransform != null)
        {
            transforms = playerTransform.GetComponentsInChildren<Transform>();
        }
    }

    public static Transform Find(string name)
    {
        for (int i = 0; i < Instance.transforms.Length; i++)
        {
            if (name == "playerTransform")
            {
                return Instance.playerTransform;
            }

            if (Instance.transforms[i].name == name)
            {
                return Instance.transforms[i];
            }
        }
        return null;
    }
}
