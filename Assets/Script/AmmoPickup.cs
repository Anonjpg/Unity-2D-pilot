using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
   public int ammoAmount = 10;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"));
        {
            AimController aimController = collision.GetComponent<AimController>();

            if (aimController != null)
            {
                aimController.reserveAmmo += ammoAmount;
                aimController.UpdateAmmoUI();
                Destroy(gameObject);
            }
      
          
        }
    }
}
