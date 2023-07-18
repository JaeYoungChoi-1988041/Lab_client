using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotObject : MonoBehaviour
{
    public ParticleSystem attackp;
    Transform sopos;

    Archer ar;
    Mage ma;

    WaitForSeconds WFS05 = new WaitForSeconds(0.5f);
   
    private void Start()
    {
        sopos = this.transform;
    }

    void Update()
    {
        Destroyobj();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Shotobj());
        }
    }
    IEnumerator Shotobj()
    {
        Destroy(gameObject);
        yield return WFS05;
    }

    void Destroyobj()
    {
        Destroy(gameObject, 4);
    }
}
