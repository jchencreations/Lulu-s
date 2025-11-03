using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressStart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            MenuManager.instance.StartGame();
        }
    }
}
