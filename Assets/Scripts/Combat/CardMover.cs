using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardMover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public enum moveState
    {
        Hover,
        Drag,
        Place,
        Return,
        Idle,
        Fight,
    }

    public CardView card;
    public moveState state;

    public Transform parentToReturnTo = null;
    private BoardPosition priorBoardPosition = null;  // Reference to the previous DropZone
    private Canvas canvas;  // Reference to Canvas

    // Information I need for Hover
    // Credit to Sasquatch B. Studios and their video on UI
    // https://www.youtube.com/watch?v=u3YdlUW1nx0
    [SerializeField] private float _verticalMoveAmount = 0.3f;
    [SerializeField] private float _moveTime = 0.3f;
    [Range(0f, 2f), SerializeField] private float _scaleAmount = 1.1f;
    public Vector3 startPos;
    public Vector3 startScale;
    public Vector3 endPos;
    public Vector3 endScale;
    public float elapsedTime = 0f;
    public bool locked = false;

    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        startScale = transform.localScale;
        card = gameObject.GetComponent<CardView>();
        state = moveState.Idle;
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        updateMoveState();
    }

    // It's a finite state machine
    private void updateMoveState()
    {
        switch (state)
        {
            case moveState.Hover:
                Hover();
                break;

            case moveState.Drag:

                break;

            case moveState.Place:

                break;

            case moveState.Return:
                Return();
                break;

            case moveState.Idle:
                Idle();
                break;

            case moveState.Fight:
                Fight();
                break;
        }
    }

    public void Hover()
    {

        if (elapsedTime + Time.deltaTime > _moveTime)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        Vector3 lerpPosition = Vector3.Lerp(transform.position, endPos, elapsedTime / _moveTime);
        Vector3 lerpScale = Vector3.Lerp(transform.localScale, endScale, elapsedTime / _moveTime);

        transform.position = lerpPosition;
        transform.localScale = lerpScale;
    }

    public void Idle()
    {
        if (elapsedTime + Time.deltaTime > _moveTime)
        {
            return;
        }

        elapsedTime += Time.deltaTime;

        Vector3 lerpPosition = Vector3.Lerp(transform.position, startPos, elapsedTime / _moveTime);
        Vector3 lerpScale = Vector3.Lerp(transform.localScale, startScale, elapsedTime / _moveTime);

        transform.position = lerpPosition;
        transform.localScale = lerpScale;
    }

    private void Awake()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (locked || Board.Instance.currentPhase != Board.combatPhase.Play)
        {
            return;
        }

        state = moveState.Drag;

        parentToReturnTo = this.transform.parent;

        // If the card was in a drop zone, inform that drop zone that the card is being removed
        priorBoardPosition = parentToReturnTo.GetComponent<BoardPosition>();
        if (priorBoardPosition != null)
        {
            priorBoardPosition.clearCard();
        }
        else if (parentToReturnTo.gameObject.name == "Hand")
        {
            HandHandler.Instance.removeCard(gameObject);
        }

        // Set parent to a higher level to avoid being clipped by other UI elements
        transform.SetParent(transform.parent.parent);

        // Alter the rotation and scale to look better
        transform.localRotation = Quaternion.identity;
        transform.localScale = card.cardInfo.boardScale;

        canvasGroup.blocksRaycasts = false;

        // Get the card data from the card being dragged
        /*
        CardView card = GetComponent<CardView>(); // Ensure it's on the same GameObject
        if (card != null)
        {
            cardData = cardDisplay.cardData; // Assign card data
            Debug.Log("Card Data Retrieved: " + cardData.cardName);
        }
        else
        {
            Debug.LogError("CardDisplay component not found on this GameObject.");
        }
        */
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (locked || Board.Instance.currentPhase != Board.combatPhase.Play)
        {
            return;
        }

        Vector3 newPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = new Vector3(newPosition.x, newPosition.y, -5);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (locked || Board.Instance.currentPhase != Board.combatPhase.Play)
        {
            return;
        }

        transform.SetParent(parentToReturnTo);

        state = moveState.Idle;

        BoardPosition bPosition = parentToReturnTo.gameObject.GetComponent<BoardPosition>();

        if (parentToReturnTo.gameObject.name == "Hand")
        {
            HandHandler.Instance.AddExistingCard(gameObject);
            startScale = card.cardInfo.handScale;
            transform.localScale = card.cardInfo.handScale;
        }
        else if (bPosition != null)
        {
            bPosition.setCard(card);
            transform.position = bPosition.transform.position;
        }

        // Prevent the card from being detected by places we can drop it into until we pick it up again.
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (state == moveState.Idle && !eventData.dragging)
        {
            if (Board.Instance.currentPhase == Board.combatPhase.Play)
            {
                elapsedTime = 0f;

                startPos = transform.position;

                if (transform.localScale == card.cardInfo.boardScale)
                {
                    endPos = startPos + new Vector3(0, _verticalMoveAmount / 5, 0);
                    endScale = startScale * 1.05f;
                }
                else
                {
                    endPos = startPos + new Vector3(0, _verticalMoveAmount, 0);
                    endScale = startScale * _scaleAmount;
                }
                state = moveState.Hover;

            }

            InfoDisplay.Instance.setInfo(card);
            InfoDisplay.Instance.enable();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (state == moveState.Hover)
        {
            elapsedTime = 0f;
            state = moveState.Idle;
        }
    }

    public void Fight()
    {
        if (elapsedTime + Time.deltaTime > _moveTime)
        {
            state = moveState.Idle;
            elapsedTime = 0;
            return;
        }

        elapsedTime += Time.deltaTime;

        Vector3 lerpPosition = Vector3.Lerp(transform.position, startPos, elapsedTime / _moveTime);

        transform.position = lerpPosition;
    }

    public void Return()
    {
        print("Stuck in return");
        return;
    }
}
