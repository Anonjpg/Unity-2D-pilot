using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public GameObject currentItem;
    public Outline slotOutline;
    public InventorySelectionUI selectionUI;

    void Awake()
    {
        if (slotOutline == null)
            slotOutline = GetComponent<Outline>();

        SetSelected(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (selectionUI == null)
        {
            selectionUI = FindFirstObjectByType<InventorySelectionUI>(FindObjectsInactive.Include);
        }

        selectionUI.SelectSlot(this, eventData.clickCount);
    }

    public void SetSelected(bool selected)
    {
        if (slotOutline != null)
        {
            slotOutline.enabled = selected;
        }
    }
}
