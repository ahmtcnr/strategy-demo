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
            var normalizedHealth = bd.CurrentHealth / bd.MaxHealth; //TODO: Prevent divided by zero


            panelData.UnitSprite = bd.unitSprite;
            panelData.UnitText = bd.unitName;
            panelData.VisualizedHealth = normalizedHealth;
            return panelData;
        }
    }


    public struct InformationPanelData
    {
        public Sprite UnitSprite;
        public string UnitText;
        public float VisualizedHealth;
    }
}