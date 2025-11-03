using System.Collections;
using UnityEngine;

public class PourDetector : MonoBehaviour
{
    public int pourThreshold = 35;
    public Transform origin = null;
    public Transform raycastOrigin = null;
    public GameObject streamPrefab = null;
    public GameObject waterSoundPrefab;
    private GameObject waterSound;

    private bool isPouring = false;
    private bool pourCheck = false;
    private Stream currentStream = null;

    private bool checkHit = false;

    [SerializeField] private Cauldron cauldron;
    public Material material;


    public FMODUnity.EventReference glassColliding;
    public FMOD.Studio.EventInstance glassCollidingInstance;

    private void Start()
    {
        glassCollidingInstance = FMODUnity.RuntimeManager.CreateInstance(glassColliding);
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(glassCollidingInstance, transform);
    }
    private void Update()
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
    }


    private void StartPour()
    {
        currentStream = CreateStream();
        Vector3 targetPosition = FindEndPoint();
        waterSound = Instantiate(waterSoundPrefab, targetPosition, Quaternion.identity);
        checkHit = CheckForCauldronCollision();
        currentStream.Begin(false);

        //cauldron.GetComponent<Cauldron>().makeWaterSound();
    }

    private void EndPour()
    {
        Debug.Log("PourDetector GameObject End Pour: " + this.gameObject);
        Destroy(waterSound);
        currentStream.End();
        if(checkHit) cauldron.AddPoison(this.gameObject);

        //cauldron.GetComponent<Cauldron>().stopWaterSound();
    }

    private float CalculatePourAngle()
    {
        Vector3 localUp = transform.up;
        float angle = Vector3.Angle(Vector3.up, localUp);
        return angle;
    }

    private Stream CreateStream()
    {
        GameObject streamObject = Instantiate(streamPrefab, origin.position, Quaternion.identity, transform);
        return streamObject.GetComponent<Stream>();
    }

    private bool CheckForCauldronCollision()
    {
        Debug.Log("Check");
        RaycastHit hit;
        Ray ray = new Ray(raycastOrigin.transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log("Raycast hit: " + hit.collider.tag);
            return hit.collider.CompareTag("Cauldron");
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("PlayerHand"))
        {
            glassCollidingInstance.start();
        }
    }

    private Vector3 FindEndPoint()
    {
        RaycastHit hit;
        Ray ray = new Ray(raycastOrigin.transform.position, Vector3.down);
        Physics.Raycast(ray, out hit, 2.0f);
        Vector3 endPoint = hit.collider ? hit.point : ray.GetPoint(2.0f);

        return endPoint;
    }

}