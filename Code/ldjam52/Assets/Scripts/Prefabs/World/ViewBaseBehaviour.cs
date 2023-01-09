using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Prefabs.World
{
    public class ViewBaseBehaviour : MonoBehaviour
    {
        public UnityEvent OnHide = new UnityEvent();

        public virtual void Show()
        {
            Base.Core.Game.LockCameraMovement = true;
            Assets.Scripts.Base.Core.Game.PlayButtonSound();
        }

        public virtual void Hide()
        {
            Base.Core.Game.LockCameraMovement = false;
            Assets.Scripts.Base.Core.Game.PlayButtonSound();

            OnHide.Invoke();
        }
    }
}
