using UnityEngine;
using System.Collections;

public class Card_Manager : MonoBehaviour
{
    public Deck_Manager manager;

    private IEnumerator moveToPosition(Vector2 endPos, float duration, float offset)
    {

        gameObject.transform.SetParent(manager.deck2_Position);


        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 startPos = rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            gameObject.transform.SetAsLastSibling();
            yield return null;
        }

        rectTransform.anchoredPosition = endPos;
        manager.deck2.Add(gameObject);
        manager.topCard_deck2 = manager.deck2[0];


    }

    public void moveCard(Vector2 endPos, float duration, float offset)
    {
        StartCoroutine(moveToPosition(endPos, duration, offset));
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
