using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingPropellorScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _propellor;

    [SerializeField]
    private float _rotationSpeed = 50.0f;
	void Update ()
    {
        _propellor.transform.Rotate(0, 0, _rotationSpeed*Time.deltaTime);
	}
}
