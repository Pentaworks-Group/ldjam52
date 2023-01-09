using UnityEngine;
using UnityEngine.Events;

public class TileColliderBehaviour : MonoBehaviour
{
    TileBehaviour tileBehaviour;

    private void Awake()
    {
        tileBehaviour = transform.GetComponentInParent<TileBehaviour>();
    }

    private void OnMouseUpAsButton()
    {
        if (tileBehaviour != null && (!Assets.Scripts.Base.Core.Game.LockCameraMovement))
        {
            tileBehaviour.OnClick.Invoke(tileBehaviour);
        }
    }
}
