using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    float delta = 0.0025f;

    void Update()
    {
        transform.Rotate(new Vector3(0, 30.0f*Time.deltaTime, 0), Space.Self);
        transform.Translate(0, delta * Mathf.Sin(Time.time), 0, Space.Self);
    }
}
