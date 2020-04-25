using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TEMP


//Singleton Manager, holds info on which items are in the inventory
public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory instance = null;

    public Dictionary<string,Item> ItemList;

    public int maxItems = 3;

    public InventoryUIManager UImanager;


    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        ItemList = new Dictionary<string, Item>();
        UImanager.Initialize();
    }

    public bool ContainsItem(string name)
    {
        if (ItemList.ContainsKey(name))
        {
            return true;
        }
        else
        {
            return false;

        }
    }

    public Item GetItem(string name)
    {

        return ItemList[name];
    }

    public delegate void OnItemAddedEvent(Item item);
    public event OnItemAddedEvent OnItemAdded;


    public void AddItem(Item it)
    {
        //Debug.Log(it.name);
        if(ItemList.Count < maxItems)
        {
            ItemList.Add(it.name,it);
            OnItemAdded(it);
        }
        else
        {
            Debug.Log("Inventory Full");
        }
    }


    public delegate void OnItemRemovedEvent(Item item);
    public event OnItemRemovedEvent OnItemRemoved;


    public void RemoveItem(string name)
    {
        Item it = GetItem(name);
        ItemList.Remove(name);
        OnItemRemoved(it);
    }

}
