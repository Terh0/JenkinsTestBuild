using UnityEngine;
using System.Collections;
//Unity's Event Systems
using UnityEngine.EventSystems;
using System;

public class ItemData : MonoBehaviour,
                        IBeginDragHandler,
                        IDragHandler,
                        IPointerDownHandler,
                        IEndDragHandler
                        
{
    public Item item;
    public int amount = 1;
    public Transform originalParent;

    private CanvasGroup canvasGroup;
    private Vector2 offset;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
     if(item != null) //Check if item is valid
        {
            originalParent = transform.parent;//Store biological parent 
            transform.SetParent(originalParent.parent);//Grandparent is going to adopt item
            canvasGroup.blocksRaycasts = false;//Dont block raycasts
        }
    }

    public void OnDrag(PointerEventData eventData)
    {  
      if(item != null)  //Check if item in valid
        {
         transform.position = eventData.position - offset; //Drag the child 
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if(item != null) //Check if item is valid
        {
          offset = eventData.position - (Vector2)transform.position; //Compute our offset
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {        
        canvasGroup.blocksRaycasts = true;//Block the raycasts
        
        if(transform.parent.Equals(originalParent.parent))//If the Item is not dropped into a slot then return it back to a slot
        {
            transform.SetParent(originalParent);
            transform.position = originalParent.transform.position;
        }
    }
}
