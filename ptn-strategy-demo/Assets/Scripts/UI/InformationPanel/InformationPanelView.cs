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
        [Header("Units")]
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text unitName;
        
        [Header("Producer")]
        [SerializeField] private Image imageToProduce;
        [SerializeField] private TMP_Text produceText;
        [SerializeField] private Transform produceParent;


        public void VisualiseUnitData(InformationPanelData pd)
        {
            unitImage.sprite = pd.UnitSprite;
            unitName.text = pd.UnitText;

            if (pd.dataType == typeof(ProducerData))//TODO: Find Another way
            {
                produceParent.gameObject.SetActive(true);
                imageToProduce.sprite = pd.ProductionButtonImage;
                produceText.text = pd.ProductionText;

            }
            else
            {
                produceParent.gameObject.SetActive(false);
            }



        }
        
        
        
    }
}