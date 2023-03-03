using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public float spawnItvalTime = 2;
    public float spawnStartDelay = 1;
    public float spawnItvalTimer = 0;
    public Transform spawnPrefab;

    public int spawnCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        var pos = transform.position;
        pos.y = 0f;
        transform.position = pos;
        spawnItvalTimer += spawnStartDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.gameState != GameState.Playing)
            return;

        spawnItvalTimer -= Time.deltaTime;
        if (spawnItvalTimer <= 0)
        {
            spawnItvalTimer += spawnItvalTime;
            Spawn();
        }
    }

    public void Spawn()
    {
        Transform trans = Instantiate(spawnPrefab);
        trans.position = transform.position;

        //Entity entity = trans.GetComponent<Entity>();
        //entity.hp += (int)(spawnCount * 0.075f);
        //entity.moveSpeed += spawnCount * 0.05f;

        spawnCount++;
    }
}
