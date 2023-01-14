using UnityEngine.Events;

namespace Assets.Scripts.Scene.World.Clickables
{
    public abstract class ClickableItemBehaviour<T> : ClickableItemBehaviour
    {
        public UnityEvent<T> Clicked = new UnityEvent<T>();

        public T item;

        public override void Click()
        {
            this.Clicked.Invoke(this.item);
        }
    }
}
