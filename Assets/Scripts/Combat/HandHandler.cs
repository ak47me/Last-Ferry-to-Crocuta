using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHandler : MonoBehaviour
{
    // Credit to Sinuous for code found in this video: https://www.youtube.com/watch?v=PybdYrUKXfo

    //private static HandHandler _instance;
    //public static HandHandler Instance { get { return _instance; } }

    public GameObject cardPrefab;
    public float fanSpread;
    [Range (0f, 1000f)] public float xOffset;
    public float yOffset;
    public Transform handTransform;
    public List<GameObject> hand = new List<GameObject>();
    public List<CardInfo> cardData;
    public static HandHandler Instance { get; private set; }
    // Awaken the singleton
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize hand cards if available
        if (MainManager.Instance != null)
        {
            cardData = MainManager.Instance.handCards; // Get hand cards from MainManager
            AddStartingCards(); // Initialize the cards in hand
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // No need to add starting cards here
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddNewCard(CardInfo info)
    {
        // Instantiate Card
        GameObject card = generateCard(info, handTransform);
        card.transform.localScale = info.handScale;
        hand.Add(card);

        UpdateHandVisual();
    }

    public void UpdateHandVisual()
    {
        int numCards = hand.Count;

        if (numCards == 1)
        {
            hand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            hand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for (int i = 0; i < numCards; i++)
        {
            // The angle will go from positive to negative, with the middle card having 0 rotation.
            float rotAngle = Mathf.Lerp(fanSpread, -fanSpread, (float) i / (float) numCards);
            hand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotAngle);

            float offsetMax = numCards * xOffset;
            // Horizontal offset will go from negative to positive with the middle card being in the center.
            float woffset = Mathf.Lerp(-offsetMax, offsetMax, (float) i / (float)numCards);

            // Vertical offset is a curve between a number y0, a peak y1, and then back to y0.
            float voffset = -yOffset*Mathf.Abs(((float)i+1 - (float)(numCards + 1) / 2))/2;

            hand[i].transform.localPosition = new Vector3(woffset, voffset, 0f);

            CardMover mover = hand[i].GetComponent<CardMover>();
            mover.startPos = hand[i].transform.position;
        }
    }

    // FIXME: Needs to actually get the card data from saved data
    public void AddStartingCards()
    {
        foreach (CardInfo info in cardData)
        {
            AddNewCard(info);
        }
    }

    public void removeCard(GameObject gameObj)
    {
        hand.Remove(gameObj);
        UpdateHandVisual();
    }

    public void AddExistingCard(GameObject gameObj)
    {
        hand.Add(gameObj);
        UpdateHandVisual();
    }

    public GameObject generateCard(CardInfo info, Transform parentTransform)
    {
        GameObject card = Instantiate(cardPrefab, parentTransform.position, Quaternion.identity, parentTransform);
        CardView cardView = card.GetComponent<CardView>();
        cardView.setUpCard(info);

        return card;
    }
}
