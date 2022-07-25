using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponSystem : MonoBehaviour
{
    public enum FireMode { Semi, Auto, Gust }

    [Header("[Weapon Settings]")]
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private FireMode fireMode;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float maxRange = 100f;

    [Header("[Weapon Data]")]
    public int bulletsPerMagazine = 12;
    public int currentBullets = 12;
    public int extraBullets = 24;

    [Header("[Fire Settings]")]
    [SerializeField] private float firerate = 1f;
    [SerializeField] private float bulletHitForce = 100f;
    [SerializeField] private float bulletVelocity = 25f;
    [SerializeField] private int bulletsPerFire = 1;

    [Header("[Aim Settings]")]
    [SerializeField] private float aimSpeed = 5f;
    [SerializeField] private float aimWeaponSwayAccuracy = 0.2f;
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Quaternion aimRotation;

    [Header("[Recoil Settings]")]
    [SerializeField] private Transform recoilTransform;
    [SerializeField] private Vector3 weaponRecoilPos;
    [SerializeField] private Vector3 weaponRecoilRot;
    [SerializeField] private Vector3 cameraRecoil;
    [Space]
    [SerializeField] private float resetRecoil = 5f;


    [Header("[Sounds Settings]")]
    [SerializeField] private AudioClip[] fireSound;
    [SerializeField] private AudioClip noBulletsSound;
    [SerializeField] private AudioClip startReloadSound;
    [SerializeField] private AudioClip middleReloadSound;
    [SerializeField] private AudioClip endReloadSound;

    [Header("[Gizmos Settings]")]
    [SerializeField] private bool disableGizmos;

    // State
    [HideInInspector] public bool isReloading, isAim, isInspect;

    // Privates
    private float firerateTimer;
    private Vector3 startAimPos;
    private Vector3 startRecoilPos;
    private Quaternion startAimRot;
    private Quaternion startRecoilRot;
    private NightVision NightVision;
    private Animator Animator;
    private AudioSource Source;

    /// <summary>
    /// Returns if there is a bullet in the current magazine.
    /// </summary>
    public bool HaveBulletsInMagazine
    {
        get
        {
            return currentBullets > 0;
        }
    }

    private void Start()
    {
        // Components
        NightVision = GameObject.FindObjectOfType<NightVision>();
        Animator = GetComponentInChildren<Animator>();
        Source = GetComponent<AudioSource>();

        // Aim
        startAimPos = transform.localPosition;
        startAimRot = transform.localRotation;

        // Recoil
        startRecoilPos = recoilTransform.localPosition;
        startRecoilRot = recoilTransform.localRotation;
    }

    private void Update()
    {
        FireUpdate();
        ReloadUpdate();
        RecoilUpdate();
        AimUpdate();
        InspectUpdate();
    }

    private void FireUpdate()
    {
        bool stateLock = !StateLock.IsLocked("WEAPON_ALL") && !StateLock.IsLocked("WEAPON_FIRE") && StateLock.IsLocked("CURSOR_LOCKED");
        bool canFire = stateLock && !Player.isRunning && !isReloading;

        if (canFire)
        {
            // Fire
            switch (fireMode)
            {
                case FireMode.Semi:

                    if (Input.FireTap)
                    {
                        FireEvent();
                    }

                    break;

                case FireMode.Auto:

                    if (Input.FireAuto)
                    {
                        FireEvent();
                    }

                    break;

                case FireMode.Gust:

                    if (Input.FireTap)
                    {
                        for (int i = 0; i < bulletsPerFire; i++)
                        {
                            FireEvent();
                        }
                    }

                    break;
            }

            if (firerateTimer >= 0)
            {
                firerateTimer -= Time.deltaTime;
            }
        }
        else
        {
            isAim = false;
        }
    }

    private void ReloadUpdate()
    {
        bool stateLock = !StateLock.IsLocked("WEAPON_ALL") && !StateLock.IsLocked("WEAPON_RELOAD");
        bool canReload = stateLock && Input.Reload && PlayerCamera.CursorLocked && !Player.isRunning && extraBullets > 0 && currentBullets < bulletsPerMagazine && !isReloading;

        if (canReload)
        {
            AnimEvent("RELOAD");
        }

        // Animation no-bullet
        Animator.SetBool("NO_BULLET", !HaveBulletsInMagazine);
    }

    private void RecoilUpdate()
    {
        if (recoilTransform.localPosition != startRecoilPos || recoilTransform.localRotation != startRecoilRot)
        {
            recoilTransform.localPosition = Vector3.Lerp(recoilTransform.localPosition, startRecoilPos, resetRecoil * Time.deltaTime);
            recoilTransform.localRotation = Quaternion.Slerp(recoilTransform.localRotation, startRecoilRot, resetRecoil * Time.deltaTime);
        }
    }

    private void AimUpdate()
    {
        bool stateLock = !StateLock.IsLocked("WEAPON_ALL") && !StateLock.IsLocked("WEAPON_AIM");
        bool canAim = stateLock && Input.Aim && PlayerCamera.CursorLocked && !isReloading && !Player.isRunning;

        Player.isAim = isAim;

        if (canAim)
        {
            isAim = true;
            WeaponManager.SwayAccuracy(aimWeaponSwayAccuracy);
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, aimSpeed * Time.deltaTime);
        }
        else
        {
            isAim = false;
        }

        bool canResetAim = (!Input.Aim || Input.Run) && (transform.localPosition != startAimPos || transform.localRotation != startAimRot);

        if (canResetAim)
        {
            WeaponManager.MaxAccuracy();
            transform.localPosition = Vector3.Lerp(transform.localPosition, startAimPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startAimRot, aimSpeed * Time.deltaTime);
        }
    }

    private void InspectUpdate()
    {
        bool stateLock = !StateLock.IsLocked("WEAPON_ALL") && !StateLock.IsLocked("WEAPON_INSPECT");
        bool canInspect = stateLock && Input.Inspect && !isReloading && !isAim;

        if (canInspect)
        {
            AnimEvent("INSPECT");
        }
    }

    private void FireEvent()
    {
        if (currentBullets >= bulletsPerFire && firerateTimer <= 0)
        {
            // Tracer
            List<Vector3> positions = new List<Vector3>();
            LineRenderer tracer = Instantiate(WeaponManager.GetTracerPrefab, firePoint.position, Quaternion.identity).GetComponent<LineRenderer>();
            Tracer tracerScript = tracer.gameObject.GetComponent<Tracer>();

            Vector3 point1 = firePoint.position;
            Vector3 predictedBulletVelocity = firePoint.forward * maxRange;
            float stepSize = 0.01f;

            // Tracer start position
            positions.Add(point1);

            for (float step = 0f; step < 1; step += stepSize)
            {
                predictedBulletVelocity += Physics.gravity * stepSize;
                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;

                // Tracer positions
                positions.Add(point2);

                Ray ray = new Ray(point1, point2 - point1);
                RaycastHit hit;

                Debug.DrawLine(point1, point2, Color.green);

                if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, targetLayers))
                {
                    if (hit.transform)
                    {
                        float distance = (firePoint.position - hit.point).sqrMagnitude;
                        float time = hit.distance / bulletVelocity;
                        StartCoroutine(DelayFire(time, hit));
                        break;
                    }
                }

                point1 = point2;
            }

            // Set tracer
            tracerScript.pos = positions;

            RecoilEvent();
            AnimEvent("FIRE");
            SoundEvent("FIRE");

            currentBullets -= bulletsPerFire;
            firerateTimer = firerate;
        }
    }

    public void ReloadEvent()
    {
        if (currentBullets < bulletsPerMagazine)
        {
            int necessaryBullets = 0;

            for (int i = 0; i < bulletsPerMagazine; i++)
            {
                if ((currentBullets + necessaryBullets) < bulletsPerMagazine)
                {
                    necessaryBullets++;
                }
            }

            if (extraBullets >= necessaryBullets)
            {
                currentBullets += necessaryBullets;
                extraBullets -= necessaryBullets;
            }
            else if (extraBullets < necessaryBullets)
            {
                necessaryBullets = extraBullets;
                currentBullets += necessaryBullets;
                extraBullets = 0;
            }
        }
    }

    public void RecoilEvent()
    {
        Vector3 pos = new Vector3(
            Random.Range(weaponRecoilPos.x / 2f, weaponRecoilPos.x),
            Random.Range(weaponRecoilPos.y / 2f, weaponRecoilPos.y),
            Random.Range(weaponRecoilPos.z / 2f, weaponRecoilPos.z)
        );

        Vector3 rot = new Vector3(
                Random.Range(weaponRecoilRot.x / 2f, weaponRecoilRot.x),
                Random.Range(weaponRecoilRot.y / 2f, weaponRecoilRot.y),
                Random.Range(weaponRecoilRot.z / 2f, weaponRecoilRot.z)
            );

        recoilTransform.localPosition += pos;
        recoilTransform.localRotation *= Quaternion.Euler(rot);

        PlayerCamera.ApplyRecoil(cameraRecoil);
    }

    public void AnimEvent(string eventName, bool stop = false)
    {
        string toUpper = eventName.ToUpper();
        Animator.Play(toUpper);
    }

    public void SoundEvent(string eventName, AudioClip clip = null)
    {
        if (clip == null)
        {
            string toLower = eventName.ToLower();
            AudioClip selectedSound = null;

            switch (toLower)
            {
                case "fire":
                    selectedSound = fireSound[Random.Range(0, fireSound.Length)];
                    break;

                case "no_bullets":
                    selectedSound = noBulletsSound;
                    break;

                case "start_reload":
                    selectedSound = startReloadSound;
                    break;

                case "middle_reload":
                    selectedSound = middleReloadSound;
                    break;

                case "end_reload":
                    selectedSound = endReloadSound;
                    break;
            }

            if (selectedSound)
            {
                Source.PlayOneShot(selectedSound);
            }
        }
        else
        {
            Source.PlayOneShot(clip);
        }
    }

    private IEnumerator DelayFire(float time, RaycastHit hit)
    {
        yield return new WaitForSeconds(time);

        // Force
        Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
        if (HitBody)
        {
            HitBody.AddForceAtPosition(firePoint.forward * bulletHitForce, hit.point);
        }

        // Bullet hole
        if (WeaponManager.GetImpactWithTag(hit.transform.tag) != null)
        {
            GameObject impact = Instantiate(WeaponManager.GetImpactWithTag(hit.transform.tag).prefab, hit.point,
            Quaternion.LookRotation(hit.normal));
            impact.transform.position += impact.transform.forward * 0.001f;

            if (hit.transform.tag.Equals("Enemy"))
            {
                WeaponManager.ApplyHitMark(PlayerCamera.WorldToScreen(hit.point));
            }

            Destroy(impact, 5f);
        }
    }

    private void OnDrawGizmos()
    {
        if (!disableGizmos)
        {
            if (firePoint)
            {
                Gizmos.color = Color.red;

                Vector3 point1 = firePoint.position;
                Vector3 predictedBulletVelocity = firePoint.forward * maxRange;
                float stepSize = 0.01f;

                for (float step = 0f; step < 1; step += stepSize)
                {
                    predictedBulletVelocity += Physics.gravity * stepSize;
                    Vector3 point2 = point1 + predictedBulletVelocity * stepSize;

                    Gizmos.DrawLine(point1, point2);

                    point1 = point2;
                }
            }
        }
    }
}
