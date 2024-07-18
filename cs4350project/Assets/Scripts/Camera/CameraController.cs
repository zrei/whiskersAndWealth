using UnityEngine;
using Cinemachine;

// hm...
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    public void SetWorldPosition(Vector3 worldPosition)
    {
        virtualCam.transform.position = worldPosition;
    }

    public void SetWorldPosition(Vector2 worldPosition)
    {
        float zPos = virtualCam.transform.position.z;
        virtualCam.transform.position = new Vector3(worldPosition.x, worldPosition.y, zPos);
    }

    public void SetFollow(Transform toFollow, bool hardSet = false)
    {
        virtualCam.Follow = toFollow;

        if (hardSet)
            SetWorldPosition(toFollow.position);
    }

    public void ResetFollow()
    {
        virtualCam.Follow = null;
    }
}