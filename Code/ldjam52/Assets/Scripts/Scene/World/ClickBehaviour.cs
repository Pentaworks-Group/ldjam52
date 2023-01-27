using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Scene.World
{
    public class ClickBehaviour : MonoBehaviour
    {
        private void LateUpdate()
        {
            if (!CameraBehaviour.IsPanning())
            //if (Input.touchCount < 1 || panTimeout < 1)
            {
                if (Input.GetMouseButtonUp(0)) //!Base.Core.Game.LockCameraMovement && 
                {

                    if (!EventSystem.current.IsPointerOverGameObject())    // is the touch on the GUI
                    {
                        Debug.Log("Click triggered");

                        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                        if (Physics.Raycast(ray, out var raycastHit, 100.0f))
                        {
                            if (raycastHit.transform.gameObject != null)
                            {
                                var hitItem = raycastHit.transform.gameObject.GetComponentInParent<ClickableItemBehaviour>();

                                if (hitItem != null)
                                {
                                    hitItem.Click();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
