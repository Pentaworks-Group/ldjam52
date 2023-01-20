
using Assets.Scripts.Base;

using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Camera cam;

    //Old Values
    //    private readonly float moveSpeed = 15f;
    //    private readonly float moveSpeedTouch = 0.125f;
    private readonly float moveSpeed = 10f;
    private readonly float moveSpeedTouch = 0.085f;
    private readonly float zoomSpeed = 10.0f;
    private readonly float zoomSpeedMouse = 80.0f;
    private readonly float zoomSpeedTouch = .5f;

    private readonly float minFov = 15.0f;
    private float maxFov = 120.0f;

    private Vector2 prevTouch = Vector2.zero;
    private (Vector2, Vector2) prevPinch;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!Core.Game.LockCameraMovement)
        {
            //// Movement along x, z axis
            // Keys
            float moveX = Input.GetAxisRaw("Horizontal");
            
            float moveZ = Input.GetAxisRaw("Vertical");
            

            if (Core.Game.Options.IsMouseScreenEdgeScrollingEnabled)
            {
                //Camera angle
                float angle = (float) (cam.transform.rotation.y);

                // Mouse
                if (Input.mousePosition.x <= Screen.width * 0.01f)
                {
                    moveX = 1.0f * Mathf.Sin(angle);
                }
                else if (Input.mousePosition.x >= Screen.width * 0.99f)
                {
                    moveX = -1.0f * Mathf.Sin(angle);
                }

                if (Input.mousePosition.y <= Screen.height * 0.01f)
                {
                    moveZ = -1.0f * Mathf.Cos(angle);
                }
                else if (Input.mousePosition.y >= Screen.height * 0.99f)
                {
                    moveZ = 1.0f * Mathf.Cos(angle);
                }
            }
            if (moveX != 0)
            {
                moveX *= moveSpeed;
            }
            if (moveZ != 0)
            {
                moveZ *= moveSpeed;
            }

            // Touch
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    this.prevTouch = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    moveX = (prevTouch.x - touch.position.x) * moveSpeedTouch;
                    moveZ = (prevTouch.y - touch.position.y) * moveSpeedTouch;
                }
            }

            if (moveX != 0 || moveZ != 0)
            {
                cam.transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * Core.Game.Options.MoveSensivity;
            }

            //// Zoom
            float zoom = 0.0f;

            // Keys
            if (Input.GetKey(KeyCode.Q))
            {
                zoom = -this.zoomSpeed;
            }

            //cam.fieldOfView = Mathf.Max(minFov, cam.fieldOfView - zoomSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.E))
            {
                zoom = zoomSpeed;
            }
            //cam.fieldOfView = Mathf.Min(maxFov, cam.fieldOfView + zoomSpeed * Time.deltaTime);

            // Mouse
            if (Input.mouseScrollDelta.y != 0)
            {
                zoom = zoomSpeedMouse * -Input.mouseScrollDelta.y;
            }

            //cam.fieldOfView = Mathf.Max(minFov, cam.fieldOfView - zoomSpeedMouse * Input.mouseScrollDelta.y * Time.deltaTime);
            //cam.fieldOfView = Mathf.Min(maxFov, cam.fieldOfView);

            // Touch
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);
                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                    prevPinch = (touch1.position, touch2.position);
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    zoom = -zoomSpeedTouch * (Vector2.Distance(touch1.position, touch2.position) - Vector2.Distance(prevPinch.Item1, prevPinch.Item2));
                }
            }
            if (zoom != 0)
            {
                float newView = cam.fieldOfView + zoom * Time.deltaTime * Core.Game.Options.ZoomSensivity;
                newView = Mathf.Max(minFov, newView);
                newView = Mathf.Min(maxFov, newView);
                cam.fieldOfView = newView;
            }
        }
    }

    public void CenterFarm()
    {
        if (Core.Game.State.World.Farm != default)
        {
            cam.transform.position = new UnityEngine.Vector3(Core.Game.State.World.Farm.Position.X, this.transform.position.y, Core.Game.State.World.Farm.Position.Z);
        }
    }
}
