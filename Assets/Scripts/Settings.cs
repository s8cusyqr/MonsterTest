using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField]
    private Button _backButton;
    [SerializeField]
    private GameObject _menuCanvas;
    // Start is called before the first frame update
    void OnEnable()
    {
        _backButton.Select();
    }

    public void BackClick() {
        _menuCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
}
