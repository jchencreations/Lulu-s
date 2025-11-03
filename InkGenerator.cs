using System.Collections.Generic;
using UnityEngine;

public class InkGenerator : MonoBehaviour
{
    public static InkGenerator instance;
    [SerializeField] private GameObject inkPrefab;
    [SerializeField] private Transform pencilTransform;
    [SerializeField] private Transform paperTransform;
    [SerializeField] private Vector3 pencilOffset;

    private InkTracer ink;
    private bool isTouching = false;
    private GameObject newInk = null;
    public bool written;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void OnTriggerEnter(Collider otherObject)
    {
        if (otherObject.CompareTag("Paper"))
            isTouching = true;
    }

    private void OnTriggerExit(Collider otherObject)
    {
        if (otherObject.CompareTag("Paper"))
            isTouching = false;
    }

    private void Update()
    {
        if (isTouching && newInk == null)
        {
            written = true;
            newInk = Instantiate(inkPrefab);
            ink = newInk.GetComponent<InkTracer>();
        }
        if (isTouching && newInk != null)
        {
            Vector3 pos = new Vector3(pencilTransform.position.x, paperTransform.position.y, pencilTransform.position.z);
            ink.UpdateLineRenderer(pos + pencilOffset);
        }
        if (!isTouching)
            newInk = null;
    }
}