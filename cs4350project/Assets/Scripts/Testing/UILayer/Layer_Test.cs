using UnityEngine.UI;
using UnityEngine;

public class Layer_Test : UILayer
{
    [SerializeField] private Button _closeButton;

    public override void HandleClose() {
        Logger.Log(this.GetType().Name, name, "Closed", gameObject, LogLevel.LOG);
        _closeButton.onClick.RemoveAllListeners();
    }

    public override void HandleOpen() {
        Logger.Log(this.GetType().Name, name, "Opened", gameObject, LogLevel.LOG);
        _closeButton.onClick.AddListener(CloseLayer);
    }
}