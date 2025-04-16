using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject cardSelectUI;
    [SerializeField] public GameObject craftUI;
    [SerializeField] public GameObject inventoryUI;
    [SerializeField] public GameObject teamUI;

    private CharacterController cc;
    private GameObject monsters;

    public MoveTypeBase MoveType;
    public DodgeTypeBase DodgeType;

    public int maxExp = 1;
    public int curExp = 0;

    public Image sceneChangeBg;

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        Application.targetFrameRate = 60;

        monsters = GameObject.FindWithTag("MonsterContainer");
        MoveType = new BasicMoveType(cc, transform, 10f, 10f, () => DodgeType.isDodging, GetClosestMonsters);
        DodgeType = new BasicDodgeType(cc, 0.4f, 15f, () => MoveType.moveDirection);
    }

    private void Update()
    {
        if (Time.timeScale == 0f || craftUI.activeSelf || (inventoryUI && inventoryUI.activeSelf))
            return;

        MoveType?.OnAction();

        DodgeType?.OnAction();
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

    public void GetExp()
    {
        curExp += 1;
        if (curExp >= maxExp)
        {
            curExp = 0;
            maxExp += 1;
            cardSelectUI.SetActive(true);
        }
        MoveType.moveTarget = null;
    }

    public void OpenCraftUI() => UIManager.Instance.Open<CraftView>();

    public void OpenInventoryUI() => inventoryUI.SetActive(true);

    public void OpenTeamUI()
    {
        if (teamUI)
        {
            teamUI.SetActive(true);
            teamUI.GetComponent<TeamInfoView>().SetPresenter(new TeamInfoPresenter());
            teamUI.GetComponent<TeamInfoView>().OnClickRangerButton();
        }
    }

    public void SceneChange() => SceneController.Instance.SceneChange("Dungeon", transform.position, GetComponent<TeamContainer>().GetCharacters(), sceneChangeBg);
}