using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawnSpot;
    [SerializeField] private Transform respawnPoison;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);

        if (other.gameObject.CompareTag("Poison") || other.gameObject.CompareTag("PoisonBinder") || other.gameObject.CompareTag("Ladle"))
        {
            if (other.gameObject.GetComponent<Rigidbody>() != null)
            {
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.transform.rotation = Quaternion.identity;
                other.transform.position = respawnPoison.transform.position;
            }
            else if (other.gameObject.GetComponentInParent<Rigidbody>() != null)
            {
                other.gameObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.transform.parent.transform.rotation = Quaternion.identity;
                other.gameObject.transform.parent.transform.position = respawnPoison.transform.position;
            }
        }
        else if (other.gameObject.CompareTag("Dish") || other.gameObject.CompareTag("Ingredient"))
        {
            if(other.gameObject.GetComponent<Rigidbody>() != null)
            {
                other.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.transform.rotation = Quaternion.identity;
                other.transform.position = respawnSpot.transform.position;
            }
            else if (other.gameObject.GetComponentInParent<Rigidbody>() != null)
            {
                other.gameObject.GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
                other.gameObject.transform.parent.transform.rotation = Quaternion.identity;
                other.gameObject.transform.parent.transform.position = respawnSpot.transform.position;
            }
        }
    }
}
