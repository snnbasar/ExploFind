using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotateEffect : MonoBehaviour
{
    private float multiplier = -500;

    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * multiplier);
    }
}
