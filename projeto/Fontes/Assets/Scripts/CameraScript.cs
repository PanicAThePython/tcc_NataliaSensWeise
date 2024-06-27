using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform target;
    private float x,z;
    private bool foward, left;   

    void Update () {
        transform.LookAt(target);

        if (!left && !foward)
            transform.GetComponent<RectTransform>().transform.position = new Vector3(transform.GetComponent<RectTransform>().transform.position.x + 1,
                transform.GetComponent<RectTransform>().transform.position.y,
                transform.GetComponent<RectTransform>().transform.position.z);

        if (left)
            transform.LookAt(target, Vector3.left);

        if (foward)
            transform.LookAt(target, Vector3.forward);

        if (x != transform.GetComponent<RectTransform>().transform.position.x)
        {
            transform.LookAt(target, Vector3.left);
            left = true;
            foward = !left;
        }
        else if (z != transform.GetComponent<RectTransform>().transform.position.z)
        {
            transform.LookAt(target, Vector3.forward);
            foward = true;
            left = !foward;
        }

        x = transform.GetComponent<RectTransform>().transform.position.x;
        z = transform.GetComponent<RectTransform>().transform.position.z;
    }
}
