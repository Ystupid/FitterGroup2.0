using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace UnityEngine.UI.FitterGroup
{
    public static class FitterGroupEditor
    {
        [MenuItem("GameObject/UI/FitterGroup", false, 1)]
        public static void CraeteFitterGroup()
        {
            var canvas = Object.FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                canvas = CreateUIObject("Canvas").gameObject.AddComponent<Canvas>();
                canvas.gameObject.AddComponent<CanvasScaler>();
                canvas.gameObject.AddComponent<GraphicRaycaster>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }

            var fitterGroupObject = CreateUIObject("FitterGroup",canvas.transform);
            fitterGroupObject.sizeDelta = new Vector2(800,800);

            var view = CreateView("View",fitterGroupObject);
            var content = CreateContent("Content", view);

            var scrollRect = fitterGroupObject.gameObject.AddComponent<ScrollRect>();
            scrollRect.viewport = view;
            scrollRect.content = content;

            //fitterGroupObject.gameObject.AddComponent<FitterGroup<TItem>>();
        }

        private static GameObject CreateObejct(string name,Transform parent = null)
        {
            var gameObject = new GameObject(name);
            gameObject.transform.SetParent(parent);
            return gameObject;
        }

        private static RectTransform CreateUIObject(string name,Transform parent = null)
        {
            var gameObject = CreateObejct(name,parent);
            gameObject.layer =LayerMask.NameToLayer("UI");
            var rectTransform = gameObject.AddComponent<RectTransform>();
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.rotation = Quaternion.identity;
            return rectTransform;
        }

        private static RectTransform CreateView(string name,Transform parent = null)
        {
            var view = CreateUIObject(name, parent);
            view.anchorMin = Vector2.zero;
            view.anchorMax = Vector2.one;
            view.sizeDelta = Vector2.zero;
            view.gameObject.AddComponent<RectMask2D>();
            view.gameObject.AddComponent<Image>().color = new Color(0.2196079f, 0.2196079f, 0.2196079f, 1);
            return view;
        }

        private static RectTransform CreateContent(string name,Transform parent = null)
        {
            var content = CreateUIObject(name, parent);
            return content;
        }
    }
}