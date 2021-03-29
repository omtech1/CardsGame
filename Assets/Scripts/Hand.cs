using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

using UnityEngine;

public class Hand : MonoBehaviour
{
    public GameObject cardPrefab, backPrefab;
    public Sprite[] colors = new Sprite[4];

    private Vector2 v2card;
    private int color, unableCount, ableCount;
    private float sizex, sdvig,  lastx, betweenSmall, betweenBig;
    private GameObject[] cardsInHand = new GameObject[13];
    private GameObject[] cardBacks = new GameObject[13];
    private List<ListedCard> cards = new List<ListedCard>();
    private List<ListedCard> koloda = new List<ListedCard>();

    void Start()
    {
        v2card = new Vector2(Screen.height * 0.146f,Screen.height * 0.2f);
        cardPrefab.GetComponent<RectTransform>().sizeDelta = v2card;
        backPrefab.GetComponent<RectTransform>().sizeDelta = v2card;
        betweenSmall = v2card.x * 0.3f;

        sizex = GetComponent<RectTransform>().rect.width * 0.5f; 
        StartCoroutine(CreateCards()); 
    }

    IEnumerator CreateCards()
    {
        yield return new WaitForSeconds(0.2f); 
        
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            cardsInHand[i] = Instantiate(cardPrefab, cardPrefab.transform.parent.transform);
              cardBacks[i] = Instantiate(backPrefab, cardPrefab.transform.parent.transform);
            cardsInHand[i].name = "card " + i.ToString();
            LeanTween.scaleX(cardsInHand[i], 0, 0.15f);
        } 
        Destroy(cardPrefab);
        Destroy(backPrefab);
        
        for (int i = 0; i < cardsInHand.Length; i++)
        {
            yield return null;
            int r = Random.Range(0, 4);
            koloda.Add(new ListedCard(
                r,
                cardsInHand[i],
                cardsInHand[i].GetComponent<Image>(),
                cardsInHand[i].GetComponent<Button>(),
                cardsInHand[i].GetComponent<Card>()
                ));
        }
        foreach (ListedCard card in koloda)
        {
            yield return null;
            card.script.color = Random.Range(0, 4);
            card.im.sprite = colors[card.script.color];
        }
        yield return null;
         
        for (int i = 0; i < colors.Length; i++)
        {
            foreach (ListedCard card in koloda)
            {
                if(card.script.color == i)
                {
                    yield return null;
                    card.gameObject.transform.SetParent(gameObject.transform);
                    cards.Add(new ListedCard(
                    card.script.color,
                    card.gameObject,
                    card.gameObject.GetComponent<Image>(),
                    card.gameObject.GetComponent<Button>(),
                    card.gameObject.GetComponent<Card>()
                    ));

                    //koloda.Remove(card);
                } 
            }
        }      
        yield return null;
        DealÑards();
    }

    private void DealÑards()
    {
        color = Random.Range(0, 4); 

        BetweenCalc();

        lastx = 0;
        int i = 0;
        foreach (ListedCard card in cards)
        {
            LeanTween.scaleX(card.gameObject, 0, 0.15f);

            LeanTween.moveLocal(card.gameObject, new Vector3(lastx - sdvig, 0), 1f)
                .setEaseOutCirc().setDelay(i * 0.3f);
            LeanTween.moveLocal(cardBacks[i], new Vector3(lastx - sdvig, 0), 1f)
                .setEaseOutCirc().setDelay(i * 0.3f);

            LeanTween.scaleX(card.gameObject, 1, 0.15f).setDelay(i * 0.3f + 0.3f);
            LeanTween.scaleX(cardBacks[i], 0, 0.15f).setDelay(i * 0.3f + 0.15f);
            if (card.script.color == color)
            {
                lastx += betweenBig;
                card.im.color = Color.white; 
            }
            else
            {
                lastx += betweenSmall;
                card.im.color = Color.gray;
            }
            i++;
                card.btn.enabled = false;
        }
        StartCoroutine("DelayBackDestroy");

    }
   
    public void ReSort()
    {
        color = Random.Range(0, 4);
        
        BetweenCalc();

        lastx = 0;
        int i = 0;
        foreach (ListedCard card in cards)
        {
            if (!card.script.unable)
            {
                LeanTween.moveLocalX(card.gameObject, (lastx - sdvig), 0.5f).setEaseInOutSine();
                if (card.script.color == color)
                {
                    lastx += betweenBig;
                    card.im.color = Color.white;
                    card.btn.enabled = true;
                }
                else
                {
                    lastx += betweenSmall;
                    card.im.color = Color.gray;
                    card.btn.enabled = false;
                }
                i++;
            }
        }
    }

    private void BetweenCalc()
    {

        unableCount = 0;
        ableCount = 0;
        foreach (ListedCard card in cards)
        {
            if (!card.script.unable)
            {
                if (card.script.color != color) { unableCount++; }
                else { ableCount++; }
            }
        }
        if (ableCount == 0) { S.win = true; }

        betweenBig = sizex - unableCount * betweenSmall;
        if (betweenBig > betweenSmall * 2.5f) { betweenBig = betweenSmall * 2.5f; }
        if (betweenBig < betweenSmall) { betweenBig = betweenSmall * 1.5f; }

        sdvig = (betweenBig * ableCount + betweenSmall * unableCount) * 0.5f;
    }
    IEnumerator DelayBackDestroy()
    {
        yield return new WaitForSeconds(3.9f);
        foreach (GameObject item in cardBacks)
        {
        Destroy(item);
        }
        foreach (ListedCard card in cards)
        {
            if (card.script.color == color)
            { 
                card.btn.enabled = true;
            }
        }
    }
}
