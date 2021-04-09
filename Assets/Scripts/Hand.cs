using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class Hand : MonoBehaviour
{
    public GameObject cardPrefab, backPrefab;
    public Sprite[] colors = new Sprite[4];

    public Vector2 v2card;
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
        sizex = GetComponent<RectTransform>().rect.width + v2card.x;
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
        Deal—ards();
    }

    private void Deal—ards()
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
            }
        } 
    }
    private void BetweenCalc()
    {
        NativeArray<int> ncardColorArray = new NativeArray<int>(13, Allocator.Persistent);
        NativeArray<bool> ncardUnableArray = new NativeArray<bool>(13, Allocator.Persistent);
        NativeArray<float> results = new NativeArray<float>(3, Allocator.TempJob);

        for (int i = 0; i < cards.Count; i++)
        {
            ncardUnableArray[i] = cards[i].script.unable;
            ncardColorArray[i] = cards[i].script.color;
        }

        BetweenCalcJob betweenCalcJob = new BetweenCalcJob
        {
            ncardColor = ncardColorArray,
            ncardUnable = ncardUnableArray,
            v2cardX = v2card.x,
            color = color, 
            res = results, 
        };

        JobHandle jobHandle = betweenCalcJob.Schedule( );
        jobHandle.Complete();

        betweenSmall = results[0];
        betweenBig   = results[1];
        sdvig        = results[2];
        S.win = ncardUnableArray[0];

        ncardColorArray.Dispose();
        ncardUnableArray.Dispose();
        results.Dispose();
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

[BurstCompile]
 public struct BetweenCalcJob : IJob
{

    public NativeArray<int> ncardColor;
    public NativeArray<bool> ncardUnable;
    public NativeArray<float> res;
    public int color;
    public float v2cardX;
    public bool win;
    private int unableCount, ableCount;
    private float nbetweenSmall, nbetweenBig, nsdvig;

    public void Execute()
    {
        win = false;
        for (int index = 0; index < ncardColor.Length; index++)
        {

            if (!ncardUnable[index])
            {
                if (ncardColor[index] != color) { unableCount++; }
                else { ableCount++; }
            }
        }

        if (ableCount == 0) { win = true; }

        nbetweenSmall = v2cardX * 0.3f;
        nbetweenBig = (v2cardX - unableCount * v2cardX * 0.3f) / ableCount;
        if (nbetweenBig > v2cardX * 0.3f * 2.5f) { nbetweenBig = v2cardX * 0.3f * 2.5f; nbetweenSmall = (v2cardX - ableCount * nbetweenBig) / unableCount; }
        if (nbetweenBig < v2cardX * 0.3f) { nbetweenBig = v2cardX * 0.3f * 1.5f; }

        if (nbetweenSmall > v2cardX) { nbetweenSmall = v2cardX; }

        nsdvig = ((nbetweenBig * ableCount + nbetweenSmall * unableCount) * 0.5f);
        nsdvig -= v2cardX * 0.3f;

        res[0] = nbetweenSmall;
        res[1] = nbetweenBig;
        res[2] = nsdvig;
        ncardUnable[0] = win;
    }

}