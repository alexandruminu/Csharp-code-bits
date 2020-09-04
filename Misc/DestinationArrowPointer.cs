using UnityEngine;

/// <summary>
/// This script points a transform towards a position on one axis
/// </summary>
public class DestinationArrowPointer : MonoBehaviour
{
    public Transform target;
    public GameObject destinationSprite, arrow;

    private void Update()
    {
        if (!target) return;

        transform.LookAt(new Vector3(target.position.x, transform.position.y, target.position.z));
        transform.localEulerAngles = new Vector3(0f, 0f, 360f - transform.localEulerAngles.y);
    }

    public void SetTarget(Transform t)
    {
        destinationSprite.SetActive(t != null);
        arrow.SetActive(t != null);
        target = t;
    }
}
