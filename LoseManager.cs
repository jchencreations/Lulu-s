using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseManager : MonoBehaviour
{
    //Audio for "join us"


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoseCoroutine());
    }

    private IEnumerator LoseCoroutine()
    {
        //Play join us audio


        yield return new WaitForSeconds(10f);

        //Stop join us audio


        SceneTransitionManager.instance.LoadScene(0);
    }
}
