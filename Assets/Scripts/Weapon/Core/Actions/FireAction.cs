using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using WeaponSystem.Others;

namespace WeaponSystem.Core.Actions
{
    public class FireAction : IWeaponAction
    {
        public FireAction(Weapon weaponClass, WeaponSO weaponData)
        {
            weapon = weaponClass;
            data = weaponData;
        }

        public Weapon weapon { get; set; }
        public WeaponSO data { get; set; }
        public bool isActive { get; set; }
        public float firerateTimer { get; private set; }
        public float firingTimer { get; private set; }

        public void Start() { }

        public void Update()
        {
            bool conditions = firerateTimer <= 0 && weapon.currentBullets > 0;

            if (conditions)
            {
                switch (weapon.fireMode)
                {
                    case FireMode.Semi:

                        if (InputManager.Click("Fire"))
                        {
                            CallFire();
                        }

                        break;

                    case FireMode.Auto:

                        if (InputManager.Press("Fire"))
                        {
                            CallFire();
                        }

                        break;
                }
            }

            // Reset firerate
            if (firerateTimer >= 0)
            {
                firerateTimer -= Time.deltaTime;
            }

            // Reset firing timer
            if (firingTimer >= 0)
            {
                isActive = true;
                firingTimer -= Time.deltaTime;

            }
            else
            {
                isActive = false;
            }
        }

        private void CallFire()
        {
            // Bullet tracing
            List<Vector3> tracingPos = new List<Vector3>();
            LineRenderer tracingLine = GameObject.Instantiate(WeaponManager.TracerPrefab, weapon.fireRoot.position, Quaternion.identity).GetComponent<LineRenderer>();
            BulletTracing tracingScript = tracingLine.gameObject.GetComponent<BulletTracing>();

            Vector3 point1 = weapon.fireRoot.position;
            Vector3 predictedBulletVelocity = weapon.fireRoot.forward * data.maxBulletDistance;
            float stepSize = 0.01f;

            // Tracing start position
            tracingPos.Add(point1);

            for (float step = 0f; step < 1; step += stepSize)
            {
                // Apply gravity in bullet
                if (step > (data.effectiveDistance / data.maxBulletDistance))
                {
                    predictedBulletVelocity += (Physics.gravity * stepSize) * data.bulletGravityScale;
                }

                Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                tracingPos.Add(point2);

                Ray ray = new Ray(point1, point2 - point1);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, WeaponManager.HittableMask))
                {
                    if (hit.transform)
                    {
                        // Calculate delay time
                        float distance = (weapon.fireRoot.position - hit.point).sqrMagnitude;
                        float time = distance / (data.bulletVelocity * 1000f);

                        weapon.StartCoroutine(ApplyDelay(time, hit));
                        break;
                    }
                }

                point1 = point2;
            }

            tracingScript.pos = tracingPos;

            weapon.currentBullets -= data.bulletsPerFire;
            firerateTimer = data.firerate;

            // Animation and sound
            AudioManager.PlaySound(data.fireSounds[Random.Range(0, data.fireSounds.Length)]);
            //weaponAnimation.Play("Fire");

            // Events
            WeaponManager.Recoil.ApplyRecoil(data.recoilForcePos, data.recoilForceRot, data.recoilForceCam);
            weapon.StartCoroutine(ApplyMuzzle(data.muzzleTime));
            weapon.bulletDropping.Drop();

            weapon.OnFiring?.Invoke();
        }

        protected IEnumerator ApplyDelay(float time, RaycastHit hit)
        {
            yield return new WaitForSeconds(time);

            // Force
            Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();

            if (HitBody)
            {
                HitBody.AddForceAtPosition(weapon.fireRoot.forward * data.bulletHitForce, hit.point);
            }

            // Apply damage to IHealth
            IDamageable<float> hitHealth = hit.collider.GetComponent<IDamageable<float>>();

            if (hitHealth != null)
            {
                hitHealth.TakeDamage(Random.Range(data.minDamage, data.maxDamage));
            }

            // Hitmark in enemy
            if (hit.transform.tag.Equals("Enemy"))
            {
                //Hitmark.ApplyHitMark(PlayerCamera.WorldToScreen(hit.point));
                //WeaponManager.PlaySound("Hitmark", 0.3f);
            }

            // Bullet hole
            /*if (Impacts.GetImpactWithTag(hit.transform.tag) != null)
            {
                GameObject impact = GameObject.Instantiate(Impacts.GetImpactWithTag(hit.transform.tag).prefab, hit.point, Quaternion.LookRotation(hit.normal));
                impact.transform.parent = hit.collider.transform;
                impact.transform.position += impact.transform.forward * 0.001f;

                MonoBehaviour.Destroy(impact, 5f);
            }*/
        }

        protected IEnumerator ApplyMuzzle(float muzzleTime)
        {
            weapon.muzzleObject.SetActive(true);
            yield return new WaitForSeconds(muzzleTime);
            weapon.muzzleObject.SetActive(false);
        }
    }
}