using UnityEngine;

public class InventorySelectionUI : MonoBehaviour
{
    public GameObject optionsPopup;
    public PlayerEquipment playerEquipment;
    public int pistolItemID = 3;

    private Slot selectedSlot;

    void Start()
    {
        if (optionsPopup != null)
            optionsPopup.SetActive(false);
    }

    public void SelectSlot(Slot slot, int clickCount)
    {
        if (slot.currentItem == null)
        {
            if (selectedSlot != null)
                selectedSlot.SetSelected(false);
            
            selectedSlot = null;

            if (optionsPopup != null)
                optionsPopup.SetActive(false);

            return;
        }

        if (selectedSlot != null && selectedSlot != slot)
            selectedSlot.SetSelected(false);

        selectedSlot = slot;
        selectedSlot.SetSelected(true);

        if (optionsPopup != null)
            optionsPopup.SetActive(clickCount >= 2);
    }

    public void EquipSelectItem()
    {
        if(selectedSlot == null || selectedSlot.currentItem == null)
            return;

        Item item = selectedSlot.currentItem.GetComponent<Item>();

        if (item == null)
        {
            Debug.LogWarning("Item component tidak ditemukan.");
            return;
        }

        if (item.ID != pistolItemID)
        {
            Debug.Log("Item ini bukan pistol.");
            return;
        }

        if (playerEquipment == null)
        {
            Debug.LogWarning("PlayerEquipment belum dihubungkan.");
            return;
        }

        playerEquipment.EquipPistol();
        optionsPopup.SetActive(false);

        Debug.Log("Pistol berhasil di-equip.");
    }
}
