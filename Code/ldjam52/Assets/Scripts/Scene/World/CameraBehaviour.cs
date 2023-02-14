
using Assets.Scripts.Base;

using GameFrame.Core.Extensions;

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
    private readonly float moveSpeedMouseDrag = 1f;
    private readonly float zoomSpeed = 10.0f;
    private readonly float zoomSpeedMouse = 20.0f;
    private readonly float zoomSpeedTouch = 0.25f;

    //private readonly float minFov = 15.0f;
    //private float maxFov = 120.0f;

    private (UnityEngine.Vector2, UnityEngine.Vector2) prevPinch = default;
    private UnityEngine.Vector3 clickDown;

    public static int panTimeout = 0;
    private static bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {
        if (Core.Game.State.World.CameraPosition.HasValue)
        {
            cam.transform.position = Core.Game.State.World.CameraPosition.Value.ToUnity();
        }

        UpdateFarmButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Core.Game.LockCameraMovement)
        {
            if (!isMoving && panTimeout > 0)
            {
                panTimeout -= 1;
            }

            bool zoomChanged = ZoomHandling();
            bool positionChanged = MoveHandling();

            if (zoomChanged || positionChanged)
            {
                UpdateFarmButton();

                Core.Game.State.World.CameraPosition = cam.transform.position.ToFrame();
            }
        }
    }

    public static bool IsPanning()
    {
        return panTimeout >= 1;
    }

    private bool MoveHandling()
    {
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

        if (Input.GetMouseButtonDown(0))
        {
            clickDown = Input.mousePosition;
            isMoving = true;
        }
        else if (Input.touchCount == 2)
        {
            isMoving = false;
        }
        else if (isMoving && Input.GetMouseButton(0))
        {
            if (Input.mousePosition != clickDown)
            {
                moveX = (clickDown.x - Input.mousePosition.x);
                moveZ = (clickDown.y - Input.mousePosition.y);

                if (Application.isMobilePlatform)
                {
                    moveX *= moveSpeedTouch;
                    moveZ *= moveSpeedTouch;
                }
                else
                {
                    moveX *= moveSpeedMouseDrag;
                    moveZ *= moveSpeedMouseDrag;
                }

                clickDown = Input.mousePosition;
                panTimeout = 2;
            }
        }
        else if (isMoving && Input.GetMouseButtonUp(0))
        {
            isMoving = false;
        }

        if (moveX != 0 || moveZ != 0)
        {
            cam.transform.position += Core.Game.Options.MoveSensivity * Time.deltaTime * new UnityEngine.Vector3(moveX, 0, moveZ);

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

            if (prevPinch == default)
            {
                prevPinch = (touch1.position, touch2.position);
            }
            else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                zoom = zoomSpeedTouch * (Vector2.Distance(touch1.position, touch2.position) - Vector2.Distance(prevPinch.Item1, prevPinch.Item2));
                prevPinch = (touch1.position, touch2.position);
                panTimeout = 20;
            }
        }

        if (zoom != 0)
        {
            var newCamPos = cam.transform.position + Core.Game.Options.ZoomSensivity * Time.deltaTime * zoom * this.cam.transform.forward;

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
        var target = GetCenterTarget();
        var v3Pos = Camera.main.WorldToViewportPoint(target);

        //        Debug.Log("v3Pos: " + v3Pos);
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

        FarmButton.transform.localEulerAngles = new UnityEngine.Vector3(0.0f, 0.0f, -fAngle * Mathf.Rad2Deg);

        v3Pos.x = (0.45f * Mathf.Sin(fAngle) + 0.5f) * Screen.width;  // Place on ellipse touc$$anonymous$$ng 
        v3Pos.y = (0.45f * Mathf.Cos(fAngle) + 0.5f) * Screen.height;  //   side of viewport
        v3Pos.z = Camera.main.nearClipPlane + 0.01f;  // Looking from neg to pos Z;
        //FarmButton.transform.position = Camera.main.ViewportToWorldPoint(v3Pos);
        FarmButton.transform.position = v3Pos;
    }

    private UnityEngine.Vector3 GetCenterTarget()
    {
        UnityEngine.Vector3 target;

        var world = Core.Game.State.World;

        if (world.Farm != default)
        {
            target = new UnityEngine.Vector3(world.Farm.Position.X, world.Farm.Position.Y, world.Farm.Position.Z);
        }
        else
        {
            float x = world.Width / 2;
            float z = world.Height / 2;

            target = new UnityEngine.Vector3(x, 0, z);
        }

        return target;
    }

    public void CenterFarm()
    {
        var target = GetCenterTarget();

        float yDiff = cam.transform.position.y - target.y;
        float factor = yDiff / cam.transform.forward.y;

        cam.transform.position = target + cam.transform.forward * factor;
        panTimeout = 2;
        UpdateFarmButton();
    }

    public void ShowToFarmButton()
    {
        UpdateFarmButton();
    }

    public void HideToFarmButton()
    {
        this.FarmButton.SetActive(false);
    }
}
