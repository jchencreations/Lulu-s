using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bookSound : MonoBehaviour
{
    public FMODUnity.EventReference bookFlipping;
    public FMOD.Studio.EventInstance bookFlippingInstance;
    // Start is called before the first frame update
    void Start()
    {
        bookFlippingInstance = FMODUnity.RuntimeManager.CreateInstance(bookFlipping);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(bookFlippingInstance, transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void flippingSound()
    {
        bookFlippingInstance.start();
    }
}
