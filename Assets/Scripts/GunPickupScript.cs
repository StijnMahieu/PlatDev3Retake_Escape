using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickupScript : MonoBehaviour
{
    private Animator _animator;

    [SerializeField]
    public GameObject Pistol;

    [SerializeField]
    private GameObject _rightHand;

    //check if character is facing table
    [SerializeField]
    private GameObject _rayCastOrigin;
    [SerializeField]
    private float _pickupRange = 50.0f;

    [SerializeField]
    private GameObject _door1;
    [SerializeField]
    private GameObject _door3;
    [SerializeField]
    private GameObject _door4;

    public bool IsHoldingPistol = false;
    public bool WasHoldingPistol = false;

    public bool RevertPistolRotation = false;

    private DoorSlideScript _doorSlideScript;
    private CharacterBehaviourScript _characterBehaviourScript;

    public int IKSpeed = 2;
	void Start ()
    {
        _animator = this.gameObject.GetComponent<Animator>();
        _doorSlideScript = _door1.GetComponent<DoorSlideScript>();

        _characterBehaviourScript = this.gameObject.GetComponent<CharacterBehaviourScript>();
	}

    void Update()
    {
        OpenDoor1();
        OpenDoubleDoor();
        RevertRotation();
    }

    private void OnTriggerStay(Collider _collision)
    {
        RaycastHit hit;

        if (Physics.Raycast(_rayCastOrigin.transform.position, _rayCastOrigin.transform.forward, out hit, _pickupRange))
        {
            if (hit.transform.name.Contains("TableTrigger"))
            {
                if (_collision.gameObject.tag == "Table" && Input.GetButtonDown("Pickup") && IsHoldingPistol == false)
                {
                    _animator.SetTrigger("PickupPistol");
                    WasHoldingPistol = false;
                }

                if (_collision.gameObject.tag == "PutBackTable" && Input.GetButtonDown("Pickup") && IsHoldingPistol == true)
                {
                    _animator.SetTrigger("PutDownPistol");
                    WasHoldingPistol = true;
                }
            }
        }
    }

    private void OpenDoor1()
    {
        if(IsHoldingPistol)
        {
            _doorSlideScript.OpenDoor();
        }
    }

    private void OpenDoubleDoor()
    {
        if(!IsHoldingPistol && WasHoldingPistol == true)
        {
            _door3.GetComponent<DoorSlideScript>().OpenDoor();
            _door4.GetComponent<DoorSlideScript>().OpenDoor();
        }
    }

    public void PickupTime()
    {
        Pistol.transform.SetParent(_rightHand.transform);
        Pistol.transform.position = _rightHand.transform.position;

        IKSpeed *= -1;
    }

    public void PutDownTime()
    {
        Pistol.transform.SetParent(null);
    }

    public void FreezeCharacter()
    {
        //avoid player being able to move during pickup
        _characterBehaviourScript.Acceleration = 0;

        //avoid multiple pickupanimations to be played
        if (!WasHoldingPistol)
        {
            IsHoldingPistol = true;
        }
        if(WasHoldingPistol)
        {
            IsHoldingPistol = false;
        }
    }

    public void UnfreezeCharacter()
    {
        if(WasHoldingPistol)
        {
            RevertPistolRotation = true;
        }
        _characterBehaviourScript.Acceleration = 50;
    }

    private void RevertRotation()
    {
        if(RevertPistolRotation)
        {
            Pistol.transform.eulerAngles = new Vector3(180,0,90);
        }
    }
}
