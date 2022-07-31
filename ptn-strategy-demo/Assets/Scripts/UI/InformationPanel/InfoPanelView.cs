using System;
using System.Collections;
using System.Collections.Generic;
using SOScripts;
using TMPro;
using Units.Base;
using UnityEngine;
using UnityEngine.UI;

namespace InformationPanel
{
    public class InfoPanelView : MonoBehaviour
    {
        [Header("Units")] [SerializeField] private GameObject unitParent;
        [SerializeField] private Image unitImage;
        [SerializeField] private TMP_Text unitName;

        [Header("Producer")] [SerializeField] private GameObject produceParent;
        [SerializeField] private Button unitProduceButton;
        [SerializeField] private TMP_Text produceText;
        [SerializeField] private Image unitProduceImage;

        private InfoPanelModel _infoPanelModel;

        private void Awake()
        {
            _infoPanelModel = new InfoPanelModel();
            InfoPanelController infoPanelController = new InfoPanelController(_infoPanelModel);
            ClearPanel();
        }

        private void OnEnable()
        {
            _infoPanelModel.OnBaseUnitChanged += VisualiseUnitData;
        }

        private void OnDisable()
        {
            _infoPanelModel.OnBaseUnitChanged -= VisualiseUnitData;
        }

        private void VisualiseUnitData(BaseUnit baseUnit)
        {
            if (baseUnit == null)
            {
                ClearPanel();
                return;
            }

            unitParent.SetActive(true);
            unitImage.sprite = baseUnit.baseUnitData.UnitSprite;
            unitName.text = baseUnit.baseUnitData.UnitName;
            if (baseUnit.TryGetComponent(out BaseProducer baseProducer)) 
            {
                produceParent.SetActive(true);

                unitProduceImage.sprite = baseProducer.forcesToProduce.baseUnitData.UnitSprite;
                produceText.text = baseProducer.forcesToProduce.baseUnitData.UnitName;
                unitProduceButton.onClick.RemoveAllListeners();
                unitProduceButton.onClick.AddListener(baseProducer.Produce);
            }
            else
            {
                produceParent.SetActive(false);
            }
        }

        private void ClearPanel()
        {
            unitParent.SetActive(false);
            produceParent.SetActive(false);
        }
    }
}