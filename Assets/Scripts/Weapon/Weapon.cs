using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Character;
using Game.Utilities;

namespace Game.Weapon
{
    public class WeaponState : States
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

    [DisallowMultipleComponent]
    public class Weapon : MonoBehaviour
    {
        public WeaponPreset Preset;

        [Header("Data")]
        [SerializeField] private int bulletsPerMagazine = 12;
        [SerializeField] private int currentBullets = 12;
        [SerializeField] private int extraBullets = 24;

        [Header("Roots")]
        [SerializeField] private Transform muzzleRoot;

        // Private
        private float drawTimer, hideTimer;
        private float firerateTimer;
        private float firingTimer;
        private float aimSensitivityScale;
        private Vector3 initialAimPos;
        private Quaternion initialAimRot;
        private Vector3 defaultAimPos;
        private Transform defaultmuzzleRoot;

        // Weapon components
        private WeaponSound Sound;
        private WeaponAnimation Anim;
        private WeaponState States;

        // Delegate
        public delegate void Firing();
        public delegate void SafetyChanged();
        public delegate void StartReload();
        public delegate void EndReload();

        // Delegate callbacks
        public Firing OnFiring;
        public SafetyChanged OnSafetyChanged;
        public StartReload OnStartReload;
        public EndReload OnEndReload;

        public bool HaveBullets()
        {
            return currentBullets > 0;
        }

        public void ChangeWeaponMode(WeaponPreset.WeaponMode _mode)
        {
            Preset.weaponMode = _mode;
        }

        public void SetAimSensitivityScale(float _scale)
        {
            aimSensitivityScale = _scale;
        }

        public void MaxAimSensitivityScale()
        {
            aimSensitivityScale = 1f;
        }

        public void SetAimPosition(Vector3 _aimPos)
        {
            Preset.aimPosition = _aimPos;
        }

        public void ResetAimPosition()
        {
            Preset.aimPosition = defaultAimPos;
        }

        public void SetMuzzleRoot(Transform _muzzle)
        {
            muzzleRoot = _muzzle;
        }

        public void ResetMuzzleRoot()
        {
            muzzleRoot = defaultmuzzleRoot;
        }

        public bool GetState(string _stateName)
        {
            return States.GetState(_stateName);
        }

        public void SetState(string _stateName, bool _value)
        {
            States.SetState(_stateName, _value);
        }

        private void Awake()
        {
            // Create state
            States = new WeaponState();
        }

        private void Start()
        {
            // Animation
            Sound = GetComponentInChildren<WeaponSound>();
            Anim = GetComponentInChildren<WeaponAnimation>();
            Sound.Init(this);
            Anim.Init(this, Sound);

            // Default
            defaultmuzzleRoot = muzzleRoot;
            defaultAimPos = Preset.aimPosition;

            // Aim
            initialAimPos = transform.localPosition;
            initialAimRot = transform.localRotation;
            MaxAimSensitivityScale();

            // Delegates
            OnFiring?.Invoke();
            OnSafetyChanged?.Invoke();
        }

        private void Update()
        {
            SwitchUpdate();
            FireUpdate();
            ReloadUpdate();
            AimUpdate();
            ModeUpdate();
        }

        private void SwitchUpdate()
        {
            SetState("Drawing", drawTimer > 0);
            SetState("Hiding", hideTimer > 0);

            if (drawTimer > 0)
            {
                drawTimer -= Time.deltaTime;
            }

            if (hideTimer > 0)
            {
                hideTimer -= Time.deltaTime;
            }
        }

        private void FireUpdate()
        {
            // Conditions
            bool inputConditions = Systems.Input.GetBool("WeaponFireTap");
            bool modeConditions = (Preset.weaponMode == WeaponPreset.WeaponMode.Combat);
            bool playerConditions = !FPSCharacterController.GetState("Running") && CharacterCamera.GetState("CursorLocked");
            bool stateConditions = !GetState("Drawing") && !GetState("Hiding") && !GetState("Reloading");
            bool conditions = inputConditions && modeConditions && playerConditions && stateConditions;

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

            // Is firing
            if (firingTimer >= 0)
            {
                SetState("Firing", true);
                firingTimer -= Time.deltaTime;
            }
            else
            {
                SetState("Firing", false);
            }

            if (firerateTimer >= 0)
            {
                firerateTimer -= Time.deltaTime;
            }
        }

        private void ReloadUpdate()
        {
            bool inputConditions = Systems.Input.GetBool("WeaponReload");
            bool modeConditions = (Preset.weaponMode == WeaponPreset.WeaponMode.Combat);
            bool playerConditions = !FPSCharacterController.GetState("Running") && CharacterCamera.GetState("CursorLocked");
            bool stateConditions = !GetState("Reloading") && !GetState("Drawing") && !GetState("Hiding");
            bool reloadConditions = extraBullets > 0 && currentBullets < bulletsPerMagazine;
            bool conditions = inputConditions && modeConditions && playerConditions && stateConditions && reloadConditions;

            if (conditions)
            {
                // Animation no-bullet
                Anim.Play("Reload");
            }
        }

