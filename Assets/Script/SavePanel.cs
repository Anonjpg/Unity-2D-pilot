using UnityEditor.Search;
using UnityEngine;

public class SavePanel : MonoBehaviour
{
    public GameObject SaveCanvas;

    public bool playerinRange;

    void Start()
    {
        SaveCanvas.SetActive(false);

        playerinRange = false;
    }

    void Update()
    {
        if (playerinRange)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                SaveCanvas.SetActive(!SaveCanvas.activeSelf);
            }
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinRange = true;
            
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerinRange = false;
        }   
    }

    public void ExitUI()
    {
        SaveCanvas.SetActive(false);
    }
}

