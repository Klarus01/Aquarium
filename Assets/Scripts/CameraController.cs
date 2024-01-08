using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotationSpeed = 50f;
    private float mixYRoattion = 0f;
    private float maxYRotation = 50f;

    private void Update()
    {
        float rotationInput = Input.GetAxis("Horizontal");
        float newRotation = transform.eulerAngles.y + rotationInput * rotationSpeed * Time.deltaTime;

        newRotation = Mathf.Clamp(newRotation, mixYRoattion, maxYRotation);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, newRotation, transform.eulerAngles.z);
    }
}