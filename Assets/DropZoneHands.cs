using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZoneHands : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        // You can add hover effects here if you like
        Debug.Log("OnEnterDropZone");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Handle when pointer exits the drop zone
        Debug.Log("OnExitDropZone");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag.name + " dropped onto " + gameObject.name);

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            
            d.parentToReturnTo = this.transform;
            Debug.Log("Card successfully dropped in the DropZone.");
           
        }
    }
}
