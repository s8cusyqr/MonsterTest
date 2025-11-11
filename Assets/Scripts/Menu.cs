using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Button[] _menuButtons;
    [SerializeField]
    private GameObject _settingsCanvas;
    [SerializeField]
    private GameObject _gameCanvas;
    [SerializeField]
    private Camera _cam;
    Vector3 defaultCamPos = new Vector3(0, 0, -10f);
    // Start is called before the first frame update
    void OnEnable()
    {
        _menuButtons[0].Select();
    }

    public void ExitClick() {
        Application.Quit();
    }

    public void SettingsClick() {
        _settingsCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    public void PlayClick() {
        _cam.backgroundColor = Color.green;
        _cam.transform.position = defaultCamPos;
        _gameCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
