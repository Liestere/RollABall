using UnityEngine;
using System.Collections;

public class Pickup : PooledObject
{
    public override void OnObjectDespawn()
    {
    }

    public override void OnObjectSpawn()
    {
    }

    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }
}