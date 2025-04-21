using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTransform;

    private void Start()
    {
        camTransform = Camera.main.transform; // 메인 카메라 가져오기
    }

    private void Update()
    {
        // 카메라를 향하도록 회전
        transform.LookAt(camTransform);

        // 180도 회전해서 좌우 반전 문제 해결
        transform.Rotate(0, 180, 0);
    }
}