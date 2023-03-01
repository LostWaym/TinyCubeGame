using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public Transform target;
    public Transform cam;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float followRatio = 0.8f;

    // Update is called once per frame
    void Update()
    {
        if (target == null ||cam == null)
            return;

        Vector3 targetPos = target.position + offset;
        cam.transform.position = Vector3.Slerp(cam.transform.position, targetPos, followRatio);
    }
}
