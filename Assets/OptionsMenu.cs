//Script ByEma per chiamare OptionsMenu con Esc
using UnityEngine;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private GameObject optionsMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateOptionsMenu();
        }
    }

    public void ActivateOptionsMenu()
    {
        if (optionsMenu != null)
            optionsMenu.SetActive(!optionsMenu.activeSelf);
    }
}