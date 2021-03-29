using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FonChange : MonoBehaviour
{
    public Image im;
    public Sprite[] b;
    private int i;
    
    void Awake()
    {
        LeanTween.moveY(gameObject, Screen.height * 1.5f, 0.05f);
        S.LoadData();
        i = S.fon;
        if (i < b.Length)
        {
            im.sprite = b[i];
        }
        gameObject.SetActive(false);
    }
     
    public void TurnOn()
    {
        LeanTween.moveY(gameObject, Screen.height * 0.5f, 0.5f);
    }


    public void Select(int i)
    {
        if (i < b.Length)
        {
            S.fon = i;
            S.SaveData();
            im.sprite = b[i];
        }
        StartCoroutine("TurnOff");
    }
    IEnumerator TurnOff()
    {
        yield return null;
        LeanTween.moveY(gameObject, Screen.height * 1.5f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
