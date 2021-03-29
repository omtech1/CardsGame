using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoadingBar : MonoBehaviour
{
    
    private Slider bar;
    private bool loaded = false;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine("LoadBar");
    } 
    IEnumerator LoadBar()
    {

        bar = GetComponent<Slider>();
        for (int i = 0; i < 300; i++)
        {
            yield return new WaitForSeconds(0.01f);
            bar.value = i;
            if (i==270)
                StartCoroutine("Async");
        }
        loaded = true;
    }
    IEnumerator Async()
    {
        AsyncOperation asyncLoad =  SceneManager.LoadSceneAsync(1);
        while (!loaded) { 
        yield return new WaitForSeconds(0.1f);
        }
    }
}
