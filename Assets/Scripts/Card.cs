using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int color;
    public bool unable = false;

    private Button button;

    private void Start()
    { 
        button = GetComponent<Button>();
    }

    public void Go()
    {
        unable = true;
        button.enabled = false;
        transform.parent.GetComponent<Hand>().ReSort();
        S.score++;
        LeanTween.move(gameObject, new Vector3(Screen.width / 2, Screen.height / 2), 0.5f).setEaseOutCirc();
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setDelay(1.8f).setEaseOutCirc();
        StartCoroutine(Disabl());
    }
    IEnumerator Disabl()
    {
        yield return new WaitForSeconds(2f);
        gameObject.SetActive(false);
    }

}
