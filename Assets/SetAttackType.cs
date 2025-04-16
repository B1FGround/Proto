using UnityEngine;
using UnityEngine.UI;

public class SetAttackType : MonoBehaviour
{
    [SerializeField] private Button rangeTypeButton;
    [SerializeField] private Button meleeTypeButton;
    [SerializeField] private Button magicTypeButton;

    private void Start()
    {
        var playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        //rangeTypeButton.onClick.AddListener(() => playerController.SetRangeAttackType());
        //meleeTypeButton.onClick.AddListener(() => playerController.SetMeleeAttackType());
        //magicTypeButton.onClick.AddListener(() => playerController.SetMagicAttackType());
    }

    // Update is called once per frame
    private void Update()
    {
    }
}