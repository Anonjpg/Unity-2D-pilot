using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    public GameObject PausePanel;

    void Start()
    {
        PausePanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanel.SetActive(!PausePanel.activeSelf);
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
    }
}
