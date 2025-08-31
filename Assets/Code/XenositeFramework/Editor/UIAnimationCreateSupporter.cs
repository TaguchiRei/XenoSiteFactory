using System;
using System.Reflection;
using TMPro;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace XenositeFramework.Editor
{
    public class UIAnimationCreateSupporter : EditorWindow
    {
        GameObject targetObject;
        AnimatorController animatorController;
        AnimationClip animationClip;

        [MenuItem("Window/XenositeFramework/UIAnimationCreateSupporter")]
        public static void ShowWindow()
        {
            GetWindow<UIAnimationCreateSupporter>("UIAnimationCreateSupporter");
        }

        private void OnGUI()
        {
            GUILayout.Label("UIAnimationCreateSupporter", EditorStyles.boldLabel);

            targetObject = (GameObject)EditorGUILayout.ObjectField("Target GameObject", targetObject, typeof(GameObject), true);
            animatorController = (AnimatorController)EditorGUILayout.ObjectField("Animator Controller", animatorController, typeof(AnimatorController), false);
            animationClip = (AnimationClip)EditorGUILayout.ObjectField("Animation Clip", animationClip, typeof(AnimationClip), false);

            if (GUILayout.Button("Set Animation Keys"))
            {
                if (targetObject == null || animatorController == null || animationClip == null)
                {
                    Debug.LogError("Target, AnimatorController or AnimationClip is null!");
                    return;
                }

                SetKeys(targetObject, animatorController, animationClip);
                Debug.Log("Keys set successfully!");
            }
        }

        private void SetKeys(GameObject target, AnimatorController controller, AnimationClip clip)
        {
            foreach (Transform child in target.transform)
            {
                string path = AnimationUtility.CalculateTransformPath(child, target.transform);

                // 1. Transform or RectTransform
                AddVector3Curve(clip, path, typeof(Transform), "m_LocalPosition", child.localPosition);
                AddQuaternionCurve(clip, path, typeof(Transform), "m_LocalRotation", child.localRotation);
                AddVector3Curve(clip, path, typeof(Transform), "m_LocalScale", child.localScale);

                // 2. 全コンポーネントのenable
                Component[] components = child.GetComponents<Component>();
                foreach (var comp in components)
                {
                    if (comp == null) continue;
                    var enabledProp = comp.GetType().GetProperty("enabled", BindingFlags.Public | BindingFlags.Instance);
                    if (enabledProp != null)
                    {
                        bool value = (bool)enabledProp.GetValue(comp);
                        AddBoolCurve(clip, path, comp.GetType(), "m_Enabled", value);
                    }
                }

                // 3. ImageコンポーネントのColor
                Image img = child.GetComponent<Image>();
                if (img != null)
                {
                    AddColorCurve(clip, path, typeof(Image), "m_Color", img.color);
                }

                // 4. TextMeshProUGUIのVertexColor、FontSize
                TextMeshProUGUI tmp = child.GetComponent<TextMeshProUGUI>();
                if (tmp != null)
                {
                    AddColorCurve(clip, path, typeof(TextMeshProUGUI), "m_fontColor", tmp.color);
                    AddFloatCurve(clip, path, typeof(TextMeshProUGUI), "m_fontSize", tmp.fontSize);
                }

                // 5. SetActive
                AddBoolCurve(clip, path, typeof(GameObject), "m_IsActive", child.gameObject.activeSelf);
            }
        }

        // -------------------- ヘルパー関数 --------------------
        static void AddVector3Curve(AnimationClip clip, string path, Type type, string property, Vector3 value)
        {
            AddFloatCurve(clip, path, type, property + ".x", value.x);
            AddFloatCurve(clip, path, type, property + ".y", value.y);
            AddFloatCurve(clip, path, type, property + ".z", value.z);
        }

        static void AddQuaternionCurve(AnimationClip clip, string path, Type type, string property, Quaternion value)
        {
            AddFloatCurve(clip, path, type, property + ".x", value.x);
            AddFloatCurve(clip, path, type, property + ".y", value.y);
            AddFloatCurve(clip, path, type, property + ".z", value.z);
            AddFloatCurve(clip, path, type, property + ".w", value.w);
        }

        static void AddColorCurve(AnimationClip clip, string path, Type type, string property, Color color)
        {
            AddFloatCurve(clip, path, type, property + ".r", color.r);
            AddFloatCurve(clip, path, type, property + ".g", color.g);
            AddFloatCurve(clip, path, type, property + ".b", color.b);
            AddFloatCurve(clip, path, type, property + ".a", color.a);
        }

        static void AddBoolCurve(AnimationClip clip, string path, Type type, string property, bool value)
        {
            AddFloatCurve(clip, path, type, property, value ? 1f : 0f);
        }

        static void AddFloatCurve(AnimationClip clip, string path, Type type, string property, float value)
        {
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0f, value);
            curve.AddKey(30f / clip.frameRate, value); // 30フレーム目
            clip.SetCurve(path, type, property, curve);
        }
    }
}
