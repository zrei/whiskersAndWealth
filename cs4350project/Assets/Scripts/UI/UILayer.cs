using UnityEngine;

public abstract class UILayer : MonoBehaviour
{
    // ADD YOUR OWN FIELDS HERE

    protected abstract void HandleOpen();

    protected abstract void HandleClose();
}