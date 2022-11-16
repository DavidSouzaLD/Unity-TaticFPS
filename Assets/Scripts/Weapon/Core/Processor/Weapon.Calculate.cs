using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Game.Player;
using Game.WeaponSystem.Components;
using Game.WeaponSystem.Others;

namespace Game.WeaponSystem
{
    public partial class Weapon
    {
        private void CalculateFire()
        {
            if (currentBullets >= data.bulletsPerFire && firerateTimer <= 0)
            {
                // Tracer
                List<Vector3> tracerPositions = new List<Vector3>();
                LineRenderer tracer = GameObject.Instantiate(WeaponManager.tracerPrefab, fireRoot.position, Quaternion.identity).GetComponent<LineRenderer>();
                BulletTracer tracerScript = tracer.gameObject.GetComponent<BulletTracer>();

                Vector3 point1 = fireRoot.position;
                Vector3 predictedBulletVelocity = fireRoot.forward * data.maxBulletDistance;
                float stepSize = 0.01f;

                // Tracer start position
                tracerPositions.Add(point1);

                for (float step = 0f; step < 1; step += stepSize)
                {
                    predictedBulletVelocity += (Physics.gravity * stepSize) * data.bulletGravityScale;
                    Vector3 point2 = point1 + predictedBulletVelocity * stepSize;
                    tracerPositions.Add(point2);

                    Ray ray = new Ray(point1, point2 - point1);
                    RaycastHit hit;

                    Debug.DrawLine(point1, point2, Color.green);

                    if (Physics.Raycast(ray, out hit, (point2 - point1).magnitude, WeaponManager.hittableMask))
                    {
                        if (hit.transform)
                        {
                            float distance = (fireRoot.position - hit.point).sqrMagnitude;
                            float time = distance / (data.bulletVelocity * 1000f);
                            StartCoroutine(CalculateDelay(time, hit));
                            break;
                        }
                    }

                    point1 = point2;
                }

                tracerScript.pos = tracerPositions;

                currentBullets -= data.bulletsPerFire;
                firerateTimer = data.firerate;

                // Visuals
                WeaponManager.PlaySound("Fire");
                weaponAnimation.Play("Fire");

                // Events
                Recoil.ApplyRecoil(data.recoilForcePos, data.recoilForceRot, data.recoilForceCam);
                StartCoroutine(ApplyMuzzle(data.muzzleTime));
                bulletDrop.Drop();

                // Delegate
                onFiring?.Invoke();
            }
        }

        private IEnumerator CalculateDelay(float time, RaycastHit hit)
        {
            yield return new WaitForSeconds(time);

            // Force
            Rigidbody HitBody = hit.transform.GetComponent<Rigidbody>();
            if (HitBody)
            {
                HitBody.AddForceAtPosition(fireRoot.forward * data.bulletHitForce, hit.point);
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
                Hitmark.ApplyHitMark(PlayerCamera.WorldToScreen(hit.point));
                WeaponManager.PlaySound("Hitmark", 0.3f);
            }

            // Bullet hole
            if (Impacts.GetImpactWithTag(hit.transform.tag) != null)
            {
                GameObject impact = GameObject.Instantiate(Impacts.GetImpactWithTag(hit.transform.tag).prefab, hit.point, Quaternion.LookRotation(hit.normal));
                impact.transform.parent = hit.collider.transform;
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
    }
}