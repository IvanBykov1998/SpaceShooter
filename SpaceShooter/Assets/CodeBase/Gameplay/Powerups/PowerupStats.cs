using UnityEngine;

namespace SpaceShooter
{
    public class PowerupStats : Powerup
    {
        public enum EffectType
        {
            AddAmmo,
            AddEnergy,
            AddShield,
            AddBoost,
            AddRepair
        }

        [SerializeField] private EffectType m_EffectType;

        [SerializeField] private float m_Value;
        [SerializeField] private float m_Duration;

        protected override void OnPickedUp(SpaceShip ship)
        {
            if (m_EffectType == EffectType.AddEnergy)
            {
                ship.AddEnergy((int)m_Value);
            }

            if (m_EffectType == EffectType.AddAmmo)
            {
                ship.AddAmmo((int)m_Value);
            }

            if (m_EffectType == EffectType.AddShield)
            {
                ship.AddShield(m_Duration);
            }
            
            if (m_EffectType == EffectType.AddBoost)
            {
                ship.AddBoost(m_Duration);
            }

            if (m_EffectType == EffectType.AddRepair)
            {
                ship.AddRepair((int)m_Value);
            }
        }
    }
}