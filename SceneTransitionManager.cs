using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager instance;
    public FadeLoad fadeLoad;
    // Start is called before the first frame update

    private void Awake()
    {
        if(instance) Destroy(this.gameObject);
        else instance = this;
    }

    public void LoadScene(int sceneIdx)
    {
        StartCoroutine(SceneRoutine(sceneIdx));
    }

    private IEnumerator SceneRoutine(int sceneIdx)
    {
        fadeLoad.FadeOut();
        yield return new WaitForSeconds(fadeLoad.fadeTime);
        SceneManager.LoadScene(sceneIdx);
    }
}
