using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public CardData cardData; // Reference to the CardData ScriptableObject

    private void Start()
    {
        if (cardData != null)
        {
            Debug.Log($"Card Name: {cardData.cardName}, Type: {cardData.cardType}");
        }
    }
}
