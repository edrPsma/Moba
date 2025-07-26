using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Linq;
using Template;
using DG.Tweening;
using Pool;
using YooAsset;

namespace Audio
{
    public class AudioManager : MonoSingleton<IAudioManager, AudioManager>, IAudioManager
    {
        class SourceItem
        {
            public AudioSource AudioSource;
            public int TaskId;
            public Action OnComplete;
            public AssetHandle AssetHandle;

            public void Reset()
            {
                AudioSource = null;
                GameEntry.Task.CancelTask(ref TaskId);
                OnComplete = null;
                AssetHandle?.Dispose();
                AssetHandle = null;
            }
        }
        enum VolumeType
        {
            EFFECT, BGM, Main
        }
        //缓存的Source
        Dictionary<int, SourceItem> mSources = new Dictionary<int, SourceItem>();
        GameObjectPool<AudioSource> mSourcePool;
        ObjectPool<SourceItem> mSourceItemPool;
        AudioMixer mAudioMixer;
        SourceItem mBGMSource = null;

        public float BGMVolume
        {
            get => DBTo01(PlayerPrefs.GetFloat("BGMVolume", 0));
            set => SetVolume(VolumeType.BGM, value);
        }
        public float EffectVolume
        {
            get => DBTo01(PlayerPrefs.GetFloat("EffectVolume", 0));
            set => SetVolume(VolumeType.EFFECT, value);
        }
        public float MainVolume
        {
            get => DBTo01(PlayerPrefs.GetFloat("MainVolume", 0));
            set => SetVolume(VolumeType.Main, value);
        }

        #region 公有方法
        protected override void OnInit()
        {
            mAudioMixer = Resources.Load<AudioMixer>("MainMixer");
            mSourcePool = new GameObjectPool<AudioSource>(EPoolType.Scalable, 30, Resources.Load<AudioSource>("AudioSource"), SetSourceParent, null);
            mSourcePool.OnReleaseEvent += ResetAudioSource;
            mSourceItemPool = new ObjectPool<SourceItem>(EPoolType.Scalable, 30);
            mSourceItemPool.OnReleaseEvent += item => item.Reset();
            mBGMSource = CreateSource(VolumeType.BGM);
            mBGMSource.AudioSource.loop = true;
            InitVolume();
            DontDestroyOnLoad(gameObject);
        }

        void ResetAudioSource(AudioSource source)
        {
            source.clip = null;
            source.loop = false;
            source.outputAudioMixerGroup = null;
        }

        void SetSourceParent(AudioSource source)
        {
            source.transform.SetParent(transform);
        }

        public void Pause(int id)
        {
            if (IsLoad(id))
            {
                mSources[id].AudioSource.Pause();
                GameEntry.Task.PauseTask(mSources[id].TaskId);
            }
        }

        public void Resume(int id)
        {
            if (IsLoad(id))
            {
                mSources[id].AudioSource.Play();
                if (!mSources[id].AudioSource.loop)
                {
                    var second = mSources[id].AudioSource.clip.length;
                    GameEntry.Task.AddTask(_ => WaitToRecycle(mSources[id]))
                        .Delay(TimeSpan.FromSeconds(second - mSources[id].AudioSource.time))
                        .Run();
                }
            }
        }

        public void Stop(int id)
        {
            if (IsLoad(id))
            {
                var sourceItem = mSources[id];
                mSources.Remove(id);
                sourceItem.AudioSource.Stop();
                mSourcePool.Release(sourceItem.AudioSource);
                mSourceItemPool.Release(sourceItem);
            }
        }

        public void StopBGM()
        {
            DOTween.To(() => mBGMSource.AudioSource.volume, volume => mBGMSource.AudioSource.volume = volume, 0, 1).OnComplete(() =>
            {
                mBGMSource.AudioSource.volume = 1;
                mBGMSource.AudioSource.Stop();
                mBGMSource.AssetHandle?.Release();
                mBGMSource.AssetHandle = null;
            });
        }

        public void StopAllEffect()
        {
            var sourceList = mSources.Keys.ToList();
            for (int i = 0; i < sourceList.Count; i++)
            {
                if (sourceList[i] != mBGMSource.AudioSource.GetInstanceID())
                {
                    Stop(sourceList[i]);
                }
            }
        }
        #endregion

        bool IsLoad(int id)
        {
            return mSources.ContainsKey(id);
        }


        SourceItem CreateSource(VolumeType type, Action onComplete = null)
        {
            var source = mSourcePool.SpawnByType();
            AudioMixerGroup group = null;
            switch (type)
            {
                case VolumeType.EFFECT:
                    group = mAudioMixer.FindMatchingGroups("Effect")[0];
                    break;
                case VolumeType.BGM:
                    group = mAudioMixer.FindMatchingGroups("BGM")[0];
                    break;
            }
            source.outputAudioMixerGroup = group;
            var sourceItem = mSourceItemPool.SpawnByType();
            sourceItem.AudioSource = source;
            sourceItem.OnComplete = onComplete;
            mSources.Add(source.GetInstanceID(), sourceItem);
            return sourceItem;
        }

