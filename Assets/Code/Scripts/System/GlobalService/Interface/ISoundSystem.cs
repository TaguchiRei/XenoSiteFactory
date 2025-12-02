using System.GlobalService.Interface;

namespace Xenosite.System.GlobalService.Interface
{
    public interface IAudioMasterSettings : ICoreSystem
    {
        /// <summary>
        /// マスターボリュームを設定します（0~1）
        /// </summary>
        public void SetMasterVolume(float volume);

        /// <summary>
        /// 全体の音量を取得します
        /// </summary>
        /// <returns></returns>
        public float GetMasterVolume();

        /// <summary>
        /// 全体ミュートを設定します
        /// </summary>
        public void SetMute(bool mute);

        /// <summary>
        /// ミュート状態を取得します
        /// </summary>
        /// <returns></returns>
        public bool IsMuted();
    }

    /// <summary>
    /// 効果音を再生する機能を持ちます
    /// </summary>
    public interface ISoundSystem : ICoreSystem
    {
        /// <summary>
        /// 効果音を再生します
        /// </summary>
        /// <param name="key"></param>
        /// <param name="volume">0~1</param>
        public void PlaySoundEffect(string key, float volume);

        /// <summary>
        /// 指定した効果音が再生中なら停止します
        /// </summary>
        /// <param name="key"></param>
        public void StopSoundEffect(string key);

        /// <summary>
        /// 指定した効果音が再生中なら一時停止します
        /// </summary>
        /// <param name="key"></param>
        public void PauseSoundEffect(string key);

        /// <summary>
        /// 指定した効果音が一時停止中なら再生します
        /// </summary>
        /// <param name="key"></param>
        public void ResumeSoundEffect(string key);

        /// <summary>
        /// 指定した効果音が再生中なら音量を変えます
        /// </summary>
        /// <param name="key"></param>
        /// <param name="volume">0~1</param>
        public void SetSoundEffectVolume(string key, float volume);

        /// <summary>
        /// 再生中のすべての効果音を一時停止します
        /// </summary>
        public void PauseAllSoundEffect();

        /// <summary>
        /// 停止しているすべての効果音を再開します
        /// </summary>
        public void ResumeAllSoundEffect();

        /// <summary>
        /// すべての効果音を停止します。再開はできません
        /// </summary>
        public void StopAllSoundEffect();

        /// <summary>
        /// すべての効果音の音量を設定します
        /// </summary>
        /// <param name="volume">0~1</param>
        public void SetAllSoundEffectVolume(float volume);
    }

    /// <summary>
    /// BGMを再生する機能を持ちます
    /// </summary>
    public interface IBGMSystem : ICoreSystem
    {
        /// <summary>
        /// BGMを再生します
        /// </summary>
        /// <param name="key"></param>
        /// <param name="volume">0~1</param>
        public void PlayBGM(string key, float volume);

        /// <summary>
        /// BGMを停止します。再開はできません
        /// </summary>
        /// <param name="key"></param>
        public void StopBGM(string key);

        /// <summary>
        /// 再生中のBGMのIDをすべて取得します
        /// </summary>
        /// <returns></returns>
        public string[] GetPlayingBGM();

        /// <summary>
        ///　指定したBGMが再生中なら一時停止します
        /// </summary>
        /// <param name="key"></param>
        public void PauseBGM(string key);

        /// <summary>
        /// 指定したBGMが停止中なら再開します
        /// </summary>
        /// <param name="key"></param>
        public void ResumeBGM(string key);

        /// <summary>
        /// 指定したBGMが再生中なら音量を調節します
        /// </summary>
        /// <param name="key"></param>
        /// <param name="volume">0~1</param>
        public void SetBGMVolume(string key, float volume);
    }
}