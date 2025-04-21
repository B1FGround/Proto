using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraAttractor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    private void FixedUpdate()
    {
        this.transform.position = cameraAttractor.transform.position;
    }
}