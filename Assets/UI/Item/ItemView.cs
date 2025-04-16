using JH.DataBinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemView : UIView
{
    [SerializeField] public Image itemImage;
    [SerializeField] public TMP_Text itemCount;
    [SerializeField] private Button itemDetailButton;
    [SerializeField] public GameObject outLine;

    private ItemPresenterParent presenter;

    public void SetInfo(string name, int count, ItemPresenterParent presenter)
    {
        this.presenter = presenter;

        itemDetailButton.enabled = true;

        presenter.view = this;

        presenter.SetInfo(name, count);

        if (name.Contains(' '))
            itemImage.sprite = Resources.Load<Sprite>("UI/" + name.Split(' ')[1]);
        else
            itemImage.sprite = Resources.Load<Sprite>("UI/" + name);

        outLine.SetActive(false);
        if (count == 1)
            itemCount.gameObject.SetActive(false);

        itemDetailButton.onClick.AddListener(OnClick);
    }

    public override void Open()
    {
    }

    public void SetData(string name, string count)
    {
        itemCount.text = count;
    }

    public void OnClick()
    {
        if (presenter is IClickableUIPresenter presenterForTeamInfoView)
            presenterForTeamInfoView.OnClick();
    }

    public void SetTeamInfoViewItemData(string itemName)
    {
        GameObject.Find("TeamInfoUI").GetComponent<TeamInfoView>().OnClickItem(itemName);
        outLine.SetActive(true);
    }
}