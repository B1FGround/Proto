using UnityEngine;
using VContainer;

public class UIHelloWorldPresenter : MonoBehaviour
{
    [Inject] private HelloWorldManager m_HelloWorldMgr;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        m_HelloWorldMgr.HelloWorld();
    }

    // Update is called once per frame
    private void Update()
    {
    }
}