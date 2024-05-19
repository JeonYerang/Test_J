using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform playerTarget;

    private float cameraY;
    private float cameraZ;

    private void Awake()
    {
        cameraY = transform.position.y;
        cameraZ = transform.position.z;
    }

    private void Update()
    {
        if (playerTarget == null)
        {
            return;
        }

        transform.position = new Vector3(
            playerTarget.position.x, cameraY, playerTarget.position.z + cameraZ);
    }

    public void SetTarget(Transform target)
    {
        playerTarget = target;
    }
}
