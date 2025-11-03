using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManager;
using UnityEngine.SceneManagement;

public class FirstMenuManager : MonoBehaviour
{
    public static FirstMenuManager instance { get; private set; }
    [SerializeField] private Canvas intro;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        StartCoroutine(LoadIntro());
    }

    private IEnumerator LoadIntro()
    {
        yield return new WaitForSeconds(3);
        intro.enabled = false;
    }

    public void StartTutorial()
    {
        SceneTransitionManager.instance.LoadScene(2);
    }
}
