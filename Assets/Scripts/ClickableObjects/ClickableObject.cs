using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DialogSystem;
using Lib.Struct;

public class ClickableObject : AClickableObject
{
    public bool isTakeble = true;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Item item = null;
    [Header("Optional")]
    [SerializeField] private GameObject highlightObject = null;
    [Header("CustomEvents")]
    [SerializeField] private UnityEvent onTake = null;
    [SerializeField] private UnityEvent onInspect = null;

    private const float _takeAnimationTime = 0.2f;

    public bool IsInspectable
    {
        get => (item && item.InspectDialog);
    }

    public bool IsInteractable
    {
        get => (IsInspectable || isTakeble);
    }

    public Item Item
    {
        get => item;
    }

    protected override void Awake()
    {
        base.Awake();
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (item == null) {
            Debug.LogError(name + " need an item \\o/");
            return;
        }
        HoverOff();
    }

    private void Start()
    {
        SetupItem();
    }

    private void SetupItem()
    {
        if (item == null)
            return;
        if (item.Sprite != null)
            spriteRenderer.sprite = item.Sprite;
    }

    private void Update()
    {
        if (_isHovered && ClickableObjectManager.HaveInstance) {
            if (ClickableObjectManager.instance.CursorOnChoicePanel)
                HoverOff();
        }
        if (PlayerInventory.instance && item)
            gameObject.SetActive(!PlayerInventory.instance.ItemList.ContainsValue(item));
    }

    #region INTERACTIONS
    public void Take()
    {
        if (onTake != null)
            onTake.Invoke();
        if (!_takeAnimationLocker)
            StartCoroutine(TakeAnimation());
    }

    private bool _takeAnimationLocker = false;
    public IEnumerator TakeAnimation()
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = ClickableObjectManager.instance.GetItemTargetPos;

        _takeAnimationLocker = true;
        for (float t = 0; t < _takeAnimationTime; t += Time.deltaTime) {
            transform.position = Vector2.Lerp(startPosition, endPosition, t / _takeAnimationTime);
            yield return null;
        }
        if (item && PlayerInventory.instance) {
            if (!PlayerInventory.instance.ItemList.ContainsValue(item))
                PlayerInventory.instance.AddItem(item);
        }
        gameObject.SetActive(false);
        _takeAnimationLocker = false;
    }

    public void Inspect()
    {
        if (onInspect != null)
            onInspect.Invoke();
        if (!IsInspectable) {
            Debug.LogError("item dialog not found");
            return;
        }
        if (UIDialogManager.Instance)
            UIDialogManager.Instance.Dialog = item.InspectDialog;
        else
            Debug.LogError("No instance of UIDialogManager");
    }

    #endregion

    #region HOVER_SYSTEM
    protected override void HoverOff()
    {
        if (highlightObject)
            highlightObject.SetActive(false);
    }

    protected override void HoverOn()
    {
        if (!IsInteractable) {
            _isHovered = false;
            return;
        }
        if (highlightObject)
            highlightObject.SetActive(true);
    }

    protected override void ClickOn()
    {
        if (ClickableObjectManager.HaveInstance)
            ClickableObjectManager.instance.OpenChoicePanel(this);
    }
    #endregion
}
