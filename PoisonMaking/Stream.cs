using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stream : MonoBehaviour
{
    private LineRenderer lineRenderer = null;
    private Vector3 targetPosition = Vector3.zero;
    private ParticleSystem splashParticle = null;

    private Coroutine pourRoutine = null;

    public PourDetector pourDetector;

    public bool isLadle = false;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        splashParticle = GetComponentInChildren<ParticleSystem>();
        pourDetector = GetComponentInParent<PourDetector>(); // or use other methods to find PourDetector
        if (pourDetector != null)
            lineRenderer.material = pourDetector.material;
        else if (GetComponentInParent<LadlePourDetector>() != null)
            lineRenderer.material = GetComponentInParent<LadlePourDetector>().material;
        else
            Debug.LogError("PourDetector not found");

        
        //lineRenderer.useWorldSpace = false;
    }

    private void Update()
    {
        if (isLadle)
        {
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.alignment = LineAlignment.TransformZ;

        }
    }

    private void Start()
    {
        MoveToPosition(0, transform.position);
        MoveToPosition(1, transform.position);
    }

    public void Begin(bool pouring)
    {
        isLadle = pouring;
        StartCoroutine(UpdateParticle());
        pourRoutine = StartCoroutine(BeginPour());
    }

    private IEnumerator BeginPour()
    {
        while (gameObject.activeSelf)
        {
            targetPosition = FindEndPoint();

            MoveToPosition(0, transform.position);
            AnimateToPosition(1, targetPosition);
            yield return null;
        }
    }

    public void End()
    {
        StopCoroutine(pourRoutine);
        pourRoutine = StartCoroutine(EndPour());
    }

    public IEnumerator EndPour()
    {
        while (!HasReachPosition(0, targetPosition))
        {
            AnimateToPosition(0, targetPosition);
            AnimateToPosition(1, targetPosition);

            yield return null;
        }

        Destroy(this.gameObject);
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 2.0f);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);


        return endPoint;
    }

    public Vector2 GetTargetPosition()
    {
        return targetPosition;
    }

    private void MoveToPosition(int index, Vector3 targetPosition)
    {
        lineRenderer.SetPosition(index, targetPosition);
    }

    private void AnimateToPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPoint = lineRenderer.GetPosition(index);
        Vector3 newPosition = Vector3.MoveTowards(currentPoint, targetPosition, Time.deltaTime * 1.75f);
        lineRenderer.SetPosition(index, newPosition);
    }

    private bool HasReachPosition(int index, Vector3 targetPosition)
    {
        Vector3 currentPosition = lineRenderer.GetPosition(index);
        return currentPosition == targetPosition;
    }

    private IEnumerator UpdateParticle()
    {
        while (gameObject.activeSelf)
        {
            splashParticle.gameObject.transform.position = targetPosition;
            bool isHitting = HasReachPosition(1, targetPosition);
            splashParticle.gameObject.SetActive(isHitting);

            yield return null;
        }

    }
}