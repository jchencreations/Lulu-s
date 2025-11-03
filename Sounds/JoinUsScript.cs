using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinUsScript : MonoBehaviour
{
    public GameObject camera;
    public GameObject joinUsSourcePreFab;
    private GameObject joinUsSource;
    private Vector3 cameraPostion;
    private Vector3 soundPosition;
    private float randX,randY,randZ;
    private List<GameObject> soundlist = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        cameraPostion = camera.transform.position;
        StartCoroutine(spaceTime());

    }

    // Update is called once per frame
    private void GenerateRandomPosition()
    {
        //set obj emitter location
        randX = Random.value * 20 + cameraPostion.x - 10;
        randY = Random.value * 20 + cameraPostion.y - 10;
        randZ = Random.value * 10 + cameraPostion.z;

        soundPosition = new Vector3(randX, randY, randZ);

        joinUsSource = Instantiate(joinUsSourcePreFab, soundPosition, Quaternion.identity);

        Debug.Log(soundPosition);

        soundlist.Add(joinUsSource);

    }

    private IEnumerator spaceTime()
    {
        while (true)
        {
            float randTime = Random.value*1;
            GenerateRandomPosition();
            yield return new WaitForSeconds(randTime);
            Destroy(soundlist[0]);
            soundlist.RemoveAt(0);
        }
    }
}