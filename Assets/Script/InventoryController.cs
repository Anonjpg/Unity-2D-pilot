using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefab;

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();

        

        // for(int i = 0; i < slotCount; i++)
        // {
        //     Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
        //     if(i < itemPrefab.Length)
        //     {
        //         GameObject item = Instantiate(itemPrefab[i], slot.transform);
        //         slot.currentItem = item;
        //     }
        // }
    }

    public bool AddItem(GameObject itemPrefab)
    {
         foreach(Transform slotTransform in inventoryPanel.transform)
         {
             Slot slot = slotTransform.GetComponent<Slot>();
             if (slot != null && slot.currentItem == null)
             {
                 GameObject item = Instantiate(itemPrefab, slotTransform);
                 RectTransform rt = item.GetComponent<RectTransform>();

                 rt.anchoredPosition = Vector2.zero;
                 rt.localScale = new Vector3(1f, 0.8f, 1f);
                 slot.currentItem = item;
                 return true;
             }
         }

        
        return false;
     }
    
    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> inData = new List<InventorySaveData>();
        foreach(Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if(slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                inData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex()}); 
            }
        }
        return inData;
    }

    

    public void SetInventoryItem(List<InventorySaveData> inventorySaveData)
    {
        foreach(Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        SetupSlotReferences();

        foreach(InventorySaveData data in inventorySaveData)
        {
            if(data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
                if(itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    
                    RectTransform rt = item.GetComponent<RectTransform>();

                    rt.anchoredPosition = Vector2.zero;
                    rt.localScale = new Vector3(1f, 0.8f, 1f);

                    slot.currentItem = item;
                }
            }
        }

        void SetupSlotReferences()
        {

            InventorySelectionUI ui = FindObjectOfType<InventorySelectionUI>();

            foreach (Transform child in inventoryPanel.transform)
            {
                Slot slot = child.GetComponent<Slot>();

                if (slot != null)
                {
                    slot.selectionUI = ui;
                }
            }
        }
    }
}
