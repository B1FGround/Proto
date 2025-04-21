using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DownloadPopup : MonoBehaviour
{
    public enum State
    {
        None, 

        CalculatingSize,
        NothginToDownload,

        AskingDownload,
        Downloading,
        DownloadFinished
    }

    // 상태에 따라 게임오브젝트를 활성화/비활성화 시키기 위한 클래스
    [Serializable] public class Root
    {
        public State state;
        public GameObject root;
    }

    [SerializeField] List<Root> roots;
    [SerializeField] TMP_Text title;
    [SerializeField] TMP_Text desc;
    [SerializeField] TMP_Text downloadingBarStatus;

    [SerializeField] Slider downloadProgressBar;
    [SerializeField] DownloadController downloader;

    public State CurrentState { get; private set; } = State.None;

    DownloadProgressStatus progressInfo;
    SizeUnits sizeUnit;

    long curDownloadSizeInUnit;
    long totalSizeInUnit;


    private IEnumerator Start()
    {
        SetState(State.CalculatingSize, true);
        yield return downloader.StartDownloadRoutine((events) =>
        {
            events.SystemInitilzedListener += OnInitialized;
            events.CatalogUpdatedListener += OnCatalogUpdate;
            events.SizeDownloadedListener += OnSizeDownloaded;
            events.DownloadProgressListener += OnDownloadProgress;
            events.DownloadFinishedListener += OnDownloadFinished;
        });
    }

    void SetState(State newState, bool updateUI)
    {
        var prevRoot = roots.Find(x => x.state == CurrentState);
        var newRoot = roots.Find(x => x.state == newState);

        CurrentState = newState;

        if(prevRoot != null)
            prevRoot.root.SetActive(false);
        if(newRoot != null)
            newRoot.root.SetActive(true);

        if(updateUI)
            UpdateUI();
    }

    void UpdateUI()
    {
        if(CurrentState == State.CalculatingSize)
        {
            title.text = "Alert";
            desc.text = "Calculating file size for download";
        }
        else if(CurrentState == State.NothginToDownload)
        {
            title.text = "Alert";
            desc.text = "There is no file to download.";
        }
        else if (CurrentState == State.AskingDownload)
        {
            title.text = "Caution";
            desc.text = $"The size of the file to be downloaded is <color=green>({this.totalSizeInUnit}{sizeUnit})</color>.\n" + $"Do you want to proceed with the download?";
        }
        else if (CurrentState == State.Downloading)
        {
            title.text = "Downloading";
            desc.text = $"Downloading. {(progressInfo.totalProgress * 100).ToString("0.00")}% completed";
            downloadProgressBar.value = progressInfo.totalProgress;
            downloadingBarStatus.text = $"{curDownloadSizeInUnit}/{totalSizeInUnit}{sizeUnit}";
        }
        else if (CurrentState == State.DownloadFinished)
        {
            title.text = "Completed";
            desc.text = "Download completed. Would you like to play game?";
        }
    }

    public void OnClickStartDownloadButton()
    {
        SetState(State.Downloading, true);
        downloader.GoNext();
    }
    public void OnClickCancleButton()
    {
#if UNITY_EDITOR
        if(Application.isEditor)
            UnityEditor.EditorApplication.isPlaying = false;

#else
        Application.Quit();
#endif
    }
    public void OnClickEnterGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TestInGame");
    }

    private void OnInitialized()
    {
        downloader.GoNext();
    }
    private void OnCatalogUpdate()
    {
        downloader.GoNext();
    }
    private void OnSizeDownloaded(long size)
    {
        if(size == 0)
            SetState(State.NothginToDownload, true);
        else
        {
            sizeUnit = Utilities.GetProperByteUnit(size);
            totalSizeInUnit = Utilities.ConvertByteByUnit(size, sizeUnit);

            SetState(State.AskingDownload, true);
        }
    }
    private void OnDownloadProgress(DownloadProgressStatus newInfo)
    {
        bool changed = this.progressInfo.downloadedBytes != newInfo.downloadedBytes;

        progressInfo = newInfo;

        if(changed)
        {
            UpdateUI();
            curDownloadSizeInUnit = Utilities.ConvertByteByUnit(progressInfo.downloadedBytes, sizeUnit);
        }
    }
    private void OnDownloadFinished(bool isSuccess)
    {
        SetState(State.DownloadFinished, true);
        downloader.GoNext();
    }   


}
