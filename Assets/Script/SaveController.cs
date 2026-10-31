using UnityEngine;
using System.IO;
using System.Collections.Generic;



public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "SaveData.json");
        Debug.Log(Application.persistentDataPath);
        inventoryController = FindObjectOfType<InventoryController>();

        LoadGame();
    }

    // Update is called once per frame
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            inventorySaveData = inventoryController.GetInventoryItems()
        };

        File.WriteAllText(saveLocation, JsonUtility.ToJson(saveData));
        
    }

    public void LoadGame()
    {
        if(File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData.playerPosition;

            inventoryController.SetInventoryItem(saveData.inventorySaveData);

        }
        else
        {
            SaveGame();
        }
    }


    public void DeleteSave()
    {
        if (File.Exists(saveLocation))
        {
            File.Delete(saveLocation);
        }
        else
        {
            Debug.Log("File save tidak ditemukan.");
        }

        
        if (inventoryController != null)
        {
            inventoryController.SetInventoryItem(new List<InventorySaveData>());
        }

        
        SaveGame();
    }

}
