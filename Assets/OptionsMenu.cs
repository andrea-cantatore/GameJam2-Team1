using UnityEngine;
using UnityEngine.Serialization;

public class OptionsMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _optionsMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateOptionsMenu();
        }
    }

    public void ActivateOptionsMenu()
    {
        if (_optionsMenu != null)
        {
            _optionsMenu.SetActive(!_optionsMenu.activeSelf);
            Time.timeScale = _optionsMenu.activeSelf ? 0 : 1;
        }
    }
}