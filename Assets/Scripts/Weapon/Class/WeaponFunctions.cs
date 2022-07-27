using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class WeaponFunctions
{
    private Weapon Weapon;

    public WeaponFunctions(Weapon _weapon)
    {
        Weapon = _weapon;
    }

    public void Start()
    {
        Transform transform = Weapon.transform;

        // Components
        Weapon.Events = transform.GetComponentInChildren<WeaponEvents>();
        Weapon.Animator = transform.GetComponentInChildren<Animator>();
        Weapon.Source = transform.GetComponent<AudioSource>();

        // Weapon events
        Weapon.Events.SetWeapon(Weapon);

        // Default
        Weapon.defaultMuzzlePoint = Weapon.muzzlePoint;
        Weapon.defaultAimPos = Weapon.aimPosition;

        // Aim
        Weapon.initialAimPos = transform.localPosition;
        Weapon.initialAimRot = transform.localRotation;
        MaxAimSensitivityScale();

        // Recoil
        Weapon.recoilRoot = FindManager.Find("WeaponRecoil");
        Weapon.initialRecoilPos = Weapon.recoilRoot.localPosition;
        Weapon.initialRecoilRot = Weapon.recoilRoot.localRotation;
    }

    public Animator GetAnimator()
    {
        return Weapon.Animator;
    }

    public bool HaveBullets()
    {
        return Weapon.currentBullets > 0;
    }

    public void SetAimSensitivityScale(float scale)
    {
        Weapon.aimSensitivityScale = scale;
    }

    public void SetMuzzlePoint(Transform muzzle)
    {
        Weapon.muzzlePoint = muzzle;
    }

    public void SetAimPosition(Vector3 aimPos)
    {
        Weapon.aimPosition = aimPos;
    }

    public void MaxAimSensitivityScale()
    {
        Weapon.aimSensitivityScale = 1f;
    }

    public void ResetMuzzlePoint()
    {
        Weapon.muzzlePoint = Weapon.defaultMuzzlePoint;
    }

    public void ResetAimPosition()
    {
        Weapon.aimPosition = Weapon.defaultAimPos;
    }

    public void ChangeWeaponMode(Weapon.WeaponMode weaponMode)
    {
        Weapon.weaponMode = weaponMode;
    }

    public void PlayAnimation(string eventName)
    {
        Weapon.Animator.Play(eventName);
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
                    selectedSound = Weapon.fireSounds[Random.Range(0, Weapon.fireSounds.Length)];
                    break;

                case "NO_BULLETS":
                    selectedSound = Weapon.noBulletSound;
                    break;

                case "START_RELOAD":
                    selectedSound = Weapon.initialReloadSound;
                    break;

                case "MIDDLE_RELOAD":
                    selectedSound = Weapon.middleReloadSound;
                    break;

                case "END_RELOAD":
                    selectedSound = Weapon.endReloadSound;
                    break;
            }

            if (selectedSound)
            {
                Weapon.Source.PlayOneShot(selectedSound, volume);
            }
        }
        else
        {
            Weapon.Source.PlayOneShot(clip, volume);
        }
    }

    public void CalculateFire()
    {
        if (Weapon.currentBullets >= Weapon.bulletsPerFire && Weapon.firerateTimer <= 0)
        {
            Weapon.States.SetState("Firing", true);

            // Tracer
            List<Vector3> positions = new List<Vector3>();
            LineRenderer tracer = GameObject.Instantiate(WeaponManager.GetTracerPrefab(), Weapon.muzzlePoint.position, Quaternion.identity).GetComponent<LineRenderer>();
            Tracer tracerScript = tracer.gameObject.GetComponent<Tracer>();

            Vector3 point1 = Weapon.muzzlePoint.position;
            Vector3 predictedBulletVelocity = Weapon.muzzlePoint.forward * Weapon.maxBulletDistance;
            float stepSize = 0.01f;

            // Tracer start position
            positions.Add(point1);

            for (float step = 0f; step < 1; step += stepSize)
            {
                predictedBulletVelocity += (Physics.gravity * stepSize) * Weapon.bulletGravityScale;
                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;

                // Tracer positions
                positions.Add(point2);

                Ray ray = new Ray(point1, point2 - point1);
                RaycastHit hit;

                Debug.DrawLine(point1, point2, Color.green);

                if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, Weapon.hittableMask))
                {
                    if (hit.transform)
                    {
                        float distance = (Weapon.muzzlePoint.position - hit.point).sqrMagnitude;
                        float time = distance / (Weapon.bulletVelocity * 1000f);
                        Weapon.StartCoroutine(CalculateDelay(time, hit));
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

            Weapon.currentBullets -= Weapon.bulletsPerFire;
            Weapon.firerateTimer = Weapon.firerate;

            Weapon.States.SetState("Firing", false);
        }
    }

    public IEnumerator CalculateDelay(float time, RaycastHit hit)
    {
        yield return new WaitForSeconds(time);

        // Force
        Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
        if (HitBody)
        {
            HitBody.AddForceAtPosition(Weapon.muzzlePoint.forward * Weapon.bulletHitForce, hit.point);
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
            GameObject impact = GameObject.Instantiate(WeaponManager.GetImpactWithTag(hit.transform.tag).prefab, hit.point,
            Quaternion.LookRotation(hit.normal));
            impact.transform.position += impact.transform.forward * 0.001f;

            MonoBehaviour.Destroy(impact, 5f);
        }
    }

    public void ApplyRecoil()
    {
        Vector3 pos = new Vector3(
            Random.Range(Weapon.recoilForcePos.x / 2f, Weapon.recoilForcePos.x),
            Random.Range(Weapon.recoilForcePos.y / 2f, Weapon.recoilForcePos.y),
            Random.Range(Weapon.recoilForcePos.z / 2f, Weapon.recoilForcePos.z));

        Vector3 rot = new Vector3(
            Random.Range(Weapon.recoilForceRot.x / 2f, Weapon.recoilForceRot.x),
            Random.Range(Weapon.recoilForceRot.y / 2f, Weapon.recoilForceRot.y),
            Random.Range(Weapon.recoilForceRot.z / 2f, Weapon.recoilForceRot.z));

        Weapon.recoilRoot.localPosition += pos;
        Weapon.recoilRoot.localRotation *= Quaternion.Euler(rot);

        PlayerCamera.ApplyRecoil(Weapon.recoilForceCamera);
    }

    public void CalculateReload()
    {
        if (Weapon.currentBullets < Weapon.bulletsPerMagazine)
        {
            int necessaryBullets = 0;

            for (int i = 0; i < Weapon.bulletsPerMagazine; i++)
            {
                if ((Weapon.currentBullets + necessaryBullets) < Weapon.bulletsPerMagazine)
                {
                    necessaryBullets++;
                }
            }

            if (Weapon.extraBullets >= necessaryBullets)
            {
                Weapon.currentBullets += necessaryBullets;
                Weapon.extraBullets -= necessaryBullets;
            }
            else if (Weapon.extraBullets < necessaryBullets)
            {
                necessaryBullets = Weapon.extraBullets;
                Weapon.currentBullets += necessaryBullets;
                Weapon.extraBullets = 0;
            }
        }
    }
}
