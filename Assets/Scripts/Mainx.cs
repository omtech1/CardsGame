using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class Mainx : MonoBehaviour
{
    public RectTransform header, fon_select_Button, pause_Button;
    public GameObject pausePanel, stopPanel, hand;
    public TMP_Text score, scoreResult;
     void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        
        GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.height, 0);
        header.sizeDelta            = new Vector2(Screen.height , Screen.height * 0.25f); 
        fon_select_Button.sizeDelta = new Vector2(Screen.height * 0.056f, Screen.height * 0.056f);
        pause_Button.sizeDelta      = new Vector2(Screen.height * 0.056f, Screen.height * 0.056f);
        float chelka = Screen.height - Screen.safeArea.height;
        if (chelka > 0)
        {
            header.sizeDelta += new Vector2(0, chelka );
            score.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0, chelka * 0.3f);
        } 
        header.anchoredPosition = new Vector2(0, (0 - header.rect.height*0.5f)) ;

        S.pause = false;
        S.score = 0;
        pausePanel.SetActive(S.pause);
        stopPanel.SetActive(S.pause);
        if (Screen.height / Screen.dpi < 5.7f)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            Screen.orientation = ScreenOrientation.Landscape;
        }
    }


    void Update()
    {
        score.text = S.score.ToString();
        if(S.win)
        {
            S.win = false;
            StartCoroutine("DelayWin");
        }
    }
 
    public void Pause()   
    {
        S.pause = !S.pause;
        pausePanel.SetActive(S.pause);
        if (S.pause) Time.timeScale = 0; else Time.timeScale = 1f;
    }
    public void Restart()
    {
        S.pause = false;
        S.win = false;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator DelayWin()
    {
        yield return new WaitForSeconds(2f);
        stopPanel.SetActive(true);
        scoreResult.text = "Your score " + S.score.ToString();
    }
}
