using UnityEngine;

public class RotateCenter : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}