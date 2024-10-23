using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ILOVEYOU.UI
{

    public static class ColorPref
    {
        public static Color Get(string key)
        {
            float R = PlayerPrefs.GetFloat($"{key} R");
            float G = PlayerPrefs.GetFloat($"{key} G");
            float B = PlayerPrefs.GetFloat($"{key} B");
            float A = PlayerPrefs.GetFloat($"{key} A");

            return new Color(R,G,B,A);
        }
        public static void Set(string key, Color value)
        {
            PlayerPrefs.SetFloat($"{key} R", value.r);
            PlayerPrefs.SetFloat($"{key} G", value.g);
            PlayerPrefs.SetFloat($"{key} B", value.b);
            PlayerPrefs.SetFloat($"{key} A", value.a);
            PlayerPrefs.Save();
        }
    }
}
