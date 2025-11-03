using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMOD_Random_Trigger : MonoBehaviour
{
    public FMODUnity.EventReference woodCrack;
    public FMOD.Studio.EventInstance woodCrackInstance;
    public FMODUnity.EventReference pitchShift;
    public FMOD.Studio.EventInstance pitchShiftInstance;
    public FMODUnity.EventReference cthDia;
    public FMOD.Studio.EventInstance cthDiaInstance;
    public FMODUnity.EventReference cthPop;
    public FMOD.Studio.EventInstance cthPopIns;
    private float timeSpace_1;
    private float timeSpace_2;
    private float timeSpace_3;
    private bool diaIsPlaying = false;

    // Start is called before the first frame update
    private void Start()
    {
        woodCrackInstance = FMODUnity.RuntimeManager.CreateInstance(woodCrack);
        pitchShiftInstance = FMODUnity.RuntimeManager.CreateInstance(pitchShift);
        cthDiaInstance = FMODUnity.RuntimeManager.CreateInstance(cthDia);
        cthPopIns = FMODUnity.RuntimeManager.CreateInstance(cthPop);
        timeSpace_1 = Random.value * 5 + 5.0f;
        timeSpace_2 = Random.value * 10 + 7.0f;
        timeSpace_3 = Random.value * 7 + 20.0f;
    }

    // Update is called once per frame

    

    private void Update()
    {
        if (timeSpace_1 <= 0)
        {
            woodCrackInstance.start();
            timeSpace_1 = Random.value * 5 + 5.0f;
        }
        timeSpace_1 -= Time.deltaTime;

        if (timeSpace_2 <= 0)
        {
            //sustain_time = Random.value * 2 + 2.0f;
            StartCoroutine(WaitSeconds(3.0f));
            timeSpace_2 = Random.value * 10 + 10.0f;
        }
        timeSpace_2 -= Time.deltaTime;

        if (timeSpace_3 <= 0)
        {
            cthDiaInstance.start();
            timeSpace_3 = Random.value * 7 + 20.0f;
        }
        timeSpace_3 -= Time.deltaTime;
    }

    private IEnumerator WaitSeconds(float sec)
    {
        pitchShiftInstance.start();
        yield return new WaitForSeconds(sec);
        pitchShiftInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
    
}
