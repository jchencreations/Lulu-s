using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBG : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] private float speed;
    [SerializeField] private float width;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.forward * Time.deltaTime * speed);

        if(transform.position.x < startPos.x - width)
        {
            transform.position = startPos;
        }
    }
}
