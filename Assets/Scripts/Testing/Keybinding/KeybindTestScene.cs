using System.Collections;
using UnityEngine;

public class KeybindTestScene : MonoBehaviour
{
    [SerializeField] private UI_KeybindMenu m_KeybindMenu;
    [SerializeField] private string m_CurrControlScheme;

    private void Awake()
    {
        StartCoroutine(InitKeybindMenu());
    }

    IEnumerator InitKeybindMenu()
    {
        yield return new WaitForSeconds(1f);

        m_KeybindMenu.HandleOpen(m_CurrControlScheme);
    }
}