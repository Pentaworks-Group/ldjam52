﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Scene.World
{
    public class ClickBehaviour : MonoBehaviour
    {
        private Vector2 prevTouch = Vector2.zero;
        private void Update()
        {
            if (Input.touchCount < 1 || Input.touchCount == 1 && Input.GetTouch(0).phase != TouchPhase.Moved)
            {
                if (!Base.Core.Game.LockCameraMovement && Input.GetMouseButtonDown(0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())    // is the touch on the GUI
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
}
