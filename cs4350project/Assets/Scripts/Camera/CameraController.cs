using UnityEngine;
using Cinemachine;
using System.Collections;

/// <summary>
/// Controls a Cinemachine camera. Is currently not a singleton.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    private Coroutine m_CurrRefollowCoroutine = null;

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
        if (m_CurrRefollowCoroutine != null)
        {
            StopCoroutine(m_CurrRefollowCoroutine);
            m_CurrRefollowCoroutine = null;
        }

        if (hardSet)
        {
            m_CurrRefollowCoroutine = StartCoroutine(ChangeFollow(toFollow));
        } else
        {
            virtualCam.enabled = true;
            virtualCam.Follow = toFollow;
        }
    }

    private IEnumerator ChangeFollow(Transform toFollow)
    {
        // disable the virtual camera to force the new position without any lerping
        virtualCam.enabled = false;
        SetWorldPosition(toFollow.position);
        yield return null;
        virtualCam.enabled = true;
        virtualCam.Follow = toFollow;
    }

    public void ResetFollow()
    {
        virtualCam.Follow = null;
    }
    #endregion
}