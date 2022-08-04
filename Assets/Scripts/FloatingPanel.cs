using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingPanel : MonoBehaviour
{
    [SerializeField] private Button _floatingButton;
    [SerializeField] private GameObject _floatingPanel;
    [Header("Sprites")]
    [SerializeField] private  Sprite _defaultSprite;
    [SerializeField] private Sprite _activeSprite;
    [Header("Text")]
    [SerializeField] private TextMeshPro _textMeshPro;

    private LineRenderer _lineRenderer;

    private void Awake()
    {
        ShowRoomController.OnCarChange += SetPanelData;
    }

    private void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        Image img = _floatingButton.GetComponent<Image>();
        _floatingPanel.SetActive(false);
        _lineRenderer.enabled = false;
        img.sprite = _defaultSprite;
        _floatingButton.onClick.RemoveAllListeners();
        _floatingButton.onClick.AddListener(ShowCarPrice);
    }

    private void ShowCarPrice()
    {
        _floatingPanel.SetActive(true);
        _lineRenderer.enabled = true;
        Image img = _floatingButton.GetComponent<Image>();
        img.sprite = _activeSprite;

        _floatingButton.onClick.RemoveAllListeners();
        _floatingButton.onClick.AddListener(DeactiveCarPrice);
    }

    private void DeactiveCarPrice()
    {
        _floatingPanel.SetActive(false);
        _lineRenderer.enabled = false;
        Image img = _floatingButton.GetComponent<Image>();
        img.sprite = _defaultSprite;


        _floatingButton.onClick.RemoveAllListeners();
        _floatingButton.onClick.AddListener(ShowCarPrice);
    }

    public void SetPanelData(CarDatabase.Car car)
    {
        string text = $"{car.carName} \n {car.price} $";

        _textMeshPro.text = text;
    }


}
