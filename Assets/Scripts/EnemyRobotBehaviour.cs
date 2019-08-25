using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyRobotBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _wayPoints;

    private GameObject _player;

    [SerializeField]
    private GameObject _robotHead;
    [SerializeField]
    private GameObject _robotRayOrigin;

    public INode BehaviourTree;
    public Coroutine TreeCoroutine;

    private EnemyExplosionScript[] _explodeEnemyScript;

    [SerializeField]
    private CharacterBehaviourScript _characterBehaviourScript;

    public int Health = 2;

    private NavMeshAgent _agent;

    public bool DestinationIsReached;

    private int _destinationInt = 0;

    private Vector3 _headRotation = new Vector3(0,180,0);
    private Vector3 _currentHeadRotation;

    private bool _allowRotation;

    private bool _playerInRange;

    private float _robotRange = 200;

    private float _timer = 0;

    [SerializeField]
    private ParticleSystem[] _muzzleFlashes;

    private UnityEngine.Random _randomInt;

	void Start ()
    {
        _explodeEnemyScript = gameObject.GetComponentsInChildren<EnemyExplosionScript>();

        _agent = gameObject.GetComponent<NavMeshAgent>();

        _player = GameObject.FindGameObjectWithTag("MainCharacter");

        BehaviourTree = new SelectorNode
        (
            new SequenceNode
            (
                new SequenceNode
                (
                    new ConditionNode(IsPlayerInRange),
                    new ActionNode(TurnTowardsPlayer),
                    new ActionNode(ShootAtPlayer)
                )
            ),
            new SequenceNode
            (
                new SequenceNode
                (
                    new ConditionNode(HasReachedDestination),
                    new ActionNode(ChooseNewDestination)
                ),
                new SequenceNode
                (
                    new ActionNode(MoveToDestination)
                )
            )
        );

        TreeCoroutine = StartCoroutine(RunTree());

        _currentHeadRotation = _robotHead.transform.rotation.eulerAngles;
    }
	
	void Update ()
    {
        ExplodeEnemy();

        //don't allow tire rotation
        _agent.updateRotation = false;

        if (_allowRotation)
        {
            RotateRobotHead();
        }
    }

    public IEnumerator RunTree()
    {
        while(Health > 0)
        {
            yield return BehaviourTree.Tick();
        }
    }
    bool HasReachedDestination()
    {
        if(_agent.pathPending)
        {
            return false;
        }
        if(_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _allowRotation = true;

            if (!_agent.hasPath || _agent.velocity.sqrMagnitude <= 0.0f)
            {
                return true;
            }
        }
        return false;
    }

    bool IsPlayerInRange()
    {
        if(_playerInRange)
        {
            return true;
        }
        else
        {
            _agent.speed = 7;
            return false;
        }
    }

    IEnumerator<NodeResult> ChooseNewDestination()
    {
        _agent.SetDestination(_wayPoints[_destinationInt].transform.position);
        _destinationInt += 1;
        _destinationInt %= 2;

        yield return NodeResult.Succes;
    }

    IEnumerator<NodeResult> MoveToDestination()
    {
        _agent.SetDestination(_wayPoints[_destinationInt].transform.position);
        _currentHeadRotation = _robotHead.transform.rotation.eulerAngles;
        yield return NodeResult.Succes;
    }

    IEnumerator<NodeResult> TurnTowardsPlayer()
    {
        if(_playerInRange)
        {
            _agent.speed = 0;
            _robotHead.transform.LookAt(_player.transform);
        }
        yield return NodeResult.Succes;
    }

    IEnumerator<NodeResult> ShootAtPlayer()
    {
        _timer += Time.deltaTime;
        if (_timer >= 2)
        {
            ShootInPlayerDirection();
            _timer = 0;
        }
        yield return NodeResult.Succes;
    }

    private void ExplodeEnemy()
    {
        if(Health <= 0)
        {
            _explodeEnemyScript[0].Explode();
            _explodeEnemyScript[1].Explode();
        }
    }

    private void RotateRobotHead()
    {
        _robotHead.transform.rotation = Quaternion.Lerp(Quaternion.Euler(_currentHeadRotation), Quaternion.Euler(_currentHeadRotation + _headRotation), 180 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "MainCharacter")
        {
            _playerInRange = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "MainCharacter")
        {
            _playerInRange = false;
        }
    }

    void ShootInPlayerDirection()
    {
        RaycastHit hit;

        if (Physics.Raycast(_robotRayOrigin.transform.position, _robotRayOrigin.transform.forward, out hit, _robotRange))
        {
            if (hit.transform.name.Contains("Player"))
            {
                _muzzleFlashes[UnityEngine.Random.Range(0, 2)].Play();

                _characterBehaviourScript.PlayerHealth -= 1;
                Debug.Log("PlayerHit");
            }
        }
    }
}
