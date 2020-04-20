using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryItemDisplay : MonoBehaviour
{
    private Item item;
    Image Srenderer;

    public string hoverName;
    public string description;

    public GameObject descriptionobject = null;

    public GameObject TextPrefab;


    // Start is called before the first frame update
    void Awake()
    {
        Srenderer = gameObject.AddComponent<Image>();
        GetComponent<RectTransform>().sizeDelta = new Vector2(90f, 60f);
    }

    public void SetItem(Item itemtoset)
    {
        item = itemtoset;
        InitializeNewItem();
    }

    private void InitializeNewItem()
    {
        Srenderer.sprite = item.Sprite;
        hoverName = item.ItemName;
        description = item.InventoryDescription;
        transform.localScale = Vector3.one ;
        BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;

    }

    private void OnMouseDown()
    {
        if (descriptionobject)
        {
            Destroy(descriptionobject);
        }

        descriptionobject = Instantiate(TextPrefab) as GameObject;
        descriptionobject.transform.SetParent(this.transform);
        descriptionobject.transform.localPosition = transform.up * 0.25f;
        descriptionobject.transform.localScale = Vector3.one * 0.003f;
        Text text = descriptionobject.GetComponent<Text>();
        
        text.text = description;
    }

    private void OnMouseExit()
    {
        if (descriptionobject)
        {
            Destroy(descriptionobject);
        }
    }
}
