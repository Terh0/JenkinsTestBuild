using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class InventoryScript : MonoBehaviour
{
    [Header("UI")]
    public int slotAmount; // Amount of slots that will spawn
    [Header("Prefabs")] //Prefabs of the slot
    public GameObject slotPrefab;
    public GameObject itemPrefab;
    [Header("Items /Slots")]
    public List<ItemData> items = new List<ItemData>();
    public List<SlotData> slots = new List<SlotData>();

    public GameObject slotPanel; //Parent of the slots

	void Start ()
    {
        for (int i = 0; i < slotAmount; i++)//Creates slots for the items
        {
            GameObject clone = Instantiate(slotPrefab);//Creates the gameobject

            clone.transform.SetParent(slotPanel.transform); //Sets it as a child to the slotpanel
                        
            SlotData slotData = clone.GetComponent<SlotData>(); //gets the slot data from the clone
            slotData.inventory = this;
            
            slots.Add(slotData);
        }
	}
		
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Space))
        {
            AddItem("Steel Gloves");
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            AddItem("Iron Pickaxe");
        }
	}
    public void AddItem(string itemName, int itemAmount = 1)//Adds item to inventory
    {
        Item newItem = ItemDatabase.GetItem(itemName); //Get an item from the database
        
        if(newItem != null) //Check if item is valid
        {
            if(HasStacked(newItem,itemAmount))//Check to see if the item is stackable
            {
                return;
            }

            SlotData newSlot = GetEmptySlot(); //Gets an empty slot
            if (newSlot != null)
            {

            }
            GameObject item = Instantiate(itemPrefab);//Creates a new prefab of the item
            item.transform.position = newSlot.gameObject.transform.position; //Set new item's gameObject
            item.transform.SetParent(newSlot.gameObject.transform);
            item.name = newItem.Title;
            newItem.gameObject = item; //set the item's gameObject
            Image image = item.GetComponent<Image>();//Get the image component and set new image to item sprite
            image.sprite = newItem.Sprite; //Get ItemData Component
            ItemData itemData = item.GetComponent<ItemData>();
            itemData.item = newItem;
            
        } 
    }
    public SlotData GetEmptySlot() //Get the next slot thats empty in the inventory
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if(slots[i].transform.childCount == 0)
            {
                return slots[i];
            }
        }
        return null;
    }
    bool HasStacked(Item itemToAdd, int itemAmount = 1)
    {   //Check to see if the item is able to be stacked
        if(itemToAdd.Stackable)
        {
            SlotData occupiedSlot = GetSlotWithItem(itemToAdd); //Get the occupied slot with that item

            if(occupiedSlot != null) //Check if the spot is occupied
            {
                ItemData itemData = occupiedSlot.GetComponentInChildren<ItemData>();//Get the data from the occupied slots item
                itemData.amount += itemAmount; //Increase the amount of the items data
                Text textElement = itemData.GetComponentInChildren<Text>(); //Set the items text element to the amount
                textElement.text = itemData.amount.ToString();
                return true;
            }
        }
        return false;
    }
    SlotData GetSlotWithItem(Item item)
    {
        for (int i = 0; i < slots.Count; i++)//Loop through all the slots
        {
            ItemData currentItem = slots[i].GetComponentInChildren<ItemData>();//Get the current item in the slot
            if (currentItem != null && currentItem.item.Title == item.Title)//Check if the item exist in that slot and that the item is the same as the one coming into the function
            {
                return slots[i];//Return that slot
            }
        }
        return null;
    }
}
