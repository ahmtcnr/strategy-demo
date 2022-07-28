using Units.Base;
using UnityEngine;

namespace SOScripts
{
    [CreateAssetMenu(menuName = "BuildingData", order = 0)]
    public class ProducerData : BaseUnitData
    {
        public BaseForces forcesToProduce;
    }
}