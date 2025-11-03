using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolving : MonoBehaviour
{
    public Material dissolveMaterial;
    public float dissolveDuration = 2f;
    [SerializeField] private List<Material> originalMaterials = new List<Material>();
    [SerializeField] private List<Renderer> renderers = new List<Renderer>();
    public Action OnDissolveComplete;
    public Action OnReverseDissolveComplete;

    private bool isDissolving = false;

    void Start()
    {
        GetRenderers(transform);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isDissolving)
            StartDissolve();

        if (Input.GetKeyDown(KeyCode.R) && !isDissolving)
            StartReverseDissolve();
    }

    public void StartDissolve()
    {
        StartCoroutine(Dissolver(0, 1, dissolveDuration, OnDissolveComplete));
    }

    public void StartReverseDissolve()
    {
        StartCoroutine(Dissolver(1, 0, dissolveDuration, OnReverseDissolveComplete));
    }

    private void GetRenderers(Transform parent)
    {
        foreach (Renderer rend in parent.GetComponents<Renderer>())
        {
            renderers.Add(rend);
            foreach (Material mat in rend.materials)
                originalMaterials.Add(new Material(mat));
        }

        foreach (Transform child in parent)
            GetRenderers(child);
    }

    private IEnumerator Dissolver(float startValue, float endValue, float duration, Action onComplete)
    {
        Debug.Log("startdissolve");
        isDissolving = true;
        float elapsedTime = 0f;

        foreach (Renderer rend in renderers)
        {
            Material[] mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
                mats[i] = new Material(dissolveMaterial);
            rend.materials = mats;
        }

        Debug.Log("changematerial");

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float dissolveStrength = Mathf.Lerp(startValue, endValue, elapsedTime / duration);

            foreach (Renderer rend in renderers)
                foreach (Material mat in rend.materials)
                    mat.SetFloat("_Amount", dissolveStrength);

            yield return null;
        }

        
        onComplete?.Invoke();

        Debug.Log("restore");

        if (endValue == 0)
            RestoreOriginalMaterials();

        isDissolving = false;
    }

    private void RestoreOriginalMaterials()
    {
        int index = 0;
        foreach (Renderer rend in renderers)
        {
            Material[] mats = rend.materials;
            for (int i = 0; i < mats.Length; i++)
                if (index < originalMaterials.Count)
                {
                    mats[i] = originalMaterials[index];
                    index++;
                }
            rend.materials = mats;
        }
    }

    public static void StartDissolveObject(GameObject obj)
    {
        Dissolving dissolving = obj.GetComponent<Dissolving>();
        if (dissolving != null)
            dissolving.StartDissolve();
        else
            Debug.LogWarning("Dissolving not found on " + obj.name);
    }

    public static void StartReverseDissolveObject(GameObject obj)
    {
        Dissolving dissolving = obj.GetComponent<Dissolving>();
        if (dissolving != null)
            dissolving.StartReverseDissolve();
        else
            Debug.LogWarning("Dissolving not found on " + obj.name);
    }
}
