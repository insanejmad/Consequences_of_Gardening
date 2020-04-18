using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer = null;
    [Header("Options")]
    public bool isTakeble = true;
    public bool isInspectable = true;
    [SerializeField] private GameObject highlightObject = null;
    [SerializeField] private Item item = null;

    private bool _isHovered = false;

    private const float _takeAnimationTime = 0.2f;

    private void Awake()
    {
        if (!GetComponent<Collider2D>()) {
            Debug.LogWarning(name + " need a 2D collider :c", this);
            gameObject.AddComponent<Collider2D>();
        }
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
	//Wait PlayerInventoryInstantiationInAwaik
	//if (PlayerInventory.instance.ItemList.Contains(Item))
	//    gameObject.SetActive(false);
	
    }

    private void Update()
    {
	if (_isHovered)
	{
	    if (ClickableObjectManager.instance.CursorOnChoicePanel)
		HoverOff();
	    if (Input.GetMouseButtonDown(0))
		ClickableObjectManager.instance.OpenChoicePanel(this);
	}
    }

    public Item Item
    {
	get => item;
    }

    public bool IsInteractable
    {
	get => (isInspectable || isTakeble);
    }

    #region INTERACTIONS
    public void Take()
    {
	isTakeble = false;
	StartCoroutine(TakeAnimation());
    }

    public IEnumerator TakeAnimation()
    {
	Vector2 startPosition = transform.position;
	Vector2 endPosition = ClickableObjectManager.instance.GetItemTargetPos;

	for (float t = 0; t < _takeAnimationTime; t += Time.deltaTime)
	{
	    transform.position = Vector2.Lerp(startPosition, endPosition, t / _takeAnimationTime);
	    yield return null;
	}
	gameObject.SetActive(false);
	if (item)
	    PlayerInventory.instance.AddItem(item);
    }

    public void Inspect()
    {
	Debug.Log(name + " Inspected");
    }
    #endregion

    #region HOVER_SYSTEM
    public bool IsHovered
    {
        get => _isHovered;
    }

    private void OnMouseOver()
    {
	if (!IsInteractable || ClickableObjectManager.instance.CursorOnChoicePanel)
	    return;
	HoverOn();
    }

    private void OnMouseExit()
    {
	if (!IsInteractable)
	    return;
	HoverOff();
    }

    private void HoverOff()
    {
	_isHovered = false;
	if (highlightObject)
	    highlightObject.SetActive(false);
    }

    private void HoverOn()
    {
	_isHovered = true;
	if (highlightObject)
	    highlightObject.SetActive(true);
    }
    #endregion
}
