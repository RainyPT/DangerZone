using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactScript : MonoBehaviour
{
    private float timetokill = 3f;
    // Update is called once per frame
    void Update()
    {
        timetokill -= Time.fixedDeltaTime;
        if (timetokill <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
