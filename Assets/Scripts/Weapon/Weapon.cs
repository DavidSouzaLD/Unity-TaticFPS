using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum FireMode { Semi, Auto }

    [Header("Settings")]

    /// <summary>
    /// Defines what types of objects can be hit by the layer-based shot.
    /// </summary>
    [SerializeField] private LayerMask hittableLayers;

    /// <summary>
    /// Sets the fire type.
    /// </summary>
    [SerializeField] private FireMode fireMode;

    /// <summary>
    /// Position of the gun barrel where the raycast is launched.
    /// </summary>
    [SerializeField] private Transform muzzlePoint;

    /// <summary>
    /// Time needed for next shot.
    /// </summary>
    [SerializeField] private float firerate = 1f;

    [Header("Data")]

    /// <summary>
    /// Defines the maximum number of bullets you can have in the magazine.
    /// </summary>
    [SerializeField] private int bulletsPerMagazine = 12;

    /// <summary>
    /// How many bullets are in the current magazine.
    /// </summary>
    [SerializeField] private int currentBullets = 12;

    /// <summary>
    /// How many bullets are stored?
    /// </summary>
    [SerializeField] private int extraBullets = 24;

    [Header("Bullet")]

    /// <summary>
    /// Bullet force on objects with physics.
    /// </summary>
    [SerializeField] private float bulletHitForce = 100f;

    /// <summary>
    /// Bullet max velocity.
    /// </summary>
    [SerializeField] private float bulletVelocity = 25f;

    /// <summary>
    /// How many bullets are instantiated with each shot.
    /// </summary>
    [SerializeField] private int bulletsPerFire = 1;

    /// <summary>
    /// Gravity scale applied to bullet after effective distance.
    /// </summary>
    [SerializeField, Range(0, 20)] private float bulletGravityScale = 1;

    /// <summary>
    /// Farthest distance a bullet can travel.
    /// </summary>
    [SerializeField] private float maxBulletDistance;

    [Header("Aim")]

    /// <summary>
    /// Speed ​​it takes the weapon to reach the aiming point.
    /// </summary>
    [SerializeField] private float aimSpeed = 5f;

    /// <summary>
    /// Multiplies the total value of the sway when aiming.
    /// </summary>
    [SerializeField] private float aimSwayScale = 0.2f;

    /// <summary>
    /// Exact weapon position while aiming.
    /// </summary>
    [SerializeField] private Vector3 aimPosition;

    /// <summary>
    /// Exact weapon rotation while aiming.
    /// </summary>
    [SerializeField] private Quaternion aimRotation;

    [Header("Recoil")]

    /// <summary>
    /// Speed ​​to reset to previous position.
    /// </summary>
    [SerializeField] private float recoilResetSpeed = 5f;

    /// <summary>
    /// Kickback force applied to the position.
    /// </summary>
    [SerializeField] private Vector3 recoilForcePos;

    /// <summary>
    /// Kickback force applied to rotation.
    /// </summary>
    [SerializeField] private Vector3 recoilForceRot;

    /// <summary>
    /// Recoil force applied to the camera.
    /// </summary>
    [SerializeField] private Vector3 recoilForceCamera;

    [Header("Sounds")]

    /// <summary>
    /// Fire sounds clips.
    /// </summary>
    [SerializeField] private AudioClip[] fireSounds;

    /// <summary>
    /// Sound if not bullets.
    /// </summary>
    [SerializeField] private AudioClip noBulletSound;

    /// <summary>
    /// Reload sounds.
    /// </summary>
    [SerializeField] private AudioClip startReloadSound, middleReloadSound, endReloadSound;

    /// <summary>
    /// Returns weapon states.
    /// </summary>
    [HideInInspector] public bool isReloading, isAim, isInspect;

    // Default
    private Transform defaultMuzzlePoint;
    private Vector3 defaultAimPos;

    // Privates
    private float timerDraw;
    private float firerateTimer; // Fire rate counter.
    private float aimSensitivityScale; // Scales and multiplies the total current sensitivity. (0 to 1) 
    private Vector3 startAimPos; // Initial position of the aim root.
    private Vector3 startRecoilPos; // Initial position of the recoil root.
    private Quaternion startAimRot; // Initial rotation of the aim root.
    private Quaternion startRecoilRot; // Initial rotation of recoil root.
    private Transform recoilRoot; // The root to which the recoil force is applied.
    private WeaponEvents Events; // Animation events.
    private Animator Animator; // Animation component.
    private AudioSource Source; // Component responsible for the audio.

    /// <summary>
    /// Returns if there is a bullet in the current magazine.
    /// </summary>
    public bool HaveBullets
    {
        get
        {
            return currentBullets > 0;
        }
    }

    /// <summary>
    /// Returns weapon animator.
    /// </summary>
    public Animator GetAnimator
    {
        get
        {
            return Animator;
        }
    }

    public void SetAimSensitivityScale(float scale) => aimSensitivityScale = scale;
    public void SetMuzzlePoint(Transform muzzle) => muzzlePoint = muzzle;
    public void SetAimPosition(Vector3 aimPos) => aimPosition = aimPos;
    public void MaxAimSensitivityScale() => aimSensitivityScale = 1f;
    public void ResetMuzzlePoint() => muzzlePoint = defaultMuzzlePoint;
    public void ResetAimPosition() => aimPosition = defaultAimPos;

    private void Start()
    {
        // Components
        Events = GetComponentInChildren<WeaponEvents>();
        Animator = GetComponentInChildren<Animator>();
        Source = GetComponent<AudioSource>();

        // Weapon events
        Events.SetWeapon(this);

        // Default
        defaultMuzzlePoint = muzzlePoint;
        defaultAimPos = aimPosition;

        // Aim
        startAimPos = transform.localPosition;
        startAimRot = transform.localRotation;
        MaxAimSensitivityScale();

        // Recoil
        recoilRoot = FindManager.Find("WeaponRecoil", this);
        startRecoilPos = recoilRoot.localPosition;
        startRecoilRot = recoilRoot.localRotation;

        // Error
        if (recoilRoot == null)
        {
            DebugManager.DebugAssignedError("RecoilRoot");
        }
    }

    private void Update()
    {
        Fire();
        Reload();
        Aim();
        Recoil();
    }

    private void Fire()
    {
        bool canFire = !Player.isRunning && !isReloading;

        if (canFire)
        {
            // Fire
            switch (fireMode)
            {
                case FireMode.Semi:

                    if (InputManager.FireTap)
                    {
                        CalculateFire();
                    }

                    break;

                case FireMode.Auto:

                    if (InputManager.FireAuto)
                    {
                        CalculateFire();
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
    private void Reload()
    {
        bool canReload = InputManager.Reload && PlayerCamera.IsCursorLocked() && !Player.isRunning && extraBullets > 0 && currentBullets < bulletsPerMagazine && !isReloading;

        if (canReload)
        {
            PlayAnimation("RELOAD");
        }

        // Animation no-bullet
        Animator.SetBool("NO_BULLET", !HaveBullets);
    }

    private void Aim()
    {
        bool canAim = InputManager.Aim && PlayerCamera.IsCursorLocked() && !isReloading && !Player.isRunning;

        Player.isAim = isAim;

        if (canAim)
        {
            isAim = true;

            PlayerCamera.SetSensitivityScale(aimSensitivityScale);
            WeaponManager.SwayAccuracy(aimSwayScale);

            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, aimRotation, aimSpeed * Time.deltaTime);
        }
        else
        {
            isAim = false;
            PlayerCamera.MaxSensitivityScale();
        }

        bool canResetAim = (!InputManager.Aim || InputManager.Run) && (transform.localPosition != startAimPos || transform.localRotation != startAimRot);

        if (canResetAim)
        {
            WeaponManager.MaxAccuracy();
            transform.localPosition = Vector3.Lerp(transform.localPosition, startAimPos, aimSpeed * Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, startAimRot, aimSpeed * Time.deltaTime);
        }
    }

    private void Recoil()
    {
        // Reset recoil
        if (recoilRoot.localPosition != startRecoilPos || recoilRoot.localRotation != startRecoilRot)
        {
            recoilRoot.localPosition = Vector3.Lerp(recoilRoot.localPosition, startRecoilPos, recoilResetSpeed * Time.deltaTime);
            recoilRoot.localRotation = Quaternion.Slerp(recoilRoot.localRotation, startRecoilRot, recoilResetSpeed * Time.deltaTime);
        }
    }

    private void CalculateFire()
    {
        if (currentBullets >= bulletsPerFire && firerateTimer <= 0)
        {
            // Tracer
            List<Vector3> positions = new List<Vector3>();
            LineRenderer tracer = Instantiate(WeaponManager.GetTracerPrefab, muzzlePoint.position, Quaternion.identity).GetComponent<LineRenderer>();
            Tracer tracerScript = tracer.gameObject.GetComponent<Tracer>();

            Vector3 point1 = muzzlePoint.position;
            Vector3 predictedBulletVelocity = muzzlePoint.forward * maxBulletDistance;
            float stepSize = 0.01f;

            // Tracer start position
            positions.Add(point1);

            for (float step = 0f; step < 1; step += stepSize)
            {
                predictedBulletVelocity += (Physics.gravity * stepSize) * bulletGravityScale;
                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;

                // Tracer positions
                positions.Add(point2);

                Ray ray = new Ray(point1, point2 - point1);
                RaycastHit hit;

                Debug.DrawLine(point1, point2, Color.green);

                if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, hittableLayers))
                {
                    if (hit.transform)
                    {
                        float distance = (muzzlePoint.position - hit.point).sqrMagnitude;
                        float time = distance / (bulletVelocity * 1000f);
                        StartCoroutine(CalculateDelay(time, hit));
                        break;
                    }
                }

                point1 = point2;
            }

            // Set tracer positions
            tracerScript.pos = positions;

            // Apply recoil
            Vector3 pos = new Vector3(
            Random.Range(recoilForcePos.x / 2f, recoilForcePos.x),
            Random.Range(recoilForcePos.y / 2f, recoilForcePos.y),
            Random.Range(recoilForcePos.z / 2f, recoilForcePos.z));

            Vector3 rot = new Vector3(
            Random.Range(recoilForceRot.x / 2f, recoilForceRot.x),
            Random.Range(recoilForceRot.y / 2f, recoilForceRot.y),
            Random.Range(recoilForceRot.z / 2f, recoilForceRot.z));

            recoilRoot.localPosition += pos;
            recoilRoot.localRotation *= Quaternion.Euler(rot);

            PlayerCamera.ApplyRecoil(recoilForceCamera);

            // Apply feel
            PlayAnimation("FIRE");
            PlaySound("FIRE");

            currentBullets -= bulletsPerFire;
            firerateTimer = firerate;
        }
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

    private IEnumerator CalculateDelay(float time, RaycastHit hit)
    {
        yield return new WaitForSeconds(time);

        // Force
        Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
        if (HitBody)
        {
            HitBody.AddForceAtPosition(muzzlePoint.forward * bulletHitForce, hit.point);
        }

        // Hitmark in enemy
        if (hit.transform.tag.Equals("Enemy"))
        {
            WeaponManager.ApplyHitMark(PlayerCamera.WorldToScreen(hit.point));
            PlaySound("", WeaponManager.GetHitMarkSound(), 0.3f);
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

    public void PlayAnimation(string eventName)
    {
        string toUpper = eventName.ToUpper();
        Animator.Play(toUpper);
    }

    public void PlaySound(string eventName, AudioClip clip = null, float volume = 1f)
    {
        if (clip == null)
        {
            string toLower = eventName.ToLower();
            AudioClip selectedSound = null;

            switch (toLower)
            {
                case "fire":
                    selectedSound = fireSounds[Random.Range(0, fireSounds.Length)];
                    break;

                case "no_bullets":
                    selectedSound = noBulletSound;
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
                Source.PlayOneShot(selectedSound, volume);
            }
        }
        else
        {
            Source.PlayOneShot(clip, volume);
        }
    }

    private void OnDrawGizmos()
    {
        if (muzzlePoint)
        {
            Gizmos.color = Color.red;
            Vector3 point1 = muzzlePoint.position;
            Vector3 predictedBulletVelocity = muzzlePoint.forward * maxBulletDistance;
            float stepSize = 0.01f;

            for (float step = 0f; step < 1; step += stepSize)
            {
                predictedBulletVelocity += (Physics.gravity * stepSize) * bulletGravityScale;
                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                Gizmos.DrawLine(point1, point2);
                point1 = point2;
            }
        }
    }
}