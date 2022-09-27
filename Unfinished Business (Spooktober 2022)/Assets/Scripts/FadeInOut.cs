using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    [SerializeField]
    GameObject fader;

    int defaultFadeSpeed = 5;

    //public void FadeScene()
    //{
    //    StartCoroutine(FadeIO(defaultFadeSpeed));
    //}
    
    public void FadeScene(bool b)
    {
        StartCoroutine(Fade(b, defaultFadeSpeed));
    }

    /// <summary>
    /// Call the enumerator to fade an object in or out
    /// </summary>
    /// <param name="b"></param>
    /// <param name="element"></param>
    public void FadeElement(bool b, GameObject element)
    {
        StartCoroutine(FadeE(b, element));
    }

    /// <summary>
    /// Fades a black screen in or out
    /// </summary>
    /// <param name="fadeToBlack"></param>
    /// <param name="fadeSpeed"></param>
    /// <returns></returns>
    public IEnumerator Fade(bool fadeToBlack, int fadeSpeed)
    {
        Color color = fader.GetComponent<SpriteRenderer>().color;
        float fadeAmount;

        // Fade out
        if (fadeToBlack)
        {
            while (fader.GetComponent<SpriteRenderer>().color.a < 1)
            {
                fadeAmount = color.a + (fadeSpeed * Time.deltaTime);

                color = new Color(color.r, color.g, color.b, fadeAmount);
                fader.GetComponent<SpriteRenderer>().color = color;
                yield return null;
            }

            // When done
            FindObjectOfType<DialogueManager>().NewDialogue(true);
            StopCoroutine(Fade(fadeToBlack, fadeSpeed));
        }

        // Fade in
        else
        {
            while (fader.GetComponent<SpriteRenderer>().color.a > 0)
            {
                fadeAmount = color.a - (fadeSpeed * Time.deltaTime);

                color = new Color(color.r, color.g, color.b, fadeAmount);
                fader.GetComponent<SpriteRenderer>().color = color;
                yield return null;
            }

            // When done
            StopCoroutine(Fade(fadeToBlack, fadeSpeed));
        }
    }

    /// <summary>
    /// Fades an element in or out
    /// </summary>
    /// <param name="fadeToBlack"></param>
    /// <param name="fadeSpeed"></param>
    /// <returns></returns>
    public IEnumerator FadeE(bool fadeToBlack, GameObject element, int fadeSpeed = 5)
    {
        Color color = element.GetComponent<SpriteRenderer>().color;
        float fadeAmount;

        // Fade out
        if (fadeToBlack)
        {
            while (element.GetComponent<SpriteRenderer>().color.a < 1)
            {
                fadeAmount = color.a + (fadeSpeed * Time.deltaTime);

                color = new Color(color.r, color.g, color.b, fadeAmount);
                element.GetComponent<SpriteRenderer>().color = color;
                yield return null;
            }

            // When done
            StopCoroutine(FadeE(fadeToBlack, element, fadeSpeed));
        }

        // Fade in
        else
        {
            while (element.GetComponent<SpriteRenderer>().color.a > 0)
            {
                fadeAmount = color.a - (fadeSpeed * Time.deltaTime);

                color = new Color(color.r, color.g, color.b, fadeAmount);
                element.GetComponent<SpriteRenderer>().color = color;
                yield return null;
            }

            // When done
            StopCoroutine(FadeE(fadeToBlack, element, fadeSpeed));
        }
    }

    public bool IsFaded()
    {
        if (fader.GetComponent<SpriteRenderer>().color.a == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
