using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClickableObject : MonoBehaviour
{
    [SerializeField] private bool isTakeble = true;
    [SerializeField] private GameObject highlightObject = null;

    private bool _isHovered = false;

    private const float _takeAnimationTime = 0.2f;

    private void Awake()
    {
        if (!GetComponent<Collider2D>()) {
            Debug.LogWarning(name + " need a 2D collider");
            gameObject.AddComponent<Collider2D>();
        }
	HoveredOff();
    }

    private void Update()
    {
	if (_isHovered && isTakeble && Input.GetMouseButtonDown(0))
	{
	    ClickableObjectManager.instance.OpenPanel(this);
	}
    }

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
    }

    public void Inspect()
    {
	Debug.Log(name + " Inspected");
    }

    public bool IsHovered
    {
        get => _isHovered;
    }

    private void OnMouseOver()
    {
	if (!isTakeble)
	    return;
	_isHovered = true;
	HoveredOn();
    }

    private void OnMouseExit()
    {
	if (!isTakeble)
	    return;
	_isHovered = false;
	HoveredOff();
    }

    private void HoveredOff()
    {
	if (highlightObject)
	    highlightObject.SetActive(false);
    }

    private void HoveredOn()
    {
	if (highlightObject)
	    highlightObject.SetActive(true);
    }
}
