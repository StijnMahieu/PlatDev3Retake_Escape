using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalDoorScript : MonoBehaviour
{
    public bool IsOpened = false;

    private GameObject _door;

    [SerializeField]
    private GameObject _verticalUp;
    [SerializeField]
    private GameObject _verticalDown;

    [SerializeField]
    private float _speed = 0.1f;

    void Start()
    {
        _door = this.gameObject;
    }

    public void DoorDown()
    {
        _door.transform.position = Vector3.MoveTowards(_door.transform.position, _verticalDown.transform.position, _speed);
    }

    public void DoorUp()
    {
        _door.transform.position = Vector3.MoveTowards(_door.transform.position, _verticalUp.transform.position, _speed);
    }
}
