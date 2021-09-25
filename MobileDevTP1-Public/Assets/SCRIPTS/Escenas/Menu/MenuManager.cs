using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{    
    public void GotoScene(string scene)
    {
        ScenesLoaderHandler.LoadScene(scene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