        private void AimUpdate()
        {
            bool inputConditions = Systems.Input.GetBool("WeaponAim");
            bool modeConditions = (Preset.weaponMode == WeaponPreset.WeaponMode.Combat);
            bool playerConditions = !FPSCharacterController.GetState("Running") && CharacterCamera.GetState("CursorLocked");
            bool stateConditions = !GetState("Reloading") && !GetState("Drawing") && !GetState("Hiding");
            bool conditions = inputConditions && modeConditions && playerConditions && stateConditions;

            if (conditions)
            {
                SetState("Aiming", true);

                CharacterCamera.SetSensitivityScale(aimSensitivityScale);
                WeaponSway.SwayAccuracy(Preset.aimSwayScale);

                transform.localPosition = Vector3.Lerp(transform.localPosition, Preset.aimPosition, Preset.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, Preset.aimRotation, Preset.aimSpeed * Time.deltaTime);
            }
            else
            {
                SetState("Aiming", false);
                CharacterCamera.MaxSensitivityScale();
            }

            bool differenceConditions = (transform.localPosition != initialAimPos || transform.localRotation != initialAimRot);
            bool resetConditions = !conditions && differenceConditions;

            if (resetConditions)
            {
                WeaponSway.MaxAccuracy();
                transform.localPosition = Vector3.Lerp(transform.localPosition, initialAimPos, Preset.aimSpeed * Time.deltaTime);
                transform.localRotation = Quaternion.Slerp(transform.localRotation, initialAimRot, Preset.aimSpeed * Time.deltaTime);
            }
        }

        private void ModeUpdate()
        {
            bool inputConditions = Systems.Input.GetBool("WeaponSafety");
            bool stateConditions = !GetState("Reloading") && !GetState("Aiming") && !GetState("Firing") && !GetState("Drawing") && !GetState("Hiding");
            bool conditions = inputConditions && stateConditions;

            if (conditions)
            {
                switch (Preset.weaponMode)
                {
                    case WeaponPreset.WeaponMode.Safety: Preset.weaponMode = WeaponPreset.WeaponMode.Combat; break;
                    case WeaponPreset.WeaponMode.Combat: Preset.weaponMode = WeaponPreset.WeaponMode.Safety; break;
                }

                SetState("Safety", Preset.weaponMode == WeaponPreset.WeaponMode.Safety);

                // Delegate
                OnSafetyChanged?.Invoke();
            }
        }

        public void CalculateFire()
        {
            if (currentBullets >= Preset.bulletsPerFire && firerateTimer <= 0)
            {
                // Tracer
                List<Vector3> tracerPositions = new List<Vector3>();
                LineRenderer tracer = GameObject.Instantiate(WeaponManager.GetTracerPrefab(), muzzleRoot.position, Quaternion.identity).GetComponent<LineRenderer>();
                WeaponTracer tracerScript = tracer.gameObject.GetComponent<WeaponTracer>();

                Vector3 point1 = muzzleRoot.position;
                Vector3 predictedBulletVelocity = muzzleRoot.forward * Preset.maxBulletDistance;
                float stepSize = 0.01f;

                // Tracer start position
                tracerPositions.Add(point1);

                for (float step = 0f; step < 1; step += stepSize)
                {
                    predictedBulletVelocity += (Physics.gravity * stepSize) * Preset.bulletGravityScale;
                    Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                    tracerPositions.Add(point2);

                    Ray ray = new Ray(point1, point2 - point1);
                    RaycastHit hit;

                    Debug.DrawLine(point1, point2, Color.green);

                    if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, WeaponManager.GetHittableMask()))
                    {
                        if (hit.transform)
                        {
                            float distance = (muzzleRoot.position - hit.point).sqrMagnitude;
                            float time = distance / (Preset.bulletVelocity * 1000f);
                            StartCoroutine(CalculateDelay(time, hit));
                            break;
                        }
                    }

                    point1 = point2;
                }

                tracerScript.pos = tracerPositions;

                // Recoil
                WeaponRecoil.ApplyRecoil(Preset.recoilForcePos, Preset.recoilForceRot, Preset.recoilForceCam);

                // Animation / Sound
                Anim.Play("Fire");
                Sound.Play("Fire");

                // Removing bullets
                currentBullets -= Preset.bulletsPerFire;
                firerateTimer = Preset.firerate;

                // Delegate
                OnFiring?.Invoke();
            }
        }

        public IEnumerator CalculateDelay(float time, RaycastHit hit)
        {
            yield return new WaitForSeconds(time);

            // Force
            Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
            if (HitBody)
            {
                HitBody.AddForceAtPosition(muzzleRoot.forward * Preset.bulletHitForce, hit.point);
            }

            // Hitmark in enemy
            if (hit.transform.tag.Equals("Enemy"))
            {
                WeaponHitmark.ApplyHitMark(CharacterCamera.WorldToScreen(hit.point));
                Sound.Play("Hitmark", 0.3f);
            }

            // Bullet hole
            if (WeaponImpacts.GetImpactWithTag(hit.transform.tag) != null)
            {
                GameObject impact = GameObject.Instantiate(WeaponImpacts.GetImpactWithTag(hit.transform.tag).prefab, hit.point,
                Quaternion.LookRotation(hit.normal));
                impact.transform.position += impact.transform.forward * 0.001f;

                MonoBehaviour.Destroy(impact, 5f);
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

        private void OnEnable()
        {
            // Setting switch values
            drawTimer = Preset.drawTime;
            hideTimer = Preset.hideTime;
        }

        private void OnDrawGizmos()
        {
            if (Preset != null && muzzleRoot != null)
            {
                Gizmos.color = Color.red;
                Vector3 point1 = muzzleRoot.position;
                Vector3 predictedBulletVelocity = muzzleRoot.forward * Preset.maxBulletDistance;
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
}