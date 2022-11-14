using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Player;

namespace Game.Weapon
{
    // Fire
    public partial class Weapon
    {
        private void UpdateFire()
        {
            // Conditions
            bool modeConditions = (weaponMode == WeaponMode.Combat);
            bool playerConditions = !PlayerController.isRunning && PlayerCamera.cursorLocked;
            bool weaponManagerConditions = !weaponRetract.isRetracting;
            bool stateConditions = !isDrawing && !isHiding && !isReloading;
            bool conditions = modeConditions && playerConditions && weaponManagerConditions && stateConditions;

            if (conditions)
            {
                switch (fireMode)
                {
                    case WeaponFireMode.Semi:
                        if (PlayerKeys.Click("Fire"))
                        {
                            firingTimer = firerate * 3f;
                            CalculateFire();
                        }
                        break;

                    case WeaponFireMode.Auto:
                        if (PlayerKeys.Press("Fire"))
                        {
                            firingTimer = firerate * 3f;
                            CalculateFire();
                        }
                        break;
                }
            }

            // Is firing
            if (firingTimer >= 0)
            {
                isFiring = true;
                firingTimer -= Time.deltaTime;
            }
            else
            {
                isFiring = false;
            }

            if (firerateTimer >= 0)
            {
                firerateTimer -= Time.deltaTime;
            }
        }

        public void CalculateFire()
        {
            if (currentBullets >= bulletsPerFire && firerateTimer <= 0)
            {
                // Tracer
                List<Vector3> tracerPositions = new List<Vector3>();
                LineRenderer tracer = GameObject.Instantiate(WeaponManager.GetTracerPrefab, fireLocation.position, Quaternion.identity).GetComponent<LineRenderer>();
                BulletTracer tracerScript = tracer.gameObject.GetComponent<BulletTracer>();

                Vector3 point1 = fireLocation.position;
                Vector3 predictedBulletVelocity = fireLocation.forward * maxBulletDistance;
                float stepSize = 0.01f;

                // Tracer start position
                tracerPositions.Add(point1);

                for (float step = 0f; step < 1; step += stepSize)
                {
                    predictedBulletVelocity += (Physics.gravity * stepSize) * bulletGravityScale;
                    Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                    tracerPositions.Add(point2);

                    Ray ray = new Ray(point1, point2 - point1);
                    RaycastHit hit;

                    Debug.DrawLine(point1, point2, Color.green);

                    if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, WeaponManager.GetHittableMask))
                    {
                        if (hit.transform)
                        {
                            float distance = (fireLocation.position - hit.point).sqrMagnitude;
                            float time = distance / (bulletVelocity * 1000f);
                            StartCoroutine(CalculateDelay(time, hit));
                            break;
                        }
                    }

                    point1 = point2;
                }

                tracerScript.pos = tracerPositions;

                weaponRecoil.ApplyRecoil(recoilForcePos, recoilForceRot, recoilForceCam);

                currentBullets -= bulletsPerFire;
                firerateTimer = firerate;

                weaponAnimation.Play("Fire");
                weaponSound.Play("Fire");
                projectile.Drop();

                // Delegate
                onFiring?.Invoke();
            }
        }

        public IEnumerator CalculateDelay(float time, RaycastHit hit)
        {
            yield return new WaitForSeconds(time);

            // Force
            Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
            if (HitBody)
            {
                HitBody.AddForceAtPosition(fireLocation.forward * bulletHitForce, hit.point);
            }

            // Apply damage to IHealth
            IDamageable<float> hitHealth = hit.collider.GetComponent<IDamageable<float>>();

            if (hitHealth != null)
            {
                hitHealth.TakeDamage(Random.Range(minDamage, maxDamage));
            }

            // Hitmark in enemy
            if (hit.transform.tag.Equals("Enemy"))
            {
                WeaponHitmark.ApplyHitMark(PlayerCamera.WorldToScreen(hit.point));
                weaponSound.Play("Hitmark", 0.3f);
            }

            // Bullet hole
            if (WeaponImpacts.GetImpactWithTag(hit.transform.tag) != null)
            {
                GameObject impact = GameObject.Instantiate(WeaponImpacts.GetImpactWithTag(hit.transform.tag).prefab, hit.point, Quaternion.LookRotation(hit.normal));
                impact.transform.parent = hit.collider.transform;
                impact.transform.position += impact.transform.forward * 0.001f;

                MonoBehaviour.Destroy(impact, 5f);
            }
        }
    }
}