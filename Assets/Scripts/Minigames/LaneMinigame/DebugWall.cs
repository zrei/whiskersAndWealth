using UnityEngine;

public class DebugWall : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Wall collision");
    }
}
