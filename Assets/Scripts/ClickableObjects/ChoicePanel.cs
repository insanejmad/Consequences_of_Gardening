using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanel : MonoBehaviour
{
    [SerializeField] private Button takeButton = null;
    [SerializeField] private Button inspectButton = null;
    [SerializeField] private Text itemNameLabel = null;

    private ClickableObject _target = null;
    private RectTransform _rectTransform = null;
    private string _startTakeButtonString = null;
    private Text _takeButtontextLabel = null;
    private Canvas _canvas = null;
    private RectTransform _canvasRectTransform = null;

    private const string _inventoryFullString = "Votre inventaire est plein.";

    public bool HaveCursorOn
    {
        get
        {
            if (!gameObject.activeSelf)
                return (false);
            Camera camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main;

            return (RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, Input.mousePosition, camera));
        }
    }

    public void Setup(ClickableObject target)
    {
        if (target == null || !target.IsInteractable)
            return;
        _target = target;
        PlacePanelOnScreen();
        takeButton.gameObject.SetActive(target.isTakeble && target.TakeConditions);
        UpdateTakeButton();
        inspectButton.gameObject.SetActive(target.IsInspectable);
        itemNameLabel.text = target.Item.ItemName;
        gameObject.SetActive(true);
    }


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvas = gameObject.GetComponentInParent<Canvas>();
        _canvasRectTransform = _canvas.GetComponent<RectTransform>();
        if (takeButton)
            _takeButtontextLabel = takeButton.GetComponentInChildren<Text>();
        if (_takeButtontextLabel)
            _startTakeButtonString = _takeButtontextLabel.text;
    }

    private void Start()
    {
        takeButton.onClick.AddListener(Take);
        inspectButton.onClick.AddListener(Inspect);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!HaveCursorOn && Input.GetMouseButtonDown(0))
            ResetPanel();
    }

    private void Take()
    {
        _target.Take();
        ResetPanel();
    }

    private void Inspect()
    {
        _target.Inspect();
        ResetPanel();
    }

    private void ResetPanel()
    {
        gameObject.SetActive(false);
        itemNameLabel.text = "";
        _target = null;
    }

    private void UpdateTakeButton()
    {
        takeButton.interactable = CanAddItemInInventory;
        _takeButtontextLabel.text = CanAddItemInInventory ? _startTakeButtonString : _inventoryFullString;
    }

    private bool CanAddItemInInventory
    {
        get => (PlayerInventory.instance.ItemList.Count < PlayerInventory.instance.maxItems);
    }

    private void PlacePanelOnScreen()
    {
        float xOverLap = 0f;
        float yOverLap = 0f;

        transform.position = Input.mousePosition;
        if (_rectTransform.anchoredPosition.x + _rectTransform.sizeDelta.x > _canvasRectTransform.sizeDelta.x / 2)
            xOverLap = _rectTransform.anchoredPosition.x + _rectTransform.sizeDelta.x - _canvasRectTransform.sizeDelta.x / 2;
        if (_rectTransform.anchoredPosition.y > _canvasRectTransform.sizeDelta.y / 2)
            yOverLap = _rectTransform.anchoredPosition.y - _canvasRectTransform.sizeDelta.y / 2;
        if (_rectTransform.anchoredPosition.y - _rectTransform.sizeDelta.y < -_canvasRectTransform.sizeDelta.y / 2)
            yOverLap = _rectTransform.anchoredPosition.y - _rectTransform.sizeDelta.y + _canvasRectTransform.sizeDelta.y / 2;
        _rectTransform.anchoredPosition -= new Vector2(xOverLap, yOverLap);
    }
}
