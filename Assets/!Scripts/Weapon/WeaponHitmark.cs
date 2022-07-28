using UnityEngine;

public class WeaponHitmark : MonoBehaviour
{
    [Header("HitMark")]
    [SerializeField] private AudioClip hitMarkSound;
    [SerializeField] private float hitMarkTime;

    private float timerHitMark;
    private GameObject hitMark;

    public AudioClip GetHitMarkSound()
    {
        return hitMarkSound;
    }

    private void Start()
    {
        // Get components
        hitMark = FindManager.Find("HitMark").gameObject;

        // Others
        hitMark.SetActive(false);
    }

    private void Update()
    {
        if (timerHitMark > 0)
        {
            if (timerHitMark - Time.deltaTime <= 0)
            {
                hitMark.SetActive(false);
            }

            timerHitMark -= Time.deltaTime;
        }
    }

    public void ApplyHitMark(Vector3 _position)
    {
        if (timerHitMark <= 0)
        {
            hitMark.transform.position = _position;
            hitMark.SetActive(true);
            timerHitMark = hitMarkTime;
        }
    }
}
