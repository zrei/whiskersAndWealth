using UnityEngine;
using UnityEngine.UI;

public class UI_TransitionScene : UILayer
{
    [SerializeField] private Image m_ColouredImage;

    private void Awake()
    {
        GlobalEvents.Map.OnMapLoadProgress += OnMapLoadProgress;
    }

    private void OnDestroy()
    {
        GlobalEvents.Map.OnMapLoadProgress -= OnMapLoadProgress;
    }

    private void OnMapLoadProgress(float progress)
    {
        m_ColouredImage.fillAmount = progress;
    }

    public override void HandleClose()
    {
        
    }

    public override void HandleOpen()
    {
        
    }
}