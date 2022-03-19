
using UnityEngine;
using UnityEngine.UI;

public class CrosshairScript : MonoBehaviour
{
    public Image crosshair;
    void Start()
    {
        crosshair.transform.position = new Vector2(Screen.width / 2, Screen.height / 2);
    }
}
