using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeEffect : MonoBehaviour
{
    public Image[] currentImage;
    public Text[] currentText;
    private Color c;

    private void OnEnable()
    {
        StartCoroutine(fadeEffect(0.05f));
    }

    IEnumerator fadeEffect(float fadingRate)
    {
        //for (float f = 0; f < 1; f += fadingRate)
        for (float f = 1; f >= -0.05f; f -= fadingRate)
        {
            for (int i=0;i<currentImage.Length;i++)
            {
                c = currentImage[0].color;
                c.a = f;
                currentImage[i].color = c;
            }

            for(int i = 0; i < currentText.Length; i++)
            {
                c = currentText[0].color;
                c.a = f;
                currentText[i].color = c;
            }
            yield return new WaitForSeconds(0.05f);
        }
        gameObject.SetActive(false);
    }
}
