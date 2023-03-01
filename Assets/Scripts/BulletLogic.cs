using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public float damage;
    public bool causeDamage;
    public Vector3 velocity;
    public float lifetime = 5f;

    public void Init(float damage, Vector3 velocity)
    {
        this.damage = damage;
        this.velocity = velocity;
        transform.LookAt(transform.position + velocity);
    }

    private void FixedUpdate()
    {
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
        lifetime -= Time.fixedDeltaTime;
        if (lifetime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (causeDamage)
            return;
        var entity = other.GetComponent<Entity>();
        if (entity == null)
            return;

        if (entity.entityType != EntityType.Player)
        {
            entity.TakeDamage(damage);
            causeDamage = true;
            Destroy(gameObject);
        }
    }
}