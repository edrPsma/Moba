using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EventInitialize { }

public struct EventInitPackage { }

public struct EventCheckVersion { }

public struct EventUpdateManifest { }

public struct EventDownload { }

public struct EventDownloadProgress
{
    public int TotalDownloadCount;
    public int CurrentDownloadCount;
    public long TotalDownloadSizeBytes;
    public long CurrentDownloadSizeBytes;
}

public struct EventEnterGame
{

}

public struct EventShowTips
{
    public string Content;
    public Action CallBack;
}