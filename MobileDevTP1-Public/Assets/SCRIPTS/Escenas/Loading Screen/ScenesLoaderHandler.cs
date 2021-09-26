using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ScenesLoaderHandler : MonoBehaviour
{
    const string LOADING_SCENE = "LoadingScreen";
    static string sceneFrom;
    static string sceneToLoad;

    public Slider fillSlider;
    public float minimumTime = 5f;
    public TextMeshProUGUI textProgres;
    
    float timeLoading;
    float loadingProgress;

    public static void LoadScene(string scene)
    {
        sceneFrom = SceneManager.GetActiveScene().name;
        sceneToLoad = scene;
        SceneManager.LoadScene(LOADING_SCENE, LoadSceneMode.Additive);
    }

    private IEnumerator Start()
    {
        yield return SceneManager.UnloadSceneAsync(sceneFrom);
        var operation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        loadingProgress = 0;
        timeLoading = 0;

        yield return null;
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            timeLoading += Time.deltaTime;
            loadingProgress = operation.progress + 0.1f;
            loadingProgress = loadingProgress * timeLoading / minimumTime;

            if (loadingProgress >= 1)
            {
                operation.allowSceneActivation = true;
            }

            UpdateBar(operation.progress);
            yield return null;
        }
    }

    void UpdateBar(float value)
    {
        float progress = Mathf.Clamp01(value / 0.9f);
        fillSlider.value = progress;

        textProgres.text = (progress * 100f).ToString("0") + "%";
    }
}
