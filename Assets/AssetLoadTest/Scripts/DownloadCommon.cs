using UnityEngine;

public struct DownloadProgressStatus
{
    public long totalBytes;
    public long downloadedBytes;
    public long remainedBytes;
    public float totalProgress; // 0.0 ~ 1.0

    public DownloadProgressStatus(long totalBytes, long downloadedBytes, long remainedBytes, float totalProgress)
    {
        this.totalBytes = totalBytes;
        this.downloadedBytes = downloadedBytes;
        this.remainedBytes = remainedBytes;
        this.totalProgress = totalProgress;
    }
}
