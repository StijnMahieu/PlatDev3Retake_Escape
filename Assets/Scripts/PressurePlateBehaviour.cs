using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject _pressurePlate;
    [SerializeField]
    private GameObject _verticalDoor;

    private Animator _animator;

    private VerticalDoorScript _verticalDoorScript;

    private bool _isOpened = false;
    void Start()
    {
        _animator = _pressurePlate.GetComponent<Animator>();
        _verticalDoorScript = _verticalDoor.GetComponent<VerticalDoorScript>();
    }

    void Update()
    {
        DoorGoesUp();
    }

    private void PressureDown()
    {
        _animator.SetBool("IsPressurePlateDown", true);
        _verticalDoorScript.DoorDown();
    }

    private void PressureUp()
    {
        _animator.SetBool("IsPressurePlateDown", false);
        _verticalDoorScript.DoorUp();
    }

    private void OnTriggerStay(Collider _collision)
    {
        if (_collision.tag == "PressurePlateTrigger")
        {
            _isOpened = false;
            PressureDown();
        }
    }
    private void OnTriggerExit(Collider _collision)
    {
        _isOpened = true;
    }

    private void DoorGoesUp()
    {
        if(_isOpened)
        {
            PressureUp();
        }
    }
}

