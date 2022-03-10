using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingTextAnimation : MonoBehaviour
{
    public TMP_Text loadingText;
    float delay = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        loadingText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delay < 0)
        {
            if (loadingText.text.Contains("..."))
            {
                loadingText.text = loadingText.text.Split('.')[0];
            }
            else
            {
                loadingText.text += ".";
            }
            delay = 0.5f;
        }
        delay -= Time.deltaTime;
    }
}
