using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : AClickableObject
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    public bool isTakeble = true;
    public bool isInspectable = true;
    [SerializeField] private GameObject highlightObject = null;
    [SerializeField] private Item item = null;
    [Header("Events")]
    [SerializeField] private UnityEvent onTake = null;
    [SerializeField] private UnityEvent onInspect = null;

    private const float _takeAnimationTime = 0.2f;

    public bool IsInteractable
    {
        get => (isInspectable || isTakeble);
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
    }

    #region INTERACTIONS
    public void Take()
    {
        isTakeble = false;
        if (onTake != null)
            onTake.Invoke();
        StartCoroutine(TakeAnimation());
    }

    public IEnumerator TakeAnimation()
    {
        Vector2 startPosition = transform.position;
        Vector2 endPosition = ClickableObjectManager.instance.GetItemTargetPos;

        for (float t = 0; t < _takeAnimationTime; t += Time.deltaTime) {
            transform.position = Vector2.Lerp(startPosition, endPosition, t / _takeAnimationTime);
            yield return null;
        }
        gameObject.SetActive(false);
        if (item)
            PlayerInventory.instance.AddItem(item);
    }

    public void Inspect()
    {
        if (onInspect != null)
            onInspect.Invoke();
        Debug.Log(name + " Inspected");
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
