using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Prefabs.World
{
    public class ViewBaseBehaviour : MonoBehaviour
    {
        public UnityEvent OnHide = new UnityEvent();
        bool isShown = false;

        public virtual void Show()
        {
            Base.Core.Game.LockCameraMovement = true;
            Assets.Scripts.Base.Core.Game.PlayButtonSound();
            isShown = true;
        }

        public virtual void Hide()
        {
            Base.Core.Game.LockCameraMovement = false;
            Assets.Scripts.Base.Core.Game.PlayButtonSound();

            OnHide.Invoke();
            isShown = false;
        }

        private void Start()
        {
            OnStart();
        }

        private void Update()
        {
            OnUpdate();
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnEscapePressed();
            }
        }

        protected virtual void OnEscapePressed()
        {
            if (isShown)
            {
                Hide();
            }            
        }
    }
}
