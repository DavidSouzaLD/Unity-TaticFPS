using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WeaponSystem : MonoBehaviour
{
    public enum FireMode { Semi, Auto, Gust }
    [SerializeField] private bool disableGizmos;

    [Header("[Weapon Data]")]
    public int bulletsPerMagazine = 12;
    public int currentBullets = 12;
    public int extraBullets = 24;

    [Header("[Weapon Settings]")]
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private FireMode fireMode;
    [SerializeField] private Transform firePoint;

    [Header("[Fire Settings]")]
    [SerializeField] private float firerate = 1f;
    [SerializeField] private float maxRange = 100f;
    [SerializeField] private float bulletHitForce = 100f;
    [SerializeField] private float bulletVelocity = 25f;
    [SerializeField] private int bulletsPerFire = 1;
    float _firerateTimer;

    [Header("[Aim Settings]")]
    [SerializeField] private float aimSpeed = 5f;
    [SerializeField] private float aimWeaponSwayAccuracy = 0.2f;
    [SerializeField] private Vector3 aimPosition;
    [SerializeField] private Quaternion aimRotation;
    Vector3 _startAimPos;
    Quaternion _startAimRot;

    [Header("[Recoil Settings]")]
    [SerializeField] private Transform recoilTransform;
    [SerializeField] private Vector3 weaponRecoilPos;
    [SerializeField] private Vector3 weaponRecoilRot;
    [SerializeField] private Vector3 cameraRecoil;
    [Space]
    [SerializeField] private float resetRecoil = 5f;
    Vector3 _startRecoilPos;
    Quaternion _startRecoilRot;

    [Header("[Sounds Settings]")]
    [SerializeField] private AudioClip[] fireSound;
    [SerializeField] private AudioClip noBulletsSound;
    [SerializeField] private AudioClip startReloadSound;
    [SerializeField] private AudioClip middleReloadSound;
    [SerializeField] private AudioClip endReloadSound;

    bool changeLockCursor;

    [HideInInspector] public bool isReloading, isAim, isCustom, isInspect;
    [HideInInspector] public WeaponManager WeaponManager;
    [HideInInspector] public WeaponSway WeaponSway;
    [HideInInspector] public WeaponHorizontalMove WeaponHorizontalMove;
    [HideInInspector] public Player Player;
    [HideInInspector] public NightVision NightVision;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public AudioSource Source;

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
        WeaponSway = GetComponentInParent<WeaponSway>();
        WeaponManager = GetComponentInParent<WeaponManager>();
        WeaponHorizontalMove = GetComponentInParent<WeaponHorizontalMove>();
        Player = GetComponentInParent<Player>();
        NightVision = GameObject.FindObjectOfType<NightVision>();
        Animator = GetComponentInChildren<Animator>();
        Source = GetComponent<AudioSource>();

        // Aim
        _startAimPos = transform.localPosition;
        _startAimRot = transform.localRotation;

        // Recoil
        _startRecoilPos = recoilTransform.localPosition;
        _startRecoilRot = recoilTransform.localRotation;
    }

    private void Update()
    {
        FireUpdate();
        ReloadUpdate();
        RecoilUpdate();
        AimUpdate();
        CustomUpdate();
        InspectUpdate();
    }

    private void FireUpdate()
    {
        bool _canFire = !Player.isRunning && !isReloading && Player.Camera.CursorLocked;

        if (_canFire)
        {
            // Fire
            switch (fireMode)
            {
                case FireMode.Semi:

                    if (PlayerInput.Keys.FireTap)
                    {
                        FireEvent();
                    }

                    break;

                case FireMode.Auto:

                    if (PlayerInput.Keys.FireAuto)
                    {
                        FireEvent();
                    }

                    break;

                case FireMode.Gust:

                    if (PlayerInput.Keys.FireTap)
                    {
                        for (int i = 0; i < bulletsPerFire; i++)
                        {
                            FireEvent();
                        }
                    }

                    break;
            }

            if (_firerateTimer >= 0)
            {
                _firerateTimer -= Time.deltaTime;
            }
        }
        else
        {
            isAim = false;
        }
    }

    private void ReloadUpdate()
    {
        bool _canReload = PlayerInput.Keys.Reload && Player.Camera.CursorLocked && !Player.isRunning && extraBullets > 0 && currentBullets < bulletsPerMagazine && !isReloading;

        if (_canReload)
        {
            AnimEvent("RELOAD");
        }

        // Animation no-bullet
        Animator.SetBool("NOBULLET", !HaveBulletsInMagazine);
    }

    private void RecoilUpdate()
    {
        if (recoilTransform.localPosition != _startRecoilPos || recoilTransform.localRotation != _startRecoilRot)
        {
            recoilTransform.localPosition = Vector3.Lerp(recoilTransform.localPosition, _startRecoilPos, resetRecoil * Time.deltaTime);
            recoilTransform.localRotation = Quaternion.Slerp(recoilTransform.localRotation, _startRecoilRot, resetRecoil * Time.deltaTime);
        }
    }

    private void AimUpdate()
    {
        bool _canAim = PlayerInput.Keys.Aim && Player.Camera.CursorLocked && !isReloading && !Player.isRunning;

        if (_canAim)
        {
            isAim = true;
            WeaponSway.SetAccuracy(aimWeaponSwayAccuracy);
            WeaponHorizontalMove.SetAccuracy(aimWeaponSwayAccuracy);
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, aimSpeed * Time.deltaTime);
        }
        else
        {
            isAim = false;
        }

        bool _canResetAim = (!PlayerInput.Keys.Aim || PlayerInput.Keys.Run) && (transform.localPosition != _startAimPos || transform.localRotation != _startAimRot);

        if (_canResetAim)
        {
            WeaponSway.SetMaxAccuracy();
            WeaponHorizontalMove.SetMaxAccuracy();
            transform.localPosition = Vector3.Lerp(transform.localPosition, _startAimPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, _startAimRot, aimSpeed * Time.deltaTime);
        }
    }

    private void CustomUpdate()
    {
        bool _canCustom = PlayerInput.Keys.Custom && !NightVision.Enabled && HaveBulletsInMagazine && !isReloading && !isAim;

        if (_canCustom)
        {
            if (!changeLockCursor)
            {
                isCustom = true;

                Animator.SetBool("CUSTOM", isCustom);
                Player.Camera.ApplyVolume("CUSTOM");

                // Locks
                Player.Camera.LockCursor(false);
                Player.Lock(Player.LockType.Movement, "WEAPON_MOVEMENT", true);
                Player.Lock(Player.LockType.Jump, "WEAPON_JUMP", true);
                Player.Lock(Player.LockType.Jump, "WEAPON_TURN", true);

                changeLockCursor = true;
            }
        }
        else
        {
            if (changeLockCursor)
            {
                isCustom = false;

                Animator.SetBool("CUSTOM", isCustom);
                Player.Camera.ApplyVolume("BASE");

                // Locks
                Player.Camera.LockCursor(true);
                Player.Lock(Player.LockType.Movement, "WEAPON_MOVEMENT", false);
                Player.Lock(Player.LockType.Jump, "WEAPON_JUMP", false);
                Player.Lock(Player.LockType.Jump, "WEAPON_TURN", false);

                changeLockCursor = false;
            }
        }
    }

    private void InspectUpdate()
    {
        bool _canInspect = PlayerInput.Keys.Inspect && !isReloading && !isAim && !isCustom;

        if (_canInspect)
        {
            AnimEvent("INSPECT");
        }
    }

    private void FireEvent()
    {
        bool _canFire = currentBullets >= bulletsPerFire && _firerateTimer <= 0;

        if (_canFire)
        {

            Vector3 point1 = firePoint.position;
            Vector3 predictedBulletVelocity = firePoint.forward * maxRange;
            float stepSize = 0.01f;

            for (float step = 0f; step < 1; step += stepSize)
            {
                predictedBulletVelocity += Physics.gravity * stepSize;
                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;

                Ray ray = new Ray(point1, point2 - point1);
                RaycastHit hit;

                Debug.DrawLine(point1, point2, Color.green);

                if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, targetLayers))
                {
                    if (hit.transform)
                    {
                        StartCoroutine(DelayFire(((firePoint.position - hit.point).sqrMagnitude / bulletVelocity), hit));
                        break;
                    }
                }

                point1 = point2;
            }

            RecoilEvent();
            AnimEvent("FIRE");
            SoundEvent("FIRE");

            currentBullets -= bulletsPerFire;
            _firerateTimer = firerate;
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

        Player.Camera.ApplyRecoil(cameraRecoil);
    }

    public void AnimEvent(string _eventName, bool _stop = false)
    {
        string toUpper = _eventName.ToUpper();
        Animator.Play(toUpper);
    }

    public void SoundEvent(string _eventName, AudioClip _clip = null)
    {
        if (_clip == null)
        {
            string toLower = _eventName.ToLower();
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
            Source.PlayOneShot(_clip);
        }
    }

    private IEnumerator DelayFire(float _time, RaycastHit hit)
    {
        yield return new WaitForSeconds(_time);
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

                    Ray ray = new Ray(point1, point2 - point1);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude))
                    {
                        Gizmos.color = Color.blue;
                        Gizmos.DrawSphere(hit.point, 0.1f);
                    }

                    point1 = point2;
                }
            }
        }
    }
}