using ILOVEYOU.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ILOVEYOU.EditorScript
{
    public class ColorPrefEditor : EditorWindow
    {
        public List<string> keys = new();
        VisualElement m_root;
        List<ColorField> m_colorFields = new();
        [MenuItem("Tools/ColorPrefs Editor", false, 1)]
        static void ShowWindow()
        {
            ColorPrefEditor wnd = GetWindow<ColorPrefEditor>();
            wnd.titleContent = new GUIContent("Color Prefs Editor");
        }
        public void CreateGUI()
        {
            //initialize
            m_root = rootVisualElement;

            m_root.Clear();

            m_colorFields = new();
            keys = new();

            keys.Add("Important Color");
            keys.Add("Buff color");
            keys.Add("Debuff color");
            keys.Add("Hazard color");
            keys.Add("Summon color");

            foreach(var color in keys)
            {
                Label name = new(color);
                m_root.Add(name);
                ColorField cf = new();
                cf.value = ColorPref.Get(color);
                m_colorFields.Add(cf);
                m_root.Add(cf);
            }
            //Func<VisualElement> makeItem = () => new Label();
            //Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = keys[i];
            ///const int height = 16;

            //ListView field = new ListView(keys, height, makeItem, bindItem);
/*            List<TextField> keylist = new();
            foreach (var key in keys)
            {
                TextField t = new TextField();
                t.label = key;
                keylist.Add(t);
            }*/
            //m_root.Add(field);

           /* {
                ColorField cf = new ColorField();
                cf.value = ColorPref.Get($"{key}");
                m_root.Add(cf);

            }*/

        }
        private void OnGUI()
        {
        }
        private void Update()
        {
            for(int i = 0; i < keys.Count; i++)
            {
                ColorPref.Set(keys[i], m_colorFields[i].value);
            }
            //ColorPref.Set("Important Color", m_colorField.value);
        }
    }
}