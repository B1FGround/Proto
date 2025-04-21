using UnityEngine;

public class Rotatable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.rotation = Quaternion.Euler(CameraManager.Instance.GetRotX(), this.transform.eulerAngles.y, this.transform.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation = Quaternion.Euler(CameraManager.Instance.GetRotX(), this.transform.eulerAngles.y, this.transform.eulerAngles.z);

    }
}
