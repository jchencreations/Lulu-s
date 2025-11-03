using System.Collections;
using UnityEngine;

public class LadlePourDetector : MonoBehaviour
{
    public int pourThreshold = 95;
    public Transform origin = null;
    public Transform originForward = null;
    public Transform originBackward = null;
    public Transform originLeft = null;
    public Transform originRight = null;

    public Transform raycastOrigin = null;
    public GameObject streamPrefab = null;
    public GameObject waterSoundPrefab;
    private GameObject waterSound;
    public GameObject smoke;

    private bool isPouring = false;
    private bool pourCheck = false;
    private Stream currentStream = null;

    private bool checkHit = false;

    public Material material;
    [SerializeField] private MeshRenderer rend;

    private GameObject dish;

    private GameObject streamObject;

    public bool isEmpty;

    private int layerMask;

    private void Start()
    {
        layerMask = 1 << 9;
    }

    private void Update()
    {
        if(!isEmpty)
        {
            pourCheck = CalculatePourAngle() > pourThreshold;

            if (isPouring != pourCheck)
            {
                isPouring = pourCheck;
                if (isPouring)
                {
                    StartPour();

                }
                else
                {
                    EndPour();
                }
            }
            if (isPouring)
            {
                Transform chosenOrigin = ChooseOrigin();
                streamObject.transform.position = chosenOrigin.position;
                streamObject.transform.rotation = chosenOrigin.rotation;
                streamObject.transform.forward = chosenOrigin.forward;
            }
        }
    }


    private void StartPour()
    {
        material = rend.material;
        currentStream = CreateStream();
        Vector3 targetPosition = FindEndPoint();
        waterSound = Instantiate(waterSoundPrefab, targetPosition, Quaternion.identity);
        checkHit = CheckForDishCollision();
        currentStream.Begin(true);
    }

    private Stream CreateStream()
    {
        Transform chosenOrigin = ChooseOrigin();
        streamObject = Instantiate(streamPrefab, chosenOrigin.position, Quaternion.identity, transform);

        streamObject.transform.rotation = chosenOrigin.rotation;
        streamObject.transform.forward = chosenOrigin.forward;
        return streamObject.GetComponent<Stream>();
    }

    private Transform ChooseOrigin()
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float forwardAngle = Vector3.Angle(forward, Vector3.down);
        float backwardAngle = Vector3.Angle(-forward, Vector3.down);
        float leftAngle = Vector3.Angle(-right, Vector3.down);
        float rightAngle = Vector3.Angle(right, Vector3.down);

        if (forwardAngle < backwardAngle && forwardAngle < leftAngle && forwardAngle < rightAngle)
            return originForward;
        else if (backwardAngle < forwardAngle && backwardAngle < leftAngle && backwardAngle < rightAngle)
            return originBackward;
        else if (leftAngle < forwardAngle && leftAngle < backwardAngle && leftAngle < rightAngle)
            return originLeft;
        else
            return originRight;
    }

    private void EndPour()
    {
        Debug.Log("PourDetector GameObject End Pour: " + this.gameObject);
        Destroy(waterSound);
        currentStream.End();
        if (checkHit)
        {
            dish.GetComponentInParent<PoisonStorer>().poisonRecipe = GetComponent<PoisonStorer>().poisonRecipe;
            Debug.Log(dish.GetComponentInParent<PoisonStorer>().poisonRecipe.poisonName);
            rend.enabled = false;
            GetComponent<PoisonStorer>().Reset();
            isEmpty = true;

            Instantiate(smoke, dish.transform.position, Quaternion.identity);
        }
    }

    private float CalculatePourAngle()
    {
        Vector3 localUp = transform.up;
        float angle = Vector3.Angle(Vector3.up, localUp);
        return angle;
    }

    private bool CheckForDishCollision()
    {
        Debug.Log("Check");
        RaycastHit hit;
        Ray ray = new Ray(raycastOrigin.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Debug.Log("Raycast hit: " + hit.collider.tag);
            if (hit.collider.CompareTag("Dish"))
            {
                dish = hit.collider.gameObject;
                Debug.Log("goinggggggggggggggggg");
                Debug.Log("should finish");

                return true;
            }
        }

        return false;
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(ChooseOrigin().transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 2.0f);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);

        return endPoint;
    }

}