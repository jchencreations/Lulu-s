using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollision : MonoBehaviour
{
    [SerializeField] private float _detectionDistance = 0.2f;
    [SerializeField] private LayerMask _detectionLayers;

    [SerializeField] private GameObject _camera;

    public List<RaycastHit> DetectedColliderHits { get; private set; }

    [SerializeField] public float pushBackStrength = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        DetectedColliderHits = PerformDetection(transform.position, _detectionDistance, _detectionLayers);
    }

    // Update is called once per frame
    void Update()
    {
        DetectedColliderHits = PerformDetection(transform.position, _detectionDistance, _detectionLayers);     

        if(DetectedColliderHits.Count > 0)
        {
            Vector3 dir = PushBackDirection(DetectedColliderHits).normalized;
            _camera.transform.position += (dir * pushBackStrength * Time.deltaTime);
        }
    }

    private List<RaycastHit> PerformDetection(Vector3 position, float distance, LayerMask mask)
    {
        List<RaycastHit> detectedHits = new();
        List<Vector3> directions = new() {transform.forward, transform.right, -transform.right};

        RaycastHit hit;

        foreach(Vector3 dir in directions)
        {
            if(Physics.Raycast(position, dir, out hit, distance, mask))
            {
                detectedHits.Add(hit);
            }
        }
        return detectedHits;
    }

    private Vector3 PushBackDirection(List<RaycastHit> colliderHits)
    {
        Vector3 combinedNormal = Vector3.zero;
        foreach(RaycastHit hit in colliderHits)
        {
            combinedNormal += new Vector3(hit.normal.x,0,hit.normal.z);

        }
        return combinedNormal;
    }
}
