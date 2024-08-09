using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Starvation : MonoBehaviour
{
    [SerializeField] private Transform m_FoodIndicatorParent;

    private List<UI_FoodIndicator> m_FoodIndicators = new List<UI_FoodIndicator>();

    private void Awake()
    {
        GlobalEvents.Starvation.StarvationChangeEvent += OnStarvationChange;

        for (int i = 0; i < m_FoodIndicatorParent.childCount; ++i)
        {
            m_FoodIndicators.Add(m_FoodIndicatorParent.GetChild(i).GetComponent<UI_FoodIndicator>());
        }
        SetStarvationIndicators((int) StarvationManager.Instance.StarvationAmount);
    }

    private void OnDestroy()
    {
        GlobalEvents.Starvation.StarvationChangeEvent -= OnStarvationChange;
    }

    private void SetStarvationIndicators(int starvationLevel)
    {
        for (int i = 0; i < starvationLevel; ++i)
        {
            m_FoodIndicators[i].ToggleColor(false);
        }
        
        for (int i = starvationLevel; i < GlobalSettings.MaxStarvationLevel; ++i)
        {
            m_FoodIndicators[i].ToggleColor(true);
        }
    }

    

    private void OnStarvationChange(float starvationAmount)
    {
        SetStarvationIndicators((int) starvationAmount);
    }
}