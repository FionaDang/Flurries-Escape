using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlurryFire : MonoBehaviour
{
    public Rigidbody sparkle;
    public Transform FireTransform;
    private string fire_Button;
    private bool shoot = true;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("space") && shoot)
        {
            StartCoroutine(Fire());
        }
    }
    IEnumerator Fire()
    {
        shoot = false;
        Rigidbody sparkleInstance = Instantiate(sparkle, FireTransform.position, FireTransform.rotation) as Rigidbody;
        sparkleInstance.AddForce(-FireTransform.right * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(0.5f);
        shoot = true;
    }
}
