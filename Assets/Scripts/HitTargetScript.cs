using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTargetScript : MonoBehaviour
{
    private Animator _targetAnimator;

    private GameManagerScript _gameManagerScript;

    [SerializeField]
    private GameObject _gameManager;

    void Start ()
    {
        _targetAnimator = this.gameObject.GetComponent<Animator>();
        _gameManagerScript = _gameManager.GetComponent<GameManagerScript>();
	}

    public void TargetHit()
    {
        _targetAnimator.SetBool("IsHit", true);
    }

    public void AmountOfTargetsHit()
    {
        _gameManagerScript.AmountOfTargetsHit += 1;
    }
}
