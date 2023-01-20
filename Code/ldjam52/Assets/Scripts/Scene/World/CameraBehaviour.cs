
using Assets.Scripts.Base;

using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Camera cam;

    public GameObject FarmButton;

    //Old Values
    //    private readonly float moveSpeed = 15f;
    //    private readonly float moveSpeedTouch = 0.125f;
    private readonly float moveSpeed = 10f;
    private readonly float moveSpeedTouch = 0.085f;
    private readonly float zoomSpeed = 10.0f;
    private readonly float zoomSpeedMouse = 80.0f;
    private readonly float zoomSpeedTouch = .5f;

    //private readonly float minFov = 15.0f;
    //private float maxFov = 120.0f;

    private Vector2 prevTouch = Vector2.zero;
    private (Vector2, Vector2) prevPinch;

    // Start is called before the first frame update
    void Start()
    {
        UpdateFarmButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Core.Game.LockCameraMovement)
        {

            bool zoomChanged = ZoomHandling();
            bool positionChanged = MoveHandling();
            if (zoomChanged || positionChanged)
            {
                UpdateFarmButton();
            }
        }
    }

    private bool MoveHandling()
    {
        //// Movement along x, z axis
        // Keys
        float moveX = Input.GetAxisRaw("Horizontal");

        float moveZ = Input.GetAxisRaw("Vertical");


        if (Core.Game.Options.IsMouseScreenEdgeScrollingEnabled)
        {
            //Camera angle
            float angle = (float)(cam.transform.rotation.y);

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
            return true;
        }
        return false;
    }

    private bool ZoomHandling()
    {
        //// Zoom
        float zoom = 0.0f;

        // Keys
        if (Input.GetKey(KeyCode.Q))
        {
            zoom = -zoomSpeed;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            zoom = zoomSpeed;
        }

        // Mouse
        if (Input.mouseScrollDelta.y != 0)
        {
            zoom = zoomSpeedMouse * Input.mouseScrollDelta.y;
        }

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
            //float newView = cam.fieldOfView + zoom * Time.deltaTime * Core.Game.Options.ZoomSensivity;
            //newView = Mathf.Max(minFov, newView);
            //newView = Mathf.Min(maxFov, newView);
            //cam.fieldOfView = newView;
            Vector3 newCamPos = cam.transform.position + cam.transform.forward * zoom * Time.deltaTime * Core.Game.Options.ZoomSensivity;
            if (newCamPos.y < 1)
            {
                return false;
            }
            cam.transform.position = newCamPos;
            return true;
        }
        return false;
    }

    private void UpdateFarmButton()
    {

        Vector3 target = GetCenterTarget();
        Vector3 v3Pos = Camera.main.WorldToViewportPoint(target);

        //if (v3Pos.z < Camera.main.nearClipPlane)
        //{
        //    FarmButton.SetActive(false);
        //    return;  // Object is behind the camera
        //}

        Debug.Log("v3Pos: " + v3Pos);
        if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f)
        {
            FarmButton.SetActive(false);
            return; // Object center is visible
        }

        FarmButton.SetActive(true);
        v3Pos.x -= 0.5f;  // Translate to use center of viewport
        v3Pos.y -= 0.5f;

        float fAngle = Mathf.Atan2(v3Pos.x, v3Pos.y);
        if (v3Pos.z < 0)
        {
            fAngle += Mathf.PI;
        }
        v3Pos.z = 0;
        FarmButton.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -fAngle * Mathf.Rad2Deg);

        v3Pos.x = (0.45f * Mathf.Sin(fAngle) + 0.5f) * Screen.width;  // Place on ellipse touc$$anonymous$$ng 
        v3Pos.y = (0.45f * Mathf.Cos(fAngle) + 0.5f) * Screen.height;  //   side of viewport
        v3Pos.z = Camera.main.nearClipPlane + 0.01f;  // Looking from neg to pos Z;
        //FarmButton.transform.position = Camera.main.ViewportToWorldPoint(v3Pos);
        FarmButton.transform.position = v3Pos;
    }

    private Vector3 GetCenterTarget()
    {
        Vector3 target;
        if (Core.Game.State.World.Farm != default)
        {
            target = new Vector3(Core.Game.State.World.Farm.Position.X, Core.Game.State.World.Farm.Position.Y, Core.Game.State.World.Farm.Position.Z);
        }
        else
        {
            var x = Core.Game.State.World.Width / 2;
            var z = Core.Game.State.World.Height / 2;
            target = new Vector3(x, 0, z);
        }
        return target;
    }

    public void CenterFarm()
    {
        Vector3 target = GetCenterTarget();

        float yDiff = cam.transform.position.y - target.y;
        float factor = yDiff / cam.transform.forward.y;

        cam.transform.position = target + cam.transform.forward * factor;

        UpdateFarmButton();
    }
}
