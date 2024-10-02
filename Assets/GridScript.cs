using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour
{
    public RectTransform handTransform;   // Reference to the Hand Panel (should be a RectTransform of the Canvas)
    public GameObject cardSlotPrefab;     // Prefab for the empty slot
    public int maxSlots = 3;              // Maximum slots (3 in this case)

    void Start()
    {
        CreateEmptySlots();
    }

    void CreateEmptySlots()
    {
        float handWidth = handTransform.rect.width;
        float slotWidth = cardSlotPrefab.GetComponent<RectTransform>().rect.width;
        float slotSpacing = (handWidth - slotWidth * maxSlots) / (maxSlots + 1);  // Spacing between slots

        for (int i = 0; i < maxSlots; i++)
        {
            // Instantiate an empty slot (DropZone)
            GameObject slotInstance = Instantiate(cardSlotPrefab, handTransform);
            RectTransform slotRectTransform = slotInstance.GetComponent<RectTransform>();
            slotRectTransform.localScale = Vector3.one;

            // Set the slot's position relative to the hand panel
            float slotXPosition = (-handWidth / 2) + slotSpacing * (i + 1) + slotWidth * i;
            slotRectTransform.anchoredPosition = new Vector2(slotXPosition, 0);
        }
    }
}
