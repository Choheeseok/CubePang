using UnityEngine;

public class Item : MonoBehaviour
{
    private const float ROTATION_SPEED = 30.0f;
    private const float FLOAT_AMPLITUDE = 0.0025f;
    private const float FLOAT_FREQUENCY = 1.0f;

    private void Update()
    {
        RotateItem();
        FloatItem();
    }

    private void RotateItem()
    {
        transform.Rotate(new Vector3(0, ROTATION_SPEED * Time.deltaTime, 0), Space.Self);
    }

    private void FloatItem()
    {
        float verticalOffset = FLOAT_AMPLITUDE * Mathf.Sin(Time.time * FLOAT_FREQUENCY);
        transform.Translate(0, verticalOffset, 0, Space.Self);
    }
}
