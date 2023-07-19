using System;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    public float deactivateAfterSeconds = 5f;  // GameObject를 비활성화할 시간(초)

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Deactivate();
        }
    }

    void OnEnable()
    {
        Invoke("Deactivate", deactivateAfterSeconds);  // 지정된 시간 후에 Deactivate() 메서드를 호출합니다.
    }

    void Deactivate()
    {
        gameObject.SetActive(false);  // GameObject를 비활성화합니다.
    }

    void OnDisable()
    {
        CancelInvoke();  // GameObject가 비활성화되면 모든 예약된 Invoke를 취소합니다.
    }
}
