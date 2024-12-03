using ILOVEYOU.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColourPalettePicker : MonoBehaviour
{
    [Serializable]
    private class ColourPalette
    {
        [SerializeField] public string m_paletteName;
        [SerializeField] public Color m_importantColour = Color.white;
        [SerializeField] public Color m_buffColour = Color.white;
        [SerializeField] public Color m_debuffColour = Color.white;
        [SerializeField] public Color m_hazardColour = Color.white;
        [SerializeField] public Color m_summonColour = Color.white;
        [SerializeField] public Color m_backgroundColour = Color.white;
    }



    [SerializeField] private ColourPalette[] m_palletes;

    [SerializeField] private int player = 0;

    private int m_currentSelected = 0;

    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private Slider m_selection;
    [SerializeField] private Image[] m_imageColours;

    private void LoadColour()
    {
        m_imageColours[0].color = m_palletes[m_currentSelected].m_importantColour;
        m_imageColours[1].color = m_palletes[m_currentSelected].m_buffColour;
        m_imageColours[2].color = m_palletes[m_currentSelected].m_debuffColour;
        m_imageColours[3].color = m_palletes[m_currentSelected].m_hazardColour;
        m_imageColours[4].color = m_palletes[m_currentSelected].m_summonColour;

    }

    private void SetColour(string key, Color color)
    {
        ColorPref.Set(key + player, color);
    }


    public void Selection(float value)
    {
        m_currentSelected = (int)value;

        m_text.text = "Current Palette:\n" + m_palletes[m_currentSelected].m_paletteName;

        PlayerPrefs.SetInt("Palette" + player, m_currentSelected);

        LoadColour();

        //m_importantColour;
        SetColour("Important Color", m_palletes[m_currentSelected].m_importantColour);
        //m_buffColour;
        SetColour("Buff color", m_palletes[m_currentSelected].m_buffColour);
        //m_debuffColour;
        SetColour("Debuff color", m_palletes[m_currentSelected].m_debuffColour);
        //m_hazardColour;
        SetColour("Hazard color", m_palletes[m_currentSelected].m_hazardColour);
        //m_summonColour;
        SetColour("Summon color", m_palletes[m_currentSelected].m_summonColour);
        //m_backgroundColour;
        SetColour("Background Color", m_palletes[m_currentSelected].m_backgroundColour);
    }

    private void Awake()
    {
        m_currentSelected = PlayerPrefs.GetInt("Palette" + player);

        m_text.text = "Current Palette:\n" + m_palletes[m_currentSelected].m_paletteName;

        m_selection.maxValue = m_palletes.Length - 1;
        m_selection.value = m_currentSelected;

        LoadColour();
    }
}
