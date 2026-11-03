using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using NUnit.Framework;
using System;
using System.Collections;

public class AimController : MonoBehaviour
{
    [Header("References")]
    public PlayerEquipment playerEquipment;
    public PlayerMovement playerMovement;
    public Animator playerAnimator;
    public Animator playerAnimReload;

    [Header("Aim Pivots")]
    public GameObject aimPivotRight;
    public GameObject aimPivotLeft;

    [Header("Aim Mouse Controllers")]
    public AimMouseRight aimMouseRight;
    public AimMouseLeft aimMouseLeft;

    [Header("Visuals")]
    public SpriteRenderer normalBodyRenderer;
    public GameObject aimBody;
    public GameObject aimRightVisuals;
    public GameObject aimLeftVisuals;

    [Header("Input")]
    public Key aimKey = Key.Q;

    [Header("Player Body Position")]
    public SpriteRenderer playerBodyRenderer;

    public Vector3 bodyRightPosition;
    public Vector3 bodyLeftPosition;

    [Header("shoot")]
    public GameObject bullets;
    public Animator gunAnim;
    public Animator gunAnimLeft;
    public Transform ShootPointRight;
    public Transform ShootPointLeft;
    public float bulletspeed;
    public float fireRates;
    public Animator muzzleFlashAnim; 

    [Header("Ammo")]
    public int magazineAmmo = 8;
    public int currentAmmo = 8;
    public int reserveAmmo = 32;

    [Header("Recoil")]
    public float recoilForce = 10f;

    [Header("Ammo UI")]
    public TMP_Text ammoText;

    [Header("reload System")]
    public bool isReloading = false;
    public PlayerMovement playerMovementScript;

    [Header("Aim Cooldown")]
    public float aimCooldown = 10f;
    private bool isAimCoolDown = false;
    public Rigidbody2D rb;
    float ReadyForNextShot;
    public CameraRecoil cameraRecoil;

    public bool IsAiming { get; private set; }

    void Start()
    {
        SetAim(false);
        UpdateAmmoUI();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[aimKey].wasPressedThisFrame)
        {
            if (!IsAiming)
            {
                if (playerEquipment == null ||
                    !playerEquipment.HasPistol)
                {
                    Debug.LogWarning("Equip pistol terlebih dahulu!");
                    return;
                }

                ToggleAim();
            }
            else
            {
                SetAim(false);
            }
        }

        if (Keyboard.current[Key.R].wasPressedThisFrame)
            {
                Reload();
            }

