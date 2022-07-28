using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InformationPanel
{
    public class InformationPanelView : MonoBehaviour
    {
        [Header("Units")] [SerializeField] private GameObject unitParent;
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text unitName;

        [Header("Producer")] [SerializeField] private GameObject produceParent;
        [SerializeField] private Image imageToProduce;
        [SerializeField] private TMP_Text produceText;

        private void Awake()
        {
            ClearPanel();
        }

        public void VisualiseUnitData(InformationPanelData pd)
        {
            unitParent.SetActive(true);
            unitImage.sprite = pd.UnitSprite;
            unitName.text = pd.UnitText;

            if (pd.dataType == typeof(ProducerData)) //TODO: Find Another way
            {
                produceParent.SetActive(true);
                imageToProduce.sprite = pd.ProductionButtonImage;
                produceText.text = pd.ProductionText;
            }
            else
            {
                produceParent.SetActive(false);
            }
        }

        public void ClearPanel()
        {
            unitParent.SetActive(false);
            produceParent.SetActive(false);
        }
    }
}