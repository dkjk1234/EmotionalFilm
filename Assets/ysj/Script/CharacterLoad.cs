using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoad : MonoBehaviour
{
    public GameObject characterPrefab;
    public Transform characterSpawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        LoadCharacter();
    }

    // Update is called once per frame
    private void LoadCharacter()
    {
        int currentHairIndex = PlayerPrefs.GetInt("CurrentHairIndex");
        GameObject player = Instantiate(characterPrefab, characterSpawnPoint.position, characterSpawnPoint.rotation);

    }
}