        void SetVolume(VolumeType audioType, float volume)
        {
            volume = Mathf.Clamp(volume, 0.0001f, 1);
            var trueVolume = Mathf.Log10(volume) * 20.0f;
            switch (audioType)
            {
                case VolumeType.EFFECT:
                    mAudioMixer.SetFloat("EffectVolume", trueVolume);
                    PlayerPrefs.SetFloat("EffectVolume", trueVolume);
                    break;
                case VolumeType.BGM:
                    mAudioMixer.SetFloat("BGMVolume", trueVolume);
                    PlayerPrefs.SetFloat("BGMVolume", trueVolume);
                    break;
                case VolumeType.Main:
                    mAudioMixer.SetFloat("MainVolume", trueVolume);
                    PlayerPrefs.SetFloat("MainVolume", trueVolume);
                    break;
            }
        }

        void InitVolume()
        {
            SetVolume(VolumeType.Main, MainVolume);
            SetVolume(VolumeType.EFFECT, EffectVolume);
            SetVolume(VolumeType.BGM, BGMVolume);
        }

        float DBTo01(float volume)
        {
            return Mathf.Pow(10, volume / 20.0f);
        }

        public void PlayEffect(string location, float volume = 1, bool loop = false, Action<int> onPlayStart = null, Action onComplete = null)
        {
            GetClip(location, (clip, assethandle) => OnEffectLoadOver(clip, assethandle, volume, loop, onPlayStart, onComplete));
        }

        public void PlayBGM(string location, float volume = 1)
        {
            GetClip(location, (clip, assethandle) => OnBGMLoadOver(clip, assethandle, volume));
        }

        private void OnEffectLoadOver(AudioClip clip, AssetHandle assetHandle, float volume, bool loop, Action<int> onPlayStart, Action onComplete)
        {
            if (clip == null)
            {
                onPlayStart?.Invoke(0);
            }
            else
            {
                SourceItem sourceItem = CreateSource(VolumeType.EFFECT, onComplete);
                sourceItem.AudioSource.clip = clip;
                sourceItem.AudioSource.loop = loop;
                sourceItem.AudioSource.volume = volume;
                sourceItem.AssetHandle = assetHandle;
                // SetVolume(AudioType.EFFECT, EffectVolume);
                sourceItem.AudioSource.Play();

                if (!loop)
                {
                    var second = sourceItem.AudioSource.clip.length;
                    GameEntry.Task.AddTask(_ => WaitToRecycle(sourceItem))
                        .Delay(TimeSpan.FromSeconds(second - sourceItem.AudioSource.time))
                        .Run();
                }

                onPlayStart?.Invoke(sourceItem.AudioSource.GetInstanceID());
            }
        }

        void WaitToRecycle(SourceItem sourceItem)
        {
            sourceItem.OnComplete?.Invoke();
            mSources.Remove(sourceItem.AudioSource.GetInstanceID());
            mSourcePool.Release(sourceItem.AudioSource);
            mSourceItemPool.Release(sourceItem);
        }

        private void OnBGMLoadOver(AudioClip clip, AssetHandle assetHandle, float volume)
        {
            if (mBGMSource.AudioSource.clip == clip && mBGMSource.AudioSource.isPlaying)
            {
                assetHandle.Release();
                return;
            }

            AssetHandle pre = mBGMSource.AssetHandle;
            mBGMSource.AssetHandle = assetHandle;
            if (mBGMSource.AudioSource.clip == null)
            {
                // SetVolume(AudioType.BGM, BGMVolume);
                mBGMSource.AudioSource.clip = clip;
                mBGMSource.AudioSource.volume = Mathf.Clamp01(volume);
                mBGMSource.AudioSource.Play();
            }
            else
            {
                DOTween.To(() => mBGMSource.AudioSource.volume, volume => mBGMSource.AudioSource.volume = volume, 0, 1).OnComplete(() =>
                {
                    pre?.Release();
                    mBGMSource.AudioSource.volume = volume;
                    mBGMSource.AudioSource.clip = clip;
                    mBGMSource.AudioSource.Play();
                });
            }
        }

        void GetClip(string location, Action<AudioClip, AssetHandle> loadOver)
        {
            AssetHandle assetHandle = GameEntry.Resource.LoadAssetAsync<AudioClip>(location);
            assetHandle.Completed += handle =>
            {
                AudioClip clip = handle.AssetObject as AudioClip;
                OnClipLoadOver(location, clip, handle, loadOver);
            };
        }

        private void OnClipLoadOver(string location, AudioClip clip, AssetHandle assetHandle, Action<AudioClip, AssetHandle> callBack)
        {
            callBack?.Invoke(clip, assetHandle);
        }
    }
}