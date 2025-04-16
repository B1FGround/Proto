using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// Addressable 다운로드 핵심 로직 실행
/// <para>
/// 다운로드에 관련되어 이벤트를 발생 시켜야함
/// </para>
/// </summary>
public class AddressableDownloader
{
    // Addressable의 프로필에서 내가 지정해 놓은 다운로드 URL을 참조해야하는데 그때 어떤 인스턴스를 참조해 URL을 가져오는게 아니라 클래스이름으로 접근하기 위해 static으로 선언
    public static string DownloadURL;
    private DownloadEvents events;
    private string labelToDownload;
    private long totalSize;
    private AsyncOperationHandle downloadHandle;

    public DownloadEvents InitializeSystem(string label, string downloadURL)
    {
        events = new DownloadEvents();

        Addressables.InitializeAsync().Completed += OnInitialized;
        labelToDownload = label;
        DownloadURL = downloadURL;

        // 어드레서블에서 리소스 다운 받을 때 생기는 예외를 가져옴(Addressable에서 ResourceManager를 사용)
        ResourceManager.ExceptionHandler += OnException;

        return events;
    }

    public void Update()
    {
        if(downloadHandle.IsValid() &&
           downloadHandle.IsDone == false && 
           downloadHandle.Status != AsyncOperationStatus.Failed)
        {
            if(Utilities.IsNetworkValid() == false)
            {
                Debug.Log("Network is not valid");
                Addressables.Release(downloadHandle);
                downloadHandle = default;
                events.NotifyDownloadFinished(false);
                return;
            }
            
            var status = downloadHandle.GetDownloadStatus();

            long curDownloadedSize = status.DownloadedBytes;
            long remainedSize = totalSize - curDownloadedSize;

            var progressStatus = new DownloadProgressStatus(totalSize, curDownloadedSize, remainedSize, status.Percent);
            events.NotifyDownloadProgress(progressStatus);
        }
    }
    public void UpdateCatalog()
    {
        Addressables.CheckForCatalogUpdates().Completed += (result) =>
        {
            var catalogToUpdate = result.Result;
            if(catalogToUpdate.Count > 0)
                Addressables.UpdateCatalogs(catalogToUpdate).Completed += OnCatalogUpdate;
            else
                events.NotifyCatalogUpdated();
        };
    }
    public void DownloadSize()
    {
        Addressables.GetDownloadSizeAsync(labelToDownload).Completed += OnSizeDownloaded;
    }
    public void BundleDownload()
    {
        downloadHandle = Addressables.DownloadDependenciesAsync(labelToDownload);
        downloadHandle.Completed += OnDependenciesDownloaded;
    }

    #region Events
    private void OnInitialized(AsyncOperationHandle<IResourceLocator> handle)
    {
        events.NotifyInitilized();
    }
    private void OnCatalogUpdate(AsyncOperationHandle<List<IResourceLocator>> handle)
    {
        events.NotifyCatalogUpdated();
    }
    private void OnSizeDownloaded(AsyncOperationHandle<long> handle)
    {
        totalSize = handle.Result;

        if (Utilities.IsDiskSpaceEnough(totalSize) == false)
        {
            Debug.Log("Disk space is not enough");
            Addressables.Release(handle);
            events.NotifyDownloadFinished(false);
            return;
        }

        events.NotifySizeDownloaded(totalSize);

    }
    private void OnDependenciesDownloaded(AsyncOperationHandle handle)
    {
        events.NotifyDownloadFinished(handle.Status == AsyncOperationStatus.Succeeded);
    }
    private void OnException(AsyncOperationHandle handle, Exception exception)
    {
        Debug.LogError($"Addrssable download exception : {exception.Message}");

        if(exception is UnityEngine.ResourceManagement.Exceptions.RemoteProviderException)
        {
            // Remote 관련 에러
        }
    }
    #endregion
}