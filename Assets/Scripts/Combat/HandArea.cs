using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HandArea : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Get the draggable object being dropped
        CardMover mover = eventData.pointerDrag.GetComponent<CardMover>();

        if (mover.locked)
        {
            return;
        }

        if (mover != null)
        {
            // Set this DropZone as the new parent of the dropped card
            mover.parentToReturnTo = this.transform;
        }
    }
}
