using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public float damage;
    public bool causeDamage;
    public Vector3 velocity;
    public float lifetime = 5f;
    public Entity launcher;
    public EntityType launcherType;

    public void Init(float damage, Vector3 velocity, Entity launcher)
    {
        this.damage = damage;
        this.velocity = velocity;
        transform.LookAt(transform.position + velocity);
        this.launcher = launcher;
        launcherType = launcher.entityType;
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

        if (launcherType == EntityType.Player && entity.entityType != EntityType.Player ||
            launcherType == EntityType.Monster && entity.entityType == EntityType.Player)
        {
            entity.TakeDamage(damage, velocity.normalized * 0.5f);
            causeDamage = true;
            Destroy(gameObject);
        }
    }
}