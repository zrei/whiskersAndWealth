using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraFollower : MonoBehaviour
{
    [SerializeField] private GameObject m_ToFollow;

    private void Update()
    {
        Vector2 followPos = (Vector2) m_ToFollow.transform.position;
        Camera.main.transform.position = new Vector3(followPos.x, followPos.y, Camera.main.transform.position.z);
    }
}
