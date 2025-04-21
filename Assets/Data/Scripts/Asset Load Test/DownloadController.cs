using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// AddressableDownloader의 기능을 외부에서 조작
/// </summary>
public class DownloadController : MonoBehaviour
{
    public enum State
    {
        Idle,
        Initialize,
        UpdateCatalog,
        DownloadSize,
        DownloadBundle,
        Downloading,
        Finished
    }
    [SerializeField] string LabelToDownload;
    [SerializeField] string DownloadURL;

    public State CurrentState { get; private set; } = State.Idle;
    public State LastValidState { get; private set; } = State.Idle; // Idle상태를 제외한 상태만 가지는 변수

    AddressableDownloader addressableDownloader;
    Action<DownloadEvents> onEventAction;

    public IEnumerator StartDownloadRoutine(Action<DownloadEvents> onEventAction)
    {
        this.addressableDownloader = new AddressableDownloader();
        this.onEventAction = onEventAction;

        LastValidState = CurrentState = State.Initialize;
        
        while(CurrentState != State.Finished)
        {
            OnExcute();
            yield return null;
        }
    }

    void OnExcute()
    {
        if (CurrentState == State.Idle)
            return;

        switch(CurrentState)
        {
            case State.Initialize:
                var events = addressableDownloader.InitializeSystem(LabelToDownload, DownloadURL);
                onEventAction?.Invoke(events);
                CurrentState = State.Idle; // 위에서 비동기 대기를 하기 때문에 Idle로 바꿔줘야함
                break;
            case State.UpdateCatalog:
                addressableDownloader.UpdateCatalog();
                CurrentState = State.Idle;
                break;
            case State.DownloadSize:
                addressableDownloader.DownloadSize();
                CurrentState = State.Idle;
                break;
            case State.DownloadBundle:
                addressableDownloader.BundleDownload();
                CurrentState = State.Downloading;
                break;
            case State.Downloading:
                addressableDownloader.Update();
                break;
            case State.Finished:
                break;
        }
    }

    public void GoNext()
    {
        if(LastValidState == State.Initialize)
            CurrentState = State.UpdateCatalog;
        else if (LastValidState == State.UpdateCatalog)
            CurrentState = State.DownloadSize;
        else if (LastValidState == State.DownloadSize)
            CurrentState = State.DownloadBundle;
        else if (LastValidState == State.DownloadBundle || LastValidState == State.Downloading)
            CurrentState = State.Finished;

        LastValidState = CurrentState;
    }
}