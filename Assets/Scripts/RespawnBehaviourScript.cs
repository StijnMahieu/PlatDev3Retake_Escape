using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnBehaviourScript : MonoBehaviour
{
    private CharacterBehaviourScript _characterBehaviourScript;

    void Start()
    {
        _characterBehaviourScript = this.gameObject.GetComponent<CharacterBehaviourScript>();
    }
    
    void Update()
    {
        Die();
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Deathzone")
        {
            SceneManager.LoadScene(2);
        }
    }
    public void Die()
    {
        if(_characterBehaviourScript.PlayerHealth<=0)
        {
            SceneManager.LoadScene(2);
        }
    }
}
