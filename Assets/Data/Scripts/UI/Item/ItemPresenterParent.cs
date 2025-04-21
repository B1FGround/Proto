using JH.DataBinding;
using System.Data;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemPresenterParent
{
    protected ItemModel model;
    public ItemView view;


    public ItemPresenterParent()
    {
        model = new ItemModel();
    }

    public void SetInfo(string name, int count)
    {
        var data = model.SetInfo(name, count);
        view.SetData(data.Item1, data.Item2);
    }
    public string GetName()
    {
        return model.ItemName;
    }
}