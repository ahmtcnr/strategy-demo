using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InformationPanel
{
    public class InformationPanelViewer : MonoBehaviour
    {
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text unitName;
        [SerializeField] private Image healthBar;


        public void VisualiseUnitData(InformationPanelData pd)
        {
            unitImage.sprite = pd.UnitSprite;
            unitName.text = pd.UnitText;
            healthBar.fillAmount = pd.VisualizedHealth;
        }
    }
}