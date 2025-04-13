using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Deck_Manager : MonoBehaviour
{
    [Header("Card Info")]
    public int deckSize = 144;
    public float offset = 0.1f;
    public float moveTime = 2.0f;
    public GameObject CardPrefab;
    public GameObject topCard_deck1;
    public GameObject topCard_deck2;

    [Header("Decks")]
    public List<GameObject> deck1 = new List<GameObject>();
    public List<GameObject> deck2 = new List<GameObject>();

    public RectTransform deck1_Position;
    public RectTransform deck2_Position;

    // Private Vars

    private GameObject objToSpawn;
    private float spawnOffset;
    private float moveOffset;
    private WaitForSeconds moveDelay = new WaitForSeconds(1);
    private Vector2 deck2_Vec2;


    void Start()
    {
        deck2_Vec2 = deck2_Position.anchoredPosition;

        SpawnCards();
        StartCoroutine(MoveCards());
    }

    void Update()
    {
        
    }

    void SpawnCards()
    {
        spawnOffset = offset;

        for (int i = 0; i < deckSize; i++)
        {
            objToSpawn = Instantiate(CardPrefab, deck1_Position);
            objToSpawn.GetComponent<RectTransform>().anchoredPosition += new Vector2(spawnOffset, 0);
            objToSpawn.GetComponent<Card_Manager>().manager = gameObject.GetComponent<Deck_Manager>();

            deck1.Add(objToSpawn);

            spawnOffset += offset;
        }

        deck1.Reverse();
        topCard_deck1 = deck1[0];

    }


    IEnumerator MoveCards()
    {
        moveOffset = offset;

        for (int i = 0; i < deckSize; i++)
        {
            yield return moveDelay;
            Vector2 targetPos = deck2_Vec2 + new Vector2(i * offset, 0);
            topCard_deck1.GetComponent<Card_Manager>().moveCard(targetPos, moveTime, moveOffset);

            moveOffset += offset;
            topCard_deck1 = deck1[1];
            deck1.RemoveAt(0);
            
        }

    }
}
