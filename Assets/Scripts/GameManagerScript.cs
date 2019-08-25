using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public int AmountOfTargetsHit = 0;

    private DoorSlideScript _doorSlideScript;

    [SerializeField]
    private CharacterBehaviourScript _characterBehaviourScript;

    [SerializeField]
    private GameObject _door2;
    
    //health UI
    [SerializeField]
    private Sprite[] _healthSprites;
    [SerializeField]
    private Image _healthHearts;

    void Start ()
    {
        _doorSlideScript = _door2.GetComponent<DoorSlideScript>();

        //lock mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	void Update ()
    {
        OpenDoor2();
        UpdateHealth();
    }

    public void OpenDoor2()
    {
        if (AmountOfTargetsHit == 3)
        {
            _doorSlideScript.OpenDoor();
        }
    }

    private void UpdateHealth()
    {
        _healthHearts.sprite = _healthSprites[_characterBehaviourScript.PlayerHealth];
    }
}
