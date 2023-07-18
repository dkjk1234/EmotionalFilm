using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCursorLock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CursorOn());
        Debug.Log("CursorLock");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CursorOn()
    {
        yield return new WaitForSeconds(0.1f);
        Cursor.lockState = CursorLockMode.None;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void TestDebugLog()
    {
        Debug.Log("Pushed Button!");
    }
}
