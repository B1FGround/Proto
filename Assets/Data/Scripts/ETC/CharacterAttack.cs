using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public AttackTypeBase AttackType;
    public Animator animator;
    public Transform bodyBone;
    public GameObject weapon;
    private GameObject monsters;

    private PlayerController playerController;

    private GameObject chainLightning;
    private TeamManager.CharacterType characterType;

    [SerializeField] private GameObject armorSlot;
    [SerializeField] private GameObject bottomSlot;
    [SerializeField] private GameObject leftLegSlot;
    [SerializeField] private GameObject rightLegSlot;

    public void Initialized(TeamManager.CharacterType characterType)
    {
        this.characterType = characterType;
        animator = GetComponent<Animator>();
        playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        switch (characterType)
        {
            case TeamManager.CharacterType.Warrior:
                AttackType = new MeleeAttackType(animator, 5f, 1, 1f, 0, GetClosestMonsters, bodyBone, weapon);
                break;

            case TeamManager.CharacterType.Ranger:
                AttackType = new BasicAttackType(animator, 10f, 1, 1f, 0, GetClosestMonsters, bodyBone, weapon, this);
                break;

            case TeamManager.CharacterType.Magician:
                chainLightning = Instantiate(Resources.Load("Prefabs/Effect/ChainLightning")) as GameObject;
                chainLightning.transform.SetParent(this.gameObject.transform);
                AttackType = new MagicAttackType(animator, 10f, 1, 1f, 0, GetClosestMonsters, bodyBone, weapon, chainLightning);
                break;
        }
        monsters = GameObject.FindWithTag("MonsterContainer");
        Equip(this.characterType);
    }

    private List<GameObject> GetClosestMonsters(int count, float detectDistance)
    {
        if (monsters is null)
            return null;

        var monstersInRange = new List<GameObject>();

        for (int i = 0; i < monsters.transform.childCount; i++)
        {
            var monster = monsters.transform.GetChild(i).gameObject;
            float distance = Vector3.Distance(transform.position, monster.transform.position);

            if (distance <= detectDistance)
                monstersInRange.Add(monster);
        }

        return monstersInRange.OrderBy(m => Vector3.Distance(transform.position, m.transform.position)).Take(count).ToList();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Time.timeScale == 0f || playerController.craftUI.activeSelf || (playerController.inventoryUI && playerController.inventoryUI.activeSelf))
            return;

        AttackType?.OnAction();
    }

    public void AddPower(CardInfo.CardType cardType)
    {
        switch (cardType)
        {
            case CardInfo.CardType.Damage:
                AttackType.Damage += 1;
                break;

            case CardInfo.CardType.AttackSpeed:
                AttackType.AttackSpeed += 0.5f;
                break;

            case CardInfo.CardType.AdditionalAttack:
                AttackType.AdditionalAttack += 1;
                break;
        }
    }

    public void Equip(TeamManager.CharacterType selected)
    {
        if (selected != characterType)
            return;

        if (InventoryManager.Instance.equipped.ContainsKey(characterType) == false)
            return;

        var equippedItem = InventoryManager.Instance.equipped[characterType];

        if (equippedItem.ContainsKey(IEquipable.EquipSocket.Armor))
        {
            var armorName = equippedItem[IEquipable.EquipSocket.Armor].itemModel.ItemName.Split()[1];

            var armorObj = Instantiate(Resources.Load("Prefabs/Equip/" + armorName), Vector3.zero, Quaternion.identity, armorSlot.transform) as GameObject;
            armorObj.transform.localPosition = Vector3.zero;
        }
        if (equippedItem.ContainsKey(IEquipable.EquipSocket.Bottom))
        {
            var armorName = equippedItem[IEquipable.EquipSocket.Bottom].itemModel.ItemName.Split()[1];

            var armorObj = Instantiate(Resources.Load("Prefabs/Equip/" + armorName), Vector3.zero, Quaternion.identity, bottomSlot.transform) as GameObject;
            armorObj.transform.localPosition = Vector3.zero;
        }
        if (equippedItem.ContainsKey(IEquipable.EquipSocket.Leg))
        {
            var armorName = equippedItem[IEquipable.EquipSocket.Leg].itemModel.ItemName.Split()[1];

            var armorObj = Instantiate(Resources.Load("Prefabs/Equip/Left" + armorName), Vector3.zero, Quaternion.identity, leftLegSlot.transform) as GameObject;
            armorObj.transform.localPosition = Vector3.zero;

            armorObj = Instantiate(Resources.Load("Prefabs/Equip/Right" + armorName), Vector3.zero, Quaternion.identity, rightLegSlot.transform) as GameObject;
            armorObj.transform.localPosition = Vector3.zero;
        }
    }
    public void Equip(TeamManager.CharacterType selected, CraftData.ItemDetail itemDetail)
    {
        if (selected != characterType)
            return;

        if (InventoryManager.Instance.equipped.ContainsKey(characterType) == false)
            return;

        var equippedItem = InventoryManager.Instance.equipped[characterType];

        if (itemDetail == CraftData.ItemDetail.Armor)
        {
            var armorName = equippedItem[IEquipable.EquipSocket.Armor].itemModel.ItemName.Split()[1];

            var armorObj = Instantiate(Resources.Load("Prefabs/Equip/" + armorName), Vector3.zero, Quaternion.identity, armorSlot.transform) as GameObject;
            armorObj.transform.localPosition = Vector3.zero;
        }
        if (itemDetail == CraftData.ItemDetail.Bottom)
        {
            var armorName = equippedItem[IEquipable.EquipSocket.Bottom].itemModel.ItemName.Split()[1];

            var armorObj = Instantiate(Resources.Load("Prefabs/Equip/" + armorName), Vector3.zero, Quaternion.identity, bottomSlot.transform) as GameObject;
            armorObj.transform.localPosition = Vector3.zero;
        }
        if (itemDetail == CraftData.ItemDetail.Leg)
        {
            var armorName = equippedItem[IEquipable.EquipSocket.Leg].itemModel.ItemName.Split()[1];

            var armorObj = Instantiate(Resources.Load("Prefabs/Equip/Left" + armorName), Vector3.zero, Quaternion.identity, leftLegSlot.transform) as GameObject;
            armorObj.transform.localPosition = Vector3.zero;

            armorObj = Instantiate(Resources.Load("Prefabs/Equip/Right" + armorName), Vector3.zero, Quaternion.identity, rightLegSlot.transform) as GameObject;
            armorObj.transform.localPosition = Vector3.zero;
        }
    }
}