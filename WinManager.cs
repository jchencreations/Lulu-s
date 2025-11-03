using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class WinManager : MonoBehaviour
{
    public GameObject table;
    public GameObject leave;
    public VisualEffect dissolveEffect;
    public VisualEffect dissolveEffectForDoors;
    public FMODUnity.EventReference cthGuide;
    private FMOD.Studio.EventInstance cthGuideIns;

    public GameObject pencil;


    public float requiredCollisionTime = 2f;
    private bool opened = false;
    public bool palm = false;
    public bool checkwriting = true;


    // Start is called before the first frame update
    void Start()
    {
        cthGuideIns = FMODUnity.RuntimeManager.CreateInstance(cthGuide);
        cthGuideIns.start();
        table.SetActive(false);
        leave.SetActive(false);
        pencil.SetActive(false);
        StartCoroutine(ShowTable());
        dissolveEffect.Stop();
        dissolveEffectForDoors.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ShowLeave();
    }

    private IEnumerator ShowTable()
    {
        yield return new WaitForSeconds(2);
        dissolveEffect.Play();
        table.SetActive(true);
        Dissolving.StartReverseDissolveObject(table);
        yield return new WaitForSeconds(8);
        pencil.SetActive(true);

        Dissolving.StartReverseDissolveObject(pencil);
    }

    private void ShowLeave()
    {
        if(checkwriting)
            StartCoroutine(LeavingItems());
    }

    private IEnumerator LeavingItems()
    {
        yield return new WaitForSeconds(3);
        dissolveEffectForDoors.Play();
        leave.SetActive(true);
        Dissolving.StartReverseDissolveObject(leave);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!opened && InkGenerator.instance.written)
        {
            if (other.CompareTag("PlayerHand"))
            {
                if (palm)
                {
                    
                    ShowLeave();
                    opened = true;
                }
            }
        }
        
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (isColliding && other.CompareTag("Key"))
    //    {
    //        collisionTimer += Time.deltaTime;

    //        if (collisionTimer >= requiredCollisionTime && palm)
    //        {
    //            ShowLeave();
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.CompareTag("Key"))
    //    {
    //        isColliding = false;
    //        collisionTimer = 0f;
    //    }
    //}
    public void PalmOn()
    {
        palm = true;
    }
    public void PalmOff()
    {
        palm = false;
    }



}
