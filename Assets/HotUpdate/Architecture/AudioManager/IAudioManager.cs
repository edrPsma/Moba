using System;

/// <summary>
/// 音效管理器
/// </summary>
public interface IAudioManager
{
    /// <summary>
    /// 主音量
    /// </summary>
    float MainVolume { get; set; }

    /// <summary>
    /// 背景音乐音量
    /// </summary>
    float BGMVolume { get; set; }

    /// <summary>
    /// 音效音量
    /// </summary>
    float EffectVolume { get; set; }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="location">位置</param>
    /// <param name="loop">是否循环</param>
    /// <param name="onPlayStart">开始播放回调</param>
    /// <param name="onComplete">结束播放回调</param>
    void PlayEffect(string location, float volume = 1, bool loop = false, Action<int> onPlayStart = null, Action onComplete = null);

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="location">位置</param>
    void PlayBGM(string location, float volume = 1);

    /// <summary>
    /// 暂停播放
    /// </summary>
    /// <param name="id">播放id</param>
    void Pause(int id);

    /// <summary>
    /// 恢复播放
    /// </summary>
    /// <param name="id">播放id</param>
    void Resume(int id);

    /// <summary>
    /// 停止播放
    /// </summary>
    /// <param name="id">播放id</param>
    void Stop(int id);

    /// <summary>
    /// 停止播放背景音乐
    /// </summary>
    void StopBGM();

    /// <summary>
    /// 停止播放所有音效
    /// </summary>
    void StopAllEffect();
}
