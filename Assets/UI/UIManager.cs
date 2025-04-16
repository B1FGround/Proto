using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public Dictionary<Type, UIView> uiViewDictionary = new Dictionary<Type, UIView>();
    private Stack<UIView> UIViews { get; set; } = new Stack<UIView>();
    private GameObject uiCanvas;

    protected override void Awake()
    {
        base.Awake();
        FindView();
    }

    public void Open<T>() where T : UIView
    {
        var view = Get<T>();
        PushUIView(view);
    }

    public void Close()
    {
        PopupUIView();
    }

    private void PopupUIView()
    {
        if (UIViews.Count <= 0)
            return;
        UIViews.Pop().ActivateUIView(false);

        if (UIViews.TryPeek(out UIView view) == true)
            view.ActivateUIView(true);
    }

    private void PushUIView(UIView view)
    {
        if (view == null)
            return;

        if (UIViews.Count > 0)
        {
            if (UIViews.Peek() == view)
                return;

            UIViews.Peek().ActivateUIView(false);
        }
        UIViews.Push(view);
        view.ActivateUIView(true);
        view.Open();
    }

    public T Get<T>() where T : UIView
    {
        if (uiViewDictionary.TryGetValue(typeof(T), out var view))
        {
            return view as T;
        }
        return null;
    }

    public void ResetUIManager()
    {
        foreach (var view in uiViewDictionary.Values)
            view.ActivateUIView(false);

        uiViewDictionary.Clear();
        UIViews.Clear();
        uiCanvas = null;

        FindView();
    }

    public void FindView()
    {
        uiCanvas = GameObject.FindWithTag("UICanvas");

        if (uiCanvas == null)
        {
            Debug.LogError("Can't find UICanvas.");
            return;
        }

        UIView[] uiViews = uiCanvas.GetComponentsInChildren<UIView>(true);

        foreach (var view in uiViews)
        {
            var type = view.GetType();
            if (!uiViewDictionary.ContainsKey(type))
                uiViewDictionary[type] = view;
            else
                Debug.LogWarning($"UI type for key is duplicated: {type}");
        }
    }
}