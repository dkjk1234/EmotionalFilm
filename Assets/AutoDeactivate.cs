using System;
using UnityEngine;

public class AutoDeactivate : MonoBehaviour
{
    public float deactivateAfterSeconds = 5f;  // GameObject�� ��Ȱ��ȭ�� �ð�(��)

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            Deactivate();
        }
    }

    void OnEnable()
    {
        Invoke("Deactivate", deactivateAfterSeconds);  // ������ �ð� �Ŀ� Deactivate() �޼��带 ȣ���մϴ�.
    }

    void Deactivate()
    {
        gameObject.SetActive(false);  // GameObject�� ��Ȱ��ȭ�մϴ�.
    }

    void OnDisable()
    {
        CancelInvoke();  // GameObject�� ��Ȱ��ȭ�Ǹ� ��� ����� Invoke�� ����մϴ�.
    }
}
