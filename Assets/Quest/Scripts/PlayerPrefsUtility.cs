using UnityEngine;

public class PlayerPrefsUtillity : MonoBehaviour
{
    [ContextMenu("DeleteSaveData")]
    void DeleteSaveData()
    {
        PlayerPrefs.DeleteAll();
    }
}
