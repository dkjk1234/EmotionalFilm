using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public WeaponScript weapon;
    public UIScript uIScript;
    public GameObject[] monsters;

    public P3dChangeCounter[] playerChangeCounter;
    public float playerHealth;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get {
            if(!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    //Start is called before the first frame update
    void Start()
    {
        monsters = GameObject.FindGameObjectsWithTag("Monster");
        playerChangeCounter = player.transform.GetComponentsInChildren<P3dChangeCounter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (weapon.paintRecovery && weapon.paintValue < 100)
        {
            weapon.paintValue += 0.02f;
        }

        playerHealth = (233664 - PlayerHealth()) / 2336.64f;

        
    }
    float PlayerHealth()
    {
        float total = 0;
        for (int i = 0; i < playerChangeCounter.Length; i++)
        {
            total += playerChangeCounter[i].Count;
        }
        return total;
    }

}