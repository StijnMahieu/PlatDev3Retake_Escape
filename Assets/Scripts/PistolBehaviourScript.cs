using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBehaviourScript : MonoBehaviour
{
    private GunPickupScript _gunPickupScript;
    private CharacterBehaviourScript _characterBehaviourScript;

    private EnemyRobotBehaviour _robot0Behaviour;
    private EnemyRobotBehaviour _robot1Behaviour;

    //control enemy robot behaviours
    [SerializeField]
    private GameObject[] _enemyRobots;

    //playeranimator
    private Animator _animator;

    private CharacterController _charController;

    //swtich between cams
    [SerializeField]
    private GameObject _playerCamera;
    [SerializeField]
    private GameObject _aDSCamera;

    //bullet range
    [SerializeField]
    private float _range = 10.0f;

    private bool _triggerPistol = false;

    [SerializeField]
    private ParticleSystem _muzzleFlash;

    [SerializeField]
    private GameObject _raycastOrigin;

    //avoid spamming gun
    private bool _gunCooldown = false;

	void Start ()
    {
        _robot0Behaviour = _enemyRobots[0].GetComponent<EnemyRobotBehaviour>();
        _robot1Behaviour = _enemyRobots[1].GetComponent<EnemyRobotBehaviour>();

        _gunPickupScript = this.gameObject.GetComponent<GunPickupScript>();
        _animator = this.gameObject.GetComponent<Animator>();
        _characterBehaviourScript = this.gameObject.GetComponent<CharacterBehaviourScript>();

        _charController = this.gameObject.GetComponent<CharacterController>();

        _aDSCamera.SetActive(false);
	}
	
	void Update ()
    {
        if (_charController.isGrounded)
        {
            ADS();
            if (Input.GetAxisRaw("ShootPistol") > 0.9f && !_triggerPistol && _aDSCamera.activeSelf && _gunCooldown == false)
            {
                _muzzleFlash.Play();
                ShootPistol();
                ShootPistolAnimation();
            }
        }

        ReleaseTrigger();
	}

    private void ADS()
    {
        if(_gunPickupScript.IsHoldingPistol)
        {
            if(Input.GetAxisRaw("ADS") > 0.0f)
            {
                _animator.SetBool("IsAiming",true);

                _playerCamera.SetActive(false);
                _aDSCamera.SetActive(true);

                _characterBehaviourScript.AllowMovement = false;
            }
            else
            {
                _animator.SetBool("IsAiming", false);

                _playerCamera.SetActive(true);
                _aDSCamera.SetActive(false);

                _characterBehaviourScript.AllowMovement = true;
            }
        }
    }

    private void ShootPistolAnimation()
    {
        if(Input.GetAxisRaw("ADS")>0.0f)
        {
            _animator.SetTrigger("ShootPistol");
        }
    }

    private void ShootPistol()
    {
        RaycastHit hit;
        _triggerPistol = true;

        if(Physics.Raycast(_raycastOrigin.transform.position, _aDSCamera.transform.forward, out hit, _range))
        {
            Debug.Log(hit.transform.name);

            //Targets 
            HitTargetScript _hitTargetScript =  hit.transform.GetComponent<HitTargetScript>();

            if(_hitTargetScript != null)
            {
                _hitTargetScript.TargetHit();
            }

            if (hit.transform.name.Contains("ShootingTarget"))
            {
                _hitTargetScript.AmountOfTargetsHit();
            }

            //Shooting robots
            if (hit.transform.name.Contains("EnemyRobot0"))
            {
                _robot0Behaviour.Health -= 1;
            }
            if (hit.transform.name.Contains("EnemyRobot1"))
            {
                _robot1Behaviour.Health -= 1;
            }
        }
    }

    private void ReleaseTrigger()
    {
        if (Input.GetAxisRaw("ShootPistol") == 0)
        {
            _triggerPistol = false;
        }
    }

    public void StartGunCooldown()
    {
        _gunCooldown = true;
    }

    public void EndGunCooldown()
    {
        _gunCooldown = false;
    }
}
