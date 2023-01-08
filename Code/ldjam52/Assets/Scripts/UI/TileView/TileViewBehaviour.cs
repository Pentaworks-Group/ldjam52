using System;

using UnityEngine;

namespace Assets.Scripts.UI.TileView
{
    public class TileViewBehaviour : MonoBehaviour
    {
        private GameObject visiblityContainer;

        public void Show()
        {
            SetVisibility(true);
        }

        public void Hide()
        {
            SetVisibility(false);
        }

        private void SetVisibility(Boolean isVisible)
        {
            Base.Core.Game.LockCameraMovement = true;

            this.visiblityContainer.SetActive(isVisible);
        }

        private void Start()
        {
            this.visiblityContainer = transform.Find("TileViewToggle").gameObject;
        }

        private void Update()
        {

        }
    }
}
