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

        private InfoPanelModel infoPanelModel;

        private void Awake()
        {
            infoPanelModel = new InfoPanelModel();

            InfoPanelController infoPanelController = new InfoPanelController(infoPanelModel);


            ClearPanel();
        }

        private void OnEnable()
        {
            infoPanelModel.OnBaseUnitChanged += VisualiseUnitData;
        }

        private void OnDisable()
        {
            infoPanelModel.OnBaseUnitChanged -= VisualiseUnitData;
        }

        public void VisualiseUnitData(BaseUnit baseUnit)
        {
            if (baseUnit == null)
            {
                ClearPanel();
                return;
            }

            unitParent.SetActive(true);
            unitImage.sprite = baseUnit.baseUnitData.UnitSprite;
            unitName.text = baseUnit.baseUnitData.UnitName;
            // Debug.Log(baseUnit.TryGetComponent(out BaseProducer bf));
            if (baseUnit.TryGetComponent(out BaseProducer baseProducer)) //TODO: Find Another way
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

        public void ClearPanel()
        {
            unitParent.SetActive(false);
            produceParent.SetActive(false);
        }
    }
}