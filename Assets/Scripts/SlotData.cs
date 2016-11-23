using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class SlotData : MonoBehaviour,
                        IDropHandler
{
    public InventoryScript inventory;
    //public Slot slot;

    public void OnDrop(PointerEventData eventData)
    {
        // Obtain the dropped item gameobject
        GameObject droppedItem = eventData.pointerDrag;
        //Get the itemData from droppedItem
        ItemData droppedItemData = droppedItem.GetComponent<ItemData>();
        //Check if the slot is empty
        if (transform.childCount == 0)
        {
            //set dropped item to be in this slot
            droppedItem.transform.SetParent(this.transform);
            droppedItem.transform.position = this.transform.position;
        }
        else //Slot is not Empty
        {
            //Get the current item in this slot
            ItemData currentItem = GetComponentInChildren<ItemData>();

            //Swap the currentItem with droppedItem
            currentItem.transform.SetParent(droppedItemData.originalParent);
            currentItem.transform.position = droppedItemData.originalParent.position;

            droppedItemData.transform.SetParent(this.transform);
            droppedItemData.transform.position = this.transform.position;
        }
    }
}
