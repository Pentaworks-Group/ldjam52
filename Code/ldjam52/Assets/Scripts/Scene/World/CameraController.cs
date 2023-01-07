using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera cam;

    private float moveSpeed = 100.0f;
    private float zoomSpeed = 5.0f;

    private float minFov = 10.0f;
    private float maxFov = 90.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // X, Z position
        cam.transform.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * Time.deltaTime * moveSpeed;

        // Zoom
        if (Input.GetKey(KeyCode.Q))
            cam.fieldOfView = Mathf.Max(minFov, cam.fieldOfView - zoomSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            cam.fieldOfView = Mathf.Min(maxFov, cam.fieldOfView + zoomSpeed * Time.deltaTime);
    }
}
