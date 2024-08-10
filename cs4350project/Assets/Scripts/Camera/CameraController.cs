using UnityEngine;
using Cinemachine;

/// <summary>
/// Controls a Cinemachine camera. Is currently not a singleton.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    #region Position
    public void SetWorldPosition(Vector3 worldPosition)
    {
        virtualCam.transform.position = worldPosition;
    }

    public void SetWorldPosition(Vector2 worldPosition)
    {
        float zPos = virtualCam.transform.position.z;
        virtualCam.transform.position = new Vector3(worldPosition.x, worldPosition.y, zPos);
    }
    #endregion

    #region Follow
    public void SetFollow(Transform toFollow, bool hardSet = false)
    {
        if (hardSet)
        {
            virtualCam.Follow = null;
            SetWorldPosition(toFollow.position);
        }

        virtualCam.Follow = toFollow;
    }

    public void ResetFollow()
    {
        virtualCam.Follow = null;
    }
    #endregion
}