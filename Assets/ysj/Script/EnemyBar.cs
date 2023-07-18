using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBar : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 localOffset;
    public Vector3 screenOffset;
    RectTransform EnemyCanvas;
    // Start is called before the first frame update
    void Start()
    {
        EnemyCanvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 worldPoint = objectToFollow.TransformPoint(localOffset);
        Vector3 viewportPoint = Camera.main.WorldToViewportPoint(worldPoint);
        viewportPoint -= 0.5f * Vector3.one;
        viewportPoint.z = 0;


        transform.rotation = Quaternion.identity;
        transform.localPosition = viewportPoint + screenOffset;
    }
}
