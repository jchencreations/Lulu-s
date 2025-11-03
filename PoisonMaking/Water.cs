using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject water;
    public GameObject waterMesh;

    public int speed = 30;
    public int rotation = 15;

    public int maxAngle = 25;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        AdjustWaterLevel();
        waterMesh.transform.Rotate(Vector3.up * rotation * Time.deltaTime, Space.Self);
    }

    private void AdjustWaterLevel()
    {
        Quaternion invRotation = Quaternion.Inverse(transform.localRotation);
        Quaternion targetRotation = Quaternion.RotateTowards(water.transform.localRotation, invRotation, speed * Time.deltaTime);

        Vector3 waterRotation = targetRotation.eulerAngles;
        waterRotation.x = ClampAngle(waterRotation.x, maxAngle);
        waterRotation.z = ClampAngle(waterRotation.z, maxAngle);

        water.transform.localEulerAngles = waterRotation;
    }

    private float ClampAngle(float angle, float maxDifference)
    {
        if (angle > 180) angle -= 360;
        return Mathf.Clamp(angle, -maxDifference, maxDifference);
    }
}
