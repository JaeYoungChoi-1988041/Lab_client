using UnityEngine;

public class SeedProjectile : AttackTrigger
{
    public float destroyTime = 1;
    public float speed;

    private void Awake()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * speed;
        Destroy(this.gameObject, destroyTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this);
    }
}
