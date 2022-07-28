using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WeaponState : StateBase
{
    public WeaponState()
    {
        states = new List<State>()
            {
                new State("Safety"),
                new State("Aiming"),
                new State("Reloading"),
                new State("Firing"),
                new State("Drawing"),
                new State("Hiding"),
            };
    }
}

public class Weapon : MonoBehaviour
{
    public WeaponPreset Preset;

    [Header("Data")]
    [SerializeField] private int bulletsPerMagazine = 12;
    [SerializeField] private int currentBullets = 12;
    [SerializeField] private int extraBullets = 24;

    private float drawTimer, hideTimer;
    private float firerateTimer;
    private float firingTimer;
    private float aimSensitivityScale;
    private Vector3 initialAimPos;
    private Vector3 initialRecoilPos;
    private Vector3 defaultAimPos;
    private Quaternion initialAimRot;
    private Quaternion initialRecoilRot;
    private Transform muzzlePoint;
    private Transform recoilRoot;
    private Transform defaultMuzzlePoint;
    private Animator Animator;
    private AudioSource Source;
    private WeaponEvents Events;
    private WeaponState States;

    public WeaponState GetStates => States;
    public Animator GetAnimator() => Animator;
    public void ResetAnimatorToNone() => Animator.Play("None");
    public bool HaveBullets() => currentBullets > 0;
    public void SetAimSensitivityScale(float scale) => aimSensitivityScale = scale;
    public void SetMuzzlePoint(Transform muzzle) => muzzlePoint = muzzle;
    public void SetAimPosition(Vector3 aimPos) => Preset.aimPosition = aimPos;
    public void MaxAimSensitivityScale() => aimSensitivityScale = 1f;
    public void ResetMuzzlePoint() => muzzlePoint = defaultMuzzlePoint;
    public void ResetAimPosition() => Preset.aimPosition = defaultAimPos;
    public void ChangeWeaponMode(WeaponPreset.WeaponMode mode) => Preset.weaponMode = mode;
    public void PlayAnimation(string eventName) => Animator.Play(eventName);

    private void Awake()
    {
        // Weapon settings
        States = new WeaponState();
    }

    private void Start()
    {
        // Components
        Events = transform.GetComponentInChildren<WeaponEvents>();
        Animator = transform.GetComponentInChildren<Animator>();
        Source = transform.GetComponent<AudioSource>();

        // Transforms
        muzzlePoint = FindManager.Find("MuzzlePoint");
        recoilRoot = FindManager.Find("WeaponRecoil");

        // Weapon events
        Events.SetWeapon(this);

        // Default
        defaultMuzzlePoint = muzzlePoint;
        defaultAimPos = Preset.aimPosition;

        // Aim
        initialAimPos = transform.localPosition;
        initialAimRot = transform.localRotation;
        MaxAimSensitivityScale();

        // Recoil
        initialRecoilPos = recoilRoot.localPosition;
        initialRecoilRot = recoilRoot.localRotation;
    }

    private void Update()
    {
        SwitchUpdate();
        Fire();
        Reload();
        Aim();
        Recoil();
        Mode();
    }

    private void SwitchUpdate()
    {
        States.SetState("Drawing", drawTimer > 0);
        States.SetState("Hiding", hideTimer > 0);

        if (drawTimer > 0)
        {
            drawTimer -= Time.deltaTime;
        }

        if (hideTimer > 0)
        {
            hideTimer -= Time.deltaTime;
        }
    }

    private void Fire()
    {
        bool conditions =
        (Preset.weaponMode == WeaponPreset.WeaponMode.Combat) &&
        !PlayerController.GetStates.GetState("Running") &&
        !States.GetState("Drawing") &&
        !States.GetState("Hiding") &&
        !States.GetState("Reloading") &&
        PlayerCamera.GetLockState();

        if (PlayerInput.WeaponFireTap)
        {
            if (conditions)
            {
                switch (Preset.fireMode)
                {
                    case WeaponPreset.FireMode.Semi:
                        firingTimer = Preset.firerate * 3f;
                        CalculateFire();
                        break;

                    case WeaponPreset.FireMode.Auto:
                        firingTimer = Preset.firerate * 3f;
                        CalculateFire();
                        break;
                }
            }
            Animator.SetBool("NoBullet", !HaveBullets());
        }

        // Is firing
        if (firingTimer >= 0)
        {
            States.SetState("Firing", true);
            firingTimer -= Time.deltaTime;
        }
        else
        {
            States.SetState("Firing", false);
        }

        if (firerateTimer >= 0)
        {
            firerateTimer -= Time.deltaTime;
        }
    }

