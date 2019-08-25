using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSlideScript : MonoBehaviour
{

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _slideTime = 3.0f;

    private GameObject _door;

	void Start ()
    {
        _door = this.gameObject;
	}

    public void OpenDoor()
    {
        _slideTime -= Time.deltaTime;

        _door.transform.Translate(_speed, 0, 0, Space.Self);

        if (_slideTime < 0)
        {
            _speed = 0.0f;
        }
    }
}
