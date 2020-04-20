using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DialogSystem;
using Lib.Struct;
using GameObjectBehavior;

[RequireComponent(typeof(Interactable))]
public class ClickableObject : MonoBehaviour
{
    public bool isTakeble = true;
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [SerializeField] private Item item = null;
    [SerializeField] private string itemNeeded = null;
    [SerializeField] private string characterToBeDead = null;
    [SerializeField] private Dialog conditionsFailDialog = null;
    [Header("CustomEvents")]
    [SerializeField] private UnityEvent onTake = null;
    [SerializeField] private UnityEvent onInspect = null;

    private Interactable _interactable = null;

    private const float _takeAnimationTime = 0.2f;

    public bool IsInspectable
    {
        get => (item && item.InspectDialog);
    }

    public bool IsInteractable
    {
        get => (IsInspectable || (isTakeble && TakeConditions));
    }

    public bool TakeConditions
    {
        get
        {
            bool itemCondition = true;
            bool characterCondition = true;

            if (itemNeeded != null && itemNeeded != "")
                itemCondition = PlayerInventory.instance.ContainsItem(itemNeeded);
            if (characterToBeDead != null && characterToBeDead != "") {
                //characterCondition = WIP;
            }

            return (itemCondition && characterCondition);
        }
    }

    public Item Item
    {
        get => item;
    }

    private void Awake()
    {
        _interactable = GetComponent<Interactable>();
        _interactable.OnInteracted.AddListener(ClickOn);
        if (!spriteRenderer)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (item == null) {
            Debug.LogError(name + " need an item \\o/");
            return;
        }
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
        if (PlayerInventory.instance && item)
            gameObject.SetActive(!PlayerInventory.instance.ContainsItem(item.name));
    }

    #region INTERACTIONS
    public void Take()
    {
        if (!isTakeble || !TakeConditions)
            return;
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
	if (UIDialogManager.Instance) {
	    if (TakeConditions)
		UIDialogManager.Instance.Dialog = item.InspectDialog;
	    else
		UIDialogManager.Instance.Dialog = conditionsFailDialog;
	}
	else
	    Debug.LogError("No instance of UIDialogManager");
    }

    #endregion

    private void ClickOn()
    {
        if (UIDialogManager.Instance.InDialog)
            return;
        if (ClickableObjectManager.HaveInstance)
            ClickableObjectManager.instance.OpenChoicePanel(this);
    }
}
