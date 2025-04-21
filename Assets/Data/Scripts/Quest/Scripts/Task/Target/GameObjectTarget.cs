using UnityEngine;

[CreateAssetMenu(fileName = "Target_", menuName = "Quest/Task/Target/GameObject")]
public class GameObjectTarget : TaskTarget
{
    [SerializeField] private GameObject value;
    public override object Value => value;

    // 여기서는 prefab일 수도 있고, 씬에 있는 오브젝트일 수도 있어서 이름을 비교함.
    // 만약 게임 오브젝트를 생성하고 이름을 바꿨다면 다른 방식을 사용해야함
    public override bool IsEqual(object target)
    {
        var targetAsGameObject = target as GameObject;
        if(targetAsGameObject == null)
            return false;

        return targetAsGameObject.name.Contains(value.name);
    }

}