        if (IsAiming)
        {
            if(!isReloading && Input.GetMouseButton(0))
                {
                    if (Time.time > ReadyForNextShot)
                    {
                        ReadyForNextShot = Time.time + 1/fireRates;
                        Shoot();
                    }
                    
                }
            

            UpdateAimDirection();
        }
            

    }

    void SetAim(bool aiming)
    {
        IsAiming = aiming;

        if (normalBodyRenderer != null)
            normalBodyRenderer.enabled = !aiming;

        if (aimBody != null)
            aimBody.SetActive(aiming);

        if (playerMovement != null)
            playerMovement.isAiming = aiming;

        if (playerAnimator != null)
            playerAnimator.SetBool("isAiming", aiming);

        if (aimMouseRight != null)
            aimMouseRight.enabled = false;

        if (aimMouseLeft != null)
            aimMouseLeft.enabled = false;

        if (!aiming)
        {
            if (aimPivotRight != null)
                aimPivotRight.SetActive(false);

            if (aimPivotLeft != null)
                aimPivotLeft.SetActive(false);

            if (aimRightVisuals != null)
                aimRightVisuals.SetActive(false);

            if (aimLeftVisuals != null)
                aimLeftVisuals.SetActive(false);
        }
        else
        {
            UpdateAimDirection();
        }
    }

    void UpdateAimDirection()
    {
        if (Camera.main == null || Mouse.current == null)
            return;

        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        float depth = -Camera.main.transform.position.z;

        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreen.x, mouseScreen.y, depth)
        );

        bool aimRight = mouseWorld.x >= transform.position.x;
        FlipPlayerBody(aimRight);

        if (aimPivotRight != null)
            aimPivotRight.SetActive(aimRight);

        if (aimPivotLeft != null)
            aimPivotLeft.SetActive(!aimRight);

        if (aimRightVisuals != null)
            aimRightVisuals.SetActive(aimRight);

        if (aimLeftVisuals != null)
            aimLeftVisuals.SetActive(!aimRight);

        if (aimMouseRight != null)
            aimMouseRight.enabled = aimRight;

        if (aimMouseLeft != null)
            aimMouseLeft.enabled = !aimRight;
    }

    public void ExitAim()
    {
        if (IsAiming)
            SetAim(false);
    }

    void FlipPlayerBody(bool faceRight)
    {
        if (playerBodyRenderer == null)
            return;

        playerBodyRenderer.flipX = !faceRight;

        playerBodyRenderer.transform.localPosition =
            faceRight ? bodyRightPosition : bodyLeftPosition;
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            Debug.Log("Ammo habis");
            return;
        }

        if (aimMouseRight != null && aimMouseRight.enabled)
        {
            GameObject bulletInsRight = Instantiate(bullets, ShootPointRight.position, ShootPointRight.rotation);
            Vector2 shootDirection = ShootPointRight.right;
            bulletInsRight.GetComponent<Rigidbody2D>().AddForce(shootDirection * bulletspeed);
            gunAnim.SetTrigger("Shoot");
            if (muzzleFlashAnim != null)
            {
                muzzleFlashAnim.SetTrigger("Shoot");
            }
            PlayerRecoil(shootDirection);
            Destroy(bulletInsRight, 3);
            currentAmmo--;
            UpdateAmmoUI();
        }
        if (aimMouseLeft != null && aimMouseLeft.enabled)
        {
            GameObject bulletInsLeft = Instantiate(bullets, ShootPointLeft.position, ShootPointLeft.rotation);
            Vector2 shootDirectionLeft = -ShootPointLeft.right;
            bulletInsLeft.GetComponent<Rigidbody2D>().AddForce(shootDirectionLeft * bulletspeed);
            gunAnimLeft.SetTrigger("Shoot1");
            PlayerRecoil(shootDirectionLeft);
            Destroy(bulletInsLeft, 3);
            currentAmmo--;
            UpdateAmmoUI();
        }
        
        cameraRecoil.ShakeCamera();
       
    }

    public void UpdateAmmoUI()
    {
        if (ammoText != null)
            {
                ammoText.text = currentAmmo.ToString("00")
                    + " / "
                    + reserveAmmo.ToString("00");
            }
    }

    void Reload()
    {
        if (isReloading)
            return;

        if (playerMovementScript.HorizontalInput != 0f)
            return;
            
        if (currentAmmo >= magazineAmmo)
        {
            return;
        }

        if (reserveAmmo <= 0)
        {
            return;
        }

            isReloading = true;
            playerAnimReload.SetTrigger("Reload");
            rb.linearVelocity = Vector2.zero;
        
    }

    public void FinishReload()
    {
        int ammoNeeded = magazineAmmo - currentAmmo;

        int ammoToReload = Math.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToReload;
        reserveAmmo -= ammoToReload;

        isReloading = false;

        UpdateAmmoUI();
    }

    void ToggleAim()
    {
        if (isAimCoolDown)
            return;

        isAimCoolDown = true;

        SetAim(!IsAiming);

        StartCoroutine(AimCoolDown());
    }

    IEnumerator AimCoolDown()
    {
        yield return new WaitForSeconds(aimCooldown);
        isAimCoolDown = false;
        Debug.Log("Aim cooldown selesai. Sekarang bisa menembak lagi.");
    }

    void PlayerRecoil(Vector2 shootDirection)
    {
        Vector2 recoilDirection = -shootDirection.normalized;

        rb.AddForce(recoilDirection * recoilForce, ForceMode2D.Impulse);
        Debug.Log("Recoil applied: " + recoilDirection * recoilForce);
    }
}
