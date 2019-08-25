using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionScript : MonoBehaviour
{

    [SerializeField]
    private float _dontDestroyDelay;
    [SerializeField]
    private float _minForce;
    [SerializeField]
    private float _maxForce;
    [SerializeField]
    private float _radius;

    public void Explode()
    {
        foreach(Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();

            if(rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(UnityEngine.Random.Range(_minForce, _maxForce), transform.position, _radius);
            }

            Destroy(t.gameObject, _dontDestroyDelay);
            Destroy(GetComponentInParent<BoxCollider>());
        }
    }
}
