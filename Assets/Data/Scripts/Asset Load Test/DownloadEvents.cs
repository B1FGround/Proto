using System;
using UnityEngine;

/// <summary>
/// 다운로드 관련하여 이벤트들을 외부에서 등록할 수 있도록 하기 위한 클래스
/// </summary>
public class DownloadEvents
{
    // Addressable 초기화
    public event Action SystemInitilzedListener;
    public void NotifyInitilized() => SystemInitilzedListener?.Invoke();

    // Catalog 업데이트 완료
    public event Action CatalogUpdatedListener;
    public void NotifyCatalogUpdated() => CatalogUpdatedListener?.Invoke();

    // Size 다운로드 완료
    public event Action<long> SizeDownloadedListener;
    public void NotifySizeDownloaded(long size) => SizeDownloadedListener?.Invoke(size);

    // 다운로드 진행
    public event Action<DownloadProgressStatus> DownloadProgressListener;
    public void NotifyDownloadProgress(DownloadProgressStatus status) => DownloadProgressListener?.Invoke(status);

    // Bundle 다운로드 완료
    public event Action<bool> DownloadFinishedListener;
    public void NotifyDownloadFinished(bool isSuccess) => DownloadFinishedListener?.Invoke(isSuccess);
}