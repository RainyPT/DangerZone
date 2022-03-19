using UnityEngine;
using UnityEngine.UI;


public class GunScript : MonoBehaviour
{
    public float firerate = 0.5f;
    private float fireDelay = 0f;
    public float range = 100f;
    public Camera cam;
    public GameObject bulletImpact;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && fireDelay <= 0f)
        {
            shoot();
            fireDelay = firerate;
        }
        fireDelay -= Time.fixedDeltaTime;
    }
    private void shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
            Instantiate(bulletImpact, hit.point +(hit.normal*0.002f),Quaternion.LookRotation(hit.normal,Vector3.up));
        }
    }
}
