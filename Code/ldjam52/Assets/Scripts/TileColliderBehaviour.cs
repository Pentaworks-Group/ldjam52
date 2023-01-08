using UnityEngine;

public class TileColliderBehaviour : MonoBehaviour
{
    TileBehaviour tileBehaviour;

    private void Awake()
    {
        tileBehaviour = transform.GetComponentInParent<TileBehaviour>();
    }

    private void OnMouseDown()
    {
        if (tileBehaviour != null)
        {
            tileBehaviour.ShowTileView();
        }
    }
}
