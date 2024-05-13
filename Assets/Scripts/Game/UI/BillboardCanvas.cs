using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    void Update()
    {
        //transform.LookAt(Camera.main.transform.position, Camera.main.transform.up);

        transform.forward = Camera.main.transform.position - transform.position;
        transform.up = Camera.main.transform.up;
    }
}
