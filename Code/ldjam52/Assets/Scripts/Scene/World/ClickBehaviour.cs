using UnityEngine;

namespace Assets.Scripts.Scene.World
{
    public class ClickBehaviour : MonoBehaviour
    {
        private void Update()
        {
            if (Input.touchCount < 2)
            {
                if (!Base.Core.Game.LockCameraMovement && Input.GetMouseButtonDown(0))
                {
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
