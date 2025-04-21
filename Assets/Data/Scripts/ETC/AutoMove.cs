using UnityEngine;
using UnityEngine.UI;

public class AutoMove : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private Button autoButton;

    private void Start()
    {
        autoButton = GetComponent<Button>();
        autoButton.onClick.AddListener(() =>
        {
            playerController.MoveType.auto = !playerController.MoveType.auto;
        });
    }
}