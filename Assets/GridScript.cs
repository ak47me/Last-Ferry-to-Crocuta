using UnityEngine;
using UnityEngine.UI;

public class GridScript : MonoBehaviour
{
    public GameObject[,] board = new GameObject[3, 3];
    public RectTransform boardTransform;
    public GameObject cardSlotPrefab;
    public Button completeButton;  // Reference to the button
    public int rows = 3;
    public int columns = 3;
    public float horizontalSpacing = 20f;
    public float verticalSpacing = 20f;
    public CardDisplay[] cardDisplays;
    public Button attackButton;


    void Start()
    {
        CreateGrid();

        // Add the button click listener
        completeButton.onClick.AddListener(OnAllCardsPlaced);
        attackButton.onClick.AddListener(HandleAttack);
    }

    void CreateGrid()
    {
        float boardWidth = boardTransform.rect.width;
        float boardHeight = boardTransform.rect.height;

        // Calculate the available space for each slot
        float totalHorizontalSpacing = horizontalSpacing * (columns - 1);
        float totalVerticalSpacing = verticalSpacing * (rows - 1);

        float availableWidth = boardWidth - totalHorizontalSpacing;
        float availableHeight = boardHeight - totalVerticalSpacing;

        // Calculate the size of each slot dynamically based on the available space
        float slotWidth = availableWidth / columns;
        float slotHeight = availableHeight / rows;

        // Center the grid within the board
        float startX = -((slotWidth * columns) + totalHorizontalSpacing) / 2 + slotWidth / 2;
        float startY = ((slotHeight * rows) + totalVerticalSpacing) / 2 - slotHeight / 2;

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                // Instantiate an empty slot (DropZone)
                GameObject slotInstance = Instantiate(cardSlotPrefab, boardTransform);
                RectTransform slotRectTransform = slotInstance.GetComponent<RectTransform>();
                slotRectTransform.localScale = Vector3.one;

                // Set the size of the slot dynamically
                slotRectTransform.sizeDelta = new Vector2(slotWidth, slotHeight);

                // Set the slot's position relative to the board panel
                float slotXPosition = startX + (slotWidth + horizontalSpacing) * column;
                float slotYPosition = startY - (slotHeight + verticalSpacing) * row;

                slotRectTransform.anchoredPosition = new Vector2(slotXPosition, slotYPosition);

                // Assign the slot a position within the grid for future reference
                DropZone dropZone = slotInstance.GetComponent<DropZone>();
                dropZone.SetGridPosition(row, column);

                // Store the slot in the singleton board
                board[row, column] = slotInstance;
            }
        }
    }


    public void OnAllCardsPlaced()
    {
        bool allSlotsFilled = true;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                DropZone dropZone = board[row, col].GetComponent<DropZone>();
                if (dropZone.currentCard == null)
                {
                    allSlotsFilled = false;
                    break;
                }
                HandleAbility(dropZone.currentCard);
                //Draggable draggable = dropZone.currentCard.GetComponent<Draggable>();
                //if (dropZone.currentCard.cardType == CardData.CardType.Character)
                //{

                //}
            }
        }

        if (!allSlotsFilled)
        {
            Debug.Log("Not all slots are filled!");
        }
        else
        {
            Debug.Log("All cards placed, abilities activated!");
        }
    }

    private void HandleAbility(CardData cardData)
    {
        switch (cardData.ability)
        {
            case "Knight":
                PerformAttackBuff(cardData);
                break;
            case "FrontKnifeBuff":
                PerformKnifeBuff(cardData);
                break;
            default:
                Debug.Log("Unknown ability: " + cardData.ability);
                break;

        }


    }

    private void PerformAttackBuff(CardData cardData)
    {
        Debug.Log("Attack ability activated!");
        int posX = cardData.posX;
        int posY = cardData.posY;

        int baseAttackPower = cardData.attackPower;
        int totalAttackPower = baseAttackPower;

        // Check the left card (posY - 1)
        if (posY - 1 >= 0)
        {
            GameObject leftCardSlot = board[posX, posY - 1];
            if (leftCardSlot != null)
            {
                DropZone leftDropZone = leftCardSlot.GetComponent<DropZone>();
                if (leftDropZone != null && leftDropZone.HasCard())
                {
                    CardData leftCard = leftDropZone.GetCard();
                    totalAttackPower += leftCard.attackPower;
                }
            }
        }

        // Check the right card (posY + 1)
        if (posY + 1 < columns)
        {
            GameObject rightCardSlot = board[posX, posY + 1];
            if (rightCardSlot != null)
            {
                DropZone rightDropZone = rightCardSlot.GetComponent<DropZone>();
                if (rightDropZone != null && rightDropZone.HasCard())
                {
                    CardData rightCard = rightDropZone.GetCard();
                    totalAttackPower += rightCard.attackPower;
                }
            }
        }

        // Set the new attack power (this will trigger the UI update automatically)
        cardData.attackPower = totalAttackPower;

        Debug.Log($"Character card at {posX}, {posY} now has an attack power of {totalAttackPower}.");
    }




    private void PerformKnifeBuff(CardData cardData)
    {
        int posX = cardData.posX;
        int posY = cardData.posY;

        GameObject frontCardSlot = board[posX + 1, posY];
        DropZone FrontZone = frontCardSlot.GetComponent<DropZone>();
        if (FrontZone != null && FrontZone.HasCard())
        {
            CardData frontCard = FrontZone.GetCard();
            frontCard.attackPower += 3;
        }

    }

    private void PerformDamage()
    {
        Debug.Log("Damage ability activated!");
        // Add your damage logic here
    }




    public void HandleAttack()
    {
        // Step 1: Handle player attacks from last row (x = 2)
        for (int y = 0; y < columns; y++)
        {
            CardData attackerCard = GetCardAtPosition(2, y);
            if (attackerCard != null && attackerCard.cardHealth > 0) // Ensure the attacker card is alive
            {
                // Step 2: Check the corresponding enemy card in row x = 0
                CardData defenderCard = GetCardAtPosition(0, y);
                if (defenderCard != null && defenderCard.cardHealth > 0)
                {
                    performAttack(attackerCard, defenderCard);
                }
                else
                {
                    // Step 3: If no valid defender, search left and right for another target
                    defenderCard = FindNextDefender(0, y);
                    if (defenderCard != null)
                    {
                        performAttack(attackerCard, defenderCard);
                    }
                }
            }
        }

        // Step 4: Handle enemy attacks from first row (x = 0)
        for (int y = 0; y < columns; y++)
        {
            CardData attackerCard = GetCardAtPosition(0, y);
            if (attackerCard != null && attackerCard.cardHealth > 0) // Ensure the attacker card is alive
            {
                // Step 5: Check the corresponding player card in row x = 2
                CardData defenderCard = GetCardAtPosition(2, y);
                if (defenderCard != null && defenderCard.cardHealth > 0)
                {
                    performAttack(attackerCard, defenderCard);
                }
                else
                {
                    // Step 6: If no valid defender, search left and right for another target
                    defenderCard = FindNextDefender(2, y);
                    if (defenderCard != null)
                    {
                        performAttack(attackerCard, defenderCard);
                    }
                }
            }
        }
        CheckGameOver();

    }
    private bool AreAllCharactersDead(int row)
    {
        for (int y = 0; y < columns; y++)
        {
            CardData card = GetCardAtPosition(row, y);
            if (card != null && card.cardHealth > 0)
            {
                return false; // Found a character still alive
            }
        }
        return true; // All characters are dead
    }



    private void CheckGameOver()
    {
        if (AreAllCharactersDead(0)) // Check if all enemies are dead
        {
            DisplayGameOverMessage("All enemies have been defeated! You win!");
            SetHealthForLosingColumn(2); // Set player health to 0
        }
        else if (AreAllCharactersDead(2)) // Check if all players are dead
        {
            DisplayGameOverMessage("All your characters have been defeated! Game over!");
            SetHealthForLosingColumn(0); // Set enemy health to 0
        }
    }

    // New method to set health for losing column
    private void SetHealthForLosingColumn(int column)
    {
        for (int row = 0; row < rows; row++)
        {
            CardData cardData = GetCardAtPosition(row, column);
            if (cardData != null)
            {
                cardData.cardHealth = 0; // Set health to 0
            }
        }
    }



    // Method to display the game over message
    private void DisplayGameOverMessage(string message)
    {
        // Implement your UI display logic here, e.g., using a UI Text element or a popup
        Debug.Log(message); // Example output to console
    }

    // Helper method to get card data at a specific position
    private CardData GetCardAtPosition(int x, int y)
    {
        if (x >= 0 && x < rows && y >= 0 && y < columns)
        {
            DropZone dropZone = board[x, y]?.GetComponent<DropZone>();
            return dropZone?.GetCard();
        }
        return null;
    }

    // Helper method to find the next defender with health > 0
    private CardData FindNextDefender(int row, int startY)
    {
        // Check to the right
        for (int offset = 0; offset < columns; offset++)
        {
            int rightY = startY + offset;
            if (rightY < columns)
            {
                CardData card = GetCardAtPosition(row, rightY);
                if (card != null && card.cardHealth > 0)
                {
                    return card;
                }
            }

            // Check to the left
            int leftY = startY - offset;
            if (leftY >= 0)
            {
                CardData card = GetCardAtPosition(row, leftY);
                if (card != null && card.cardHealth > 0)
                {
                    return card;
                }
            }
        }

        return null; // No valid defender found
    }




    private void performAttack(CardData attackerCard, CardData defenderCard)
    {
        // Perform the attack
        defenderCard.cardHealth -= attackerCard.attackPower;

        // Check if the defender card is still alive after the attack
        if (defenderCard.cardHealth <= 0)
        {
            defenderCard.cardHealth = 0; // Ensure health doesn't go below zero
            Debug.Log($"{defenderCard.cardName} has been defeated!");
        }
        else
        {
            Debug.Log($"{defenderCard.cardName} takes {attackerCard.attackPower} damage!");
        }
    }



}