using UnityEngine;

public class AnimationEvent : MonoBehaviour
{
    public GameObject player;

    public void DodgeEnd()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerController>().DodgeType.isDodging = false;
        //player.GetComponent<PlayerController>().isRolling = false;
    }

    public void AttackEvent()
    {
        player.GetComponent<CharacterAttack>().AttackType.ThrowProjectile();
        //player.GetComponent<PlayerController>().AttackType.ThrowProjectile();
    }
}