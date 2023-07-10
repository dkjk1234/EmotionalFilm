using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHpBar : MonoBehaviour
{
    private Camera uiCamera;
    private Canvas canvas;
    private RectTransform rectParent;
    private RectTransform rectHp;

    [HideInInspector] public Vector3 offset = Vector3.zero;
    [HideInInspector] public Transform targetTr;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCamera = canvas.worldCamera;
        rectParent = GetComponentInParent<RectTransform>();
        rectHp = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        if (targetTr != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

            if (screenPos.z < 0.0f)
            {
                screenPos *= -1.0f;
            }

            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, uiCamera, out localPos);

            rectHp.localPosition = new Vector3(localPos.x, localPos.y, 0);
        }
    }
}