    private void Reload()
    {
        bool conditions =
        (Preset.weaponMode == WeaponPreset.WeaponMode.Combat) &&
        !PlayerController.GetStates.GetState("Running") &&
        !States.GetState("Reloading") &&
        !States.GetState("Drawing") &&
        !States.GetState("Hiding") &&
        PlayerInput.WeaponReload &&
        PlayerCamera.GetLockState() &&
        extraBullets > 0 &&
        currentBullets < bulletsPerMagazine;

        if (conditions)
        {
            // Animation no-bullet
            PlayAnimation("Reload");
        }
    }

    private void Aim()
    {
        bool conditions =
        (Preset.weaponMode == WeaponPreset.WeaponMode.Combat) &&
        !PlayerController.GetStates.GetState("Running") &&
        !States.GetState("Reloading") &&
        !States.GetState("Drawing") &&
        !States.GetState("Hiding") &&
        PlayerInput.WeaponAim &&
        PlayerCamera.GetLockState();

        if (conditions)
        {
            States.SetState("Aiming", true);

            PlayerCamera.SetSensitivityScale(aimSensitivityScale);
            WeaponManager.Sway.SwayAccuracy(Preset.aimSwayScale);

            transform.localPosition = Vector3.Lerp(transform.localPosition, Preset.aimPosition, Preset.aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, Preset.aimRotation, Preset.aimSpeed * Time.deltaTime);
        }
        else
        {
            States.SetState("Aiming", false);
            PlayerCamera.MaxSensitivityScale();
        }

        bool resetConditions =
        ((!PlayerInput.WeaponAim || PlayerInput.Run) &&
        (transform.localPosition != initialAimPos ||
        transform.localRotation != initialAimRot) ||
        (Preset.weaponMode == WeaponPreset.WeaponMode.Safety)) ||
        States.GetState("Reloading");

        if (resetConditions)
        {
            WeaponManager.Sway.MaxAccuracy();
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialAimPos, Preset.aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, initialAimRot, Preset.aimSpeed * Time.deltaTime);
        }
    }
    private void Recoil()
    {
        if (recoilRoot.localPosition != initialRecoilPos || recoilRoot.localRotation != initialRecoilRot)
        {
            recoilRoot.localPosition = Vector3.Lerp(recoilRoot.localPosition, initialRecoilPos, Preset.recoilResetSpeed * Time.deltaTime);
            recoilRoot.localRotation = Quaternion.Slerp(recoilRoot.localRotation, initialRecoilRot, Preset.recoilResetSpeed * Time.deltaTime);
        }
    }

    private void Mode()
    {
        bool conditions =
        !States.GetState("Reloading") &&
        !States.GetState("Aiming") &&
        !States.GetState("Firing") &&
        !States.GetState("Drawing") &&
        !States.GetState("Hiding");

        if (PlayerInput.WeaponSafety)
        {
            switch (Preset.weaponMode)
            {
                case WeaponPreset.WeaponMode.Safety: Preset.weaponMode = WeaponPreset.WeaponMode.Combat; break;
                case WeaponPreset.WeaponMode.Combat: Preset.weaponMode = WeaponPreset.WeaponMode.Safety; break;
            }

            States.SetState("Safety", Preset.weaponMode == WeaponPreset.WeaponMode.Safety);
        }
    }

    public void PlaySound(string eventName, AudioClip clip = null, float volume = 1f)
    {
        if (clip == null)
        {
            string toLower = eventName.ToUpper();
            AudioClip selectedSound = null;

            switch (toLower)
            {
                case "FIRE":
                    selectedSound = Preset.fireSounds[Random.Range(0, Preset.fireSounds.Length)];
                    break;

                case "NO_BULLETS":
                    selectedSound = Preset.noBulletSound;
                    break;

                case "START_RELOAD":
                    selectedSound = Preset.initialReloadSound;
                    break;

                case "MIDDLE_RELOAD":
                    selectedSound = Preset.middleReloadSound;
                    break;

                case "END_RELOAD":
                    selectedSound = Preset.endReloadSound;
                    break;
            }

            if (selectedSound)
            {
                Source.PlayOneShot(selectedSound, volume);
            }
        }
        else
        {
            Source.PlayOneShot(clip, volume);
        }
    }

    public void CalculateFire()
    {
        if (currentBullets >= Preset.bulletsPerFire && firerateTimer <= 0)
        {
            // Tracer
            List<Vector3> positions = new List<Vector3>();
            LineRenderer tracer = GameObject.Instantiate(WeaponManager.GetTracerPrefab(), muzzlePoint.position, Quaternion.identity).GetComponent<LineRenderer>();
            Tracer tracerScript = tracer.gameObject.GetComponent<Tracer>();

            Vector3 point1 = muzzlePoint.position;
            Vector3 predictedBulletVelocity = muzzlePoint.forward * Preset.maxBulletDistance;
            float stepSize = 0.01f;

            // Tracer start position
            positions.Add(point1);

            for (float step = 0f; step < 1; step += stepSize)
            {
                predictedBulletVelocity += (Physics.gravity * stepSize) * Preset.bulletGravityScale;
                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;

                // Tracer positions
                positions.Add(point2);

                Ray ray = new Ray(point1, point2 - point1);
                RaycastHit hit;

                Debug.DrawLine(point1, point2, Color.green);

                if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, WeaponManager.GetHittableMask()))
                {
                    if (hit.transform)
                    {
                        float distance = (muzzlePoint.position - hit.point).sqrMagnitude;
                        float time = distance / (Preset.bulletVelocity * 1000f);
                        StartCoroutine(CalculateDelay(time, hit));
                        break;
                    }
                }

                point1 = point2;
            }

            // Set tracer positions
            tracerScript.pos = positions;

            ApplyRecoil();
            PlayAnimation("Fire");
            PlaySound("FIRE");

            currentBullets -= Preset.bulletsPerFire;
            firerateTimer = Preset.firerate;
        }
    }

    public IEnumerator CalculateDelay(float time, RaycastHit hit)
    {
        yield return new WaitForSeconds(time);

        // Force
        Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
        if (HitBody)
        {
            HitBody.AddForceAtPosition(muzzlePoint.forward * Preset.bulletHitForce, hit.point);
        }

        // Hitmark in enemy
        if (hit.transform.tag.Equals("Enemy"))
        {
            WeaponManager.Hitmark.ApplyHitMark(PlayerCamera.WorldToScreen(hit.point));
            PlaySound("", WeaponManager.Hitmark.GetHitMarkSound(), 0.3f);
        }

        // Bullet hole
        if (WeaponManager.Impacts.GetImpactWithTag(hit.transform.tag) != null)
        {
            GameObject impact = GameObject.Instantiate(WeaponManager.Impacts.GetImpactWithTag(hit.transform.tag).prefab, hit.point,
            Quaternion.LookRotation(hit.normal));
            impact.transform.position += impact.transform.forward * 0.001f;

            MonoBehaviour.Destroy(impact, 5f);
        }
    }

    public void ApplyRecoil()
    {
        Vector3 pos = new Vector3(
            Random.Range(Preset.recoilForcePos.x / 2f, Preset.recoilForcePos.x),
            Random.Range(Preset.recoilForcePos.y / 2f, Preset.recoilForcePos.y),
            Random.Range(Preset.recoilForcePos.z / 2f, Preset.recoilForcePos.z));

        Vector3 rot = new Vector3(
            Random.Range(Preset.recoilForceRot.x / 2f, Preset.recoilForceRot.x),
            Random.Range(Preset.recoilForceRot.y / 2f, Preset.recoilForceRot.y),
            Random.Range(Preset.recoilForceRot.z / 2f, Preset.recoilForceRot.z));

        recoilRoot.localPosition += pos;
        recoilRoot.localRotation *= Quaternion.Euler(rot);

        PlayerCamera.ApplyRecoil(Preset.recoilForceCamera);
    }

    public void CalculateReload()
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

    private void OnEnable()
    {
        // Setting switch values
        drawTimer = Preset.drawTime;
        hideTimer = Preset.hideTime;
    }

    private void OnDrawGizmos()
    {
        if (muzzlePoint)
        {
            Gizmos.color = Color.red;
            Vector3 point1 = muzzlePoint.position;
            Vector3 predictedBulletVelocity = muzzlePoint.forward * Preset.maxBulletDistance;
            float stepSize = 0.01f;

            for (float step = 0f; step < 1; step += stepSize)
            {
                predictedBulletVelocity += (Physics.gravity * stepSize) * Preset.bulletGravityScale;
                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                Gizmos.DrawLine(point1, point2);
                point1 = point2;
            }
        }
    }
}