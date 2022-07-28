using UnityEngine;

[DisallowMultipleComponent]
public class FindManager : StaticInstance<FindManager>
{
    private Transform playerTransform;
    private Transform[] transforms;

    protected override void Awake()
    {
        base.Awake();

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

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
