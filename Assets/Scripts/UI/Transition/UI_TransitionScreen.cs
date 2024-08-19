using UnityEngine;
using UnityEngine.UI;

public class UI_TransitionScene : UILayer
{
    [Header("UI References")]
    [SerializeField] private Image m_GreyedImage;

    #region Initialisation
    private void Awake()
    {
        GlobalEvents.Map.MapLoadProgressEvent += OnMapLoadProgress;
    }

    private void OnDestroy()
    {
        GlobalEvents.Map.MapLoadProgressEvent -= OnMapLoadProgress;
    }
    #endregion

    #region Event Callbacks
    private void OnMapLoadProgress(float progress)
    {
        m_GreyedImage.fillAmount = 1f - progress;
    }
    #endregion

    #region Interactions
    public override void HandleClose()
    {
        
    }

    public override void HandleOpen()
    {
        
    }

    public override void HandleUISelect()
    {

    }
    #endregion
}