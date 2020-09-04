using UnityEngine;
using Cinemachine;

public class Portal : MonoBehaviour
{
    public GameObject otherPortal;
    public float forwardOffset = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<Player>())
        {
            Vector3 posDelta = otherPortal.transform.position - Player.instance.transform.position;
            int numVcams = CinemachineCore.Instance.VirtualCameraCount;
            Player.instance.transform.position = otherPortal.transform.position + otherPortal.transform.forward * forwardOffset;

            for (int i = 0; i < numVcams; ++i)
                CinemachineCore.Instance.GetVirtualCamera(i).OnTargetObjectWarped(Player.instance.transform, posDelta);
        }
    }
}
