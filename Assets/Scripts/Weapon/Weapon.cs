using UnityEngine;

public class Weapon : WeaponBase
{
    protected override void Fire()
    {
        bool canFire = !PlayerController.GetState("Running") && PlayerCamera.isCursorLocked && !isReloading;

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

    protected override void Reload()
    {
        bool canReload = InputManager.Reload && !PlayerController.GetState("Running") && PlayerCamera.isCursorLocked && extraBullets > 0 && currentBullets < bulletsPerMagazine && !isReloading;

        if (canReload)
        {
            PlayAnimation("RELOAD");
        }

        // Animation no-bullet
        Animator.SetBool("NO_BULLET", !HaveBullets());
    }

    protected override void Aim()
    {
        bool canAim = InputManager.Aim && !PlayerController.GetState("Running") && PlayerCamera.isCursorLocked && !isReloading;

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

    protected override void Recoil()
    {
        // Reset recoil
        if (recoilRoot.localPosition != startRecoilPos || recoilRoot.localRotation != startRecoilRot)
        {
            recoilRoot.localPosition = Vector3.Lerp(recoilRoot.localPosition, startRecoilPos, recoilResetSpeed * Time.deltaTime);
            recoilRoot.localRotation = Quaternion.Slerp(recoilRoot.localRotation, startRecoilRot, recoilResetSpeed * Time.deltaTime);
        }
    }
}