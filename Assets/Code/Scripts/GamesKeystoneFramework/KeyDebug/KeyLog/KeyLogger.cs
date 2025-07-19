using System;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;
using Object = UnityEngine.Object;

namespace GamesKeystoneFramework.KeyDebug.KeyLog
{
    public static class KeyLogger
    {
        private static Canvas _canvas;
        private static TextMeshProUGUI _logText;
        private static KeyTesterUpdateMonitoring _updateMonitor;
        public static TMP_FontAsset FontAsset;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            //キャンバス作成
            var logCanvas = new GameObject("KeyTesterCanvas");
            _canvas = logCanvas.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var logCanvasScaler = logCanvas.AddComponent<CanvasScaler>();
            logCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

            //キャンバスにUpdate監視追加
            _updateMonitor = logCanvas.AddComponent<KeyTesterUpdateMonitoring>();

            //ログテキスト作成
            var logTextObject = new GameObject("KeyTesterLogText");
            _logText = logTextObject.AddComponent<TextMeshProUGUI>();
            _logText.transform.SetParent(_canvas.transform);

            //ログテキストの位置を決定
            _logText.rectTransform.anchorMax = Vector2.up;
            _logText.rectTransform.anchorMin = Vector2.up;
            _logText.rectTransform.pivot = Vector2.up;
            _logText.rectTransform.anchoredPosition = Vector2.zero;

            //ログテキストの初期化
            _logText.textWrappingMode = TextWrappingModes.NoWrap;
            try
            {
                _logText.font = Resources.Load<TMP_FontAsset>("Fonts/DefaultJapaneseFontAsset");
            }
            catch (Exception e)
            {
                Debug.Log("Fonts/DefaultJapaneseFontAsset Not Found");
                throw;
            }

            _logText.richText = true;
            _logText.fontSize = 14;

            //_allLogs準備

            Log("<color=purple>KeyTester</color> <color=black>:</color> Initialized", Color.cyan);
            Object.DontDestroyOnLoad(logCanvas.gameObject);
        }

        /// <summary>
        /// ログを流す
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        /// <param name="color"></param>
        private static void LogInternal(string message, object type, Color color)
        {
            if (color == default) color = Color.black;

            StringBuilder st = new();
            if (type != null)
            {
                st.Append($"<color=purple>{type.GetType().Name}</color><color=black> : </color>");
            }

            st.Append($"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{message}\n<s></s>");
            string log = st.ToString();
            _logText.text += log;
            _updateMonitor._logQueue.Enqueue(Time.time);
        }


        /// <summary>
        /// 画面上にログを流す。
        /// </summary>
        /// <param name="message">ログの内容</param>
        /// <param name="color">ログのカスタムカラー</param>
        public static void Log(string message, Color color = default)
        {
            LogInternal(message, null, color);
        }

        /// <summary>
        /// 画面上にログを流す
        /// </summary>
        /// <param name="message">ログの内容</param>
        /// <param name="type">呼び出し元の型</param>
        /// <param name="color">ログのカスタムカラー</param>
        /// <typeparam name="T">呼び出し元の型</typeparam>
        public static void Log<T>(string message, [NotNull] T type, Color color = default)
        {
            LogInternal(message, type, color);
        }

        public static void LogWarning(string message)
        {
            LogInternal($"[Warning] {message}", null, Color.yellow);
        }

        public static void LogWarning<T>(string message, [NotNull] T type)
        {
            LogInternal($"[Warning] {message}", type, Color.yellow);
        }

        public static void LogError(string message)
        {
            LogInternal($"[Error] {message}", null, Color.red);
        }

        public static void LogError<T>(string message, [NotNull] T type)
        {
            LogInternal($"[Error] {message}", type, Color.red);
        }

        public static void SetLogTimer(float time)
        {
            _updateMonitor.LogDeleteTime = time;
        }

        public static void OldLogDelete()
        {
            int index = _logText.text.IndexOf("<s></s>", StringComparison.Ordinal);
            if (index != -1)
            {
                _logText.text = _logText.text.Substring(index + "<s></s>".Length);
            }
        }
    }
}