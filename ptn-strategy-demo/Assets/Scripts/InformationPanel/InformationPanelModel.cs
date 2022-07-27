using SOScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace InformationPanel
{
    public class InformationPanelModel
    {
        InformationPanelData panelData;

        public InformationPanelData ProcessInformationPanelData(BaseUnitData bd)
        {
            panelData.UnitSprite = bd.unitSprite;
            panelData.UnitText = bd.unitName;
            return panelData;
        }
    }


    public struct InformationPanelData
    {
        public Sprite UnitSprite;
        public string UnitText;
    }
}