using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorBoxHandler : MonoBehaviour
{

    public TMP_Text errorText;

    public void Show(string error)
    {
        this.errorText.text = error;
        this.gameObject.SetActive(true);
    }
    public void Close()
    {
        this.gameObject.SetActive(false);
    }
}
