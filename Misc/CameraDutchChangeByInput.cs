using Cinemachine;
using UnityEngine;

public class CameraDutchChangeByInput : MonoBehaviour
{
    public float dutchScale = 1f;
    public float speed = 1f;
    public float resetSpeed = 1f;

    CinemachineVirtualCamera cinemachineVirtualCamera;

    private void Awake()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if(Input.GetAxisRaw("Horizontal") != 0f)
        {
            cinemachineVirtualCamera.m_Lens.Dutch = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.Dutch, Input.GetAxis("Horizontal") * dutchScale, speed * Time.deltaTime);
        }
        else
        {
            cinemachineVirtualCamera.m_Lens.Dutch = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.Dutch, 0, resetSpeed * Time.deltaTime);
        }
    }
}
