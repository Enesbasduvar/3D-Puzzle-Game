using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpeakToPlayer : MonoBehaviour
{
    public TextMeshProUGUI[] texts;
    public float transitionDuration = 1.0f;
    public float textDuration = 5.0f;
    private int currentIndex = 0;
    private Coroutine transitionCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        StartTransition();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void StartTransition()
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(Transition());
    }

    IEnumerator Transition()
    {
        // Fade out the current text
        float elapsedTime = 0;
        while (texts[currentIndex].color.a > 0)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            Color c = texts[currentIndex].color;
            c.a = Mathf.Lerp(1, 0, t);
            texts[currentIndex].color = c;

            yield return null;
        }
        
        // Switch to the next text
        currentIndex = (currentIndex + 1) % texts.Length;
        //if (currentIndex == texts.Length - 1)
            //SceneManager.LoadScene("MainMenu");

        // Fade in the new text
        elapsedTime = 0;
        while (texts[currentIndex].color.a < 1)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / transitionDuration);

            Color c = texts[currentIndex].color;
            c.a = Mathf.Lerp(0, 1, t);
            texts[currentIndex].color = c;

            yield return null;
        }
        yield return new WaitForSeconds(textDuration);
        // Start the next transition
        StartTransition();
    }
}
