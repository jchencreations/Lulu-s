using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressTutorial : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            FirstMenuManager.instance.StartTutorial();
        }
    }
}
