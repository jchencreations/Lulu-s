using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _menuParent;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu()
    {
        if (_menuParent.activeSelf)
            _menuParent.SetActive(false);
        else
            _menuParent.SetActive(true);
    }

}
