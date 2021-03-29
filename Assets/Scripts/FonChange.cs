using UnityEngine;
using UnityEngine.UI;
public class FonChange : MonoBehaviour
{
    public Image im;
    public Sprite[] b;
    private int i;
    
    void Awake()
    {
        S.LoadData();
        i = S.fon;
        if (i < b.Length)
        {
            im.sprite = b[i];
        }
        gameObject.SetActive(false);
    }

    public void Select(int i)
    {
        if (i < b.Length)
        {
            S.fon = i;
            S.SaveData();
            im.sprite = b[i];
        }
        gameObject.SetActive(false);
    }
}
