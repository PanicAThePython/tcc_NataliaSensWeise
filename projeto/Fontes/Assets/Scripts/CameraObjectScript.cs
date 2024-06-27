using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraObjectScript : MonoBehaviour {

    public Transform target;
    //public Camera camera;
		
	void Update () {
        transform.LookAt(target);
    }

    //public void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.cyan;

    //    Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
    //    Gizmos.DrawFrustum(Vector3.zero, camera.fieldOfView, camera.farClipPlane,
    //        camera.nearClipPlane, camera.aspect);
    //}
}
