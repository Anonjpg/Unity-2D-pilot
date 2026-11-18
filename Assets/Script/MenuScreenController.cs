using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreenController : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
