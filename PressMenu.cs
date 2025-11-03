using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressMenu : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            MenuManager.instance.StartMenu();
        }
    }
}
