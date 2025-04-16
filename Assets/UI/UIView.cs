using Unity.VisualScripting;
using UnityEngine;

public abstract class UIView : MonoBehaviour
{
    public void ActivateUIView(bool value)
    {
        gameObject.SetActive(value);
    }

    public abstract void Open();

    public void Close()
    {
        UIManager.Instance.Close();
    }
}