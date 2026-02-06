using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class PowerupIndicators : MonoBehaviour
    {
        [Header("Boost")]
        [SerializeField] public GameObject m_BoostIndicator;
        [SerializeField] private Image m_BoostImage;

        [Header("Shield")]
        [SerializeField] public GameObject m_ShieldIndicator;
        [SerializeField] private Image m_ShieldImage;

        private float m_LinearVelosity;
        private float m_AngularVelosity;

        private void Start()
        {
            m_LinearVelosity = Player.Instance.ActiveShip.m_MaxLinearVelocity;
            m_AngularVelosity = Player.Instance.ActiveShip.m_MaxAngularVelocity;
        }

        private void Update()
        {
            CooldownBoost();
            CooldownShield();
        }

        private void CooldownBoost() // Индикатор таймера буста
        {
            if (Player.Instance.ActiveShip.IsCooldownBoost == true)
            {
                m_BoostIndicator.SetActive(true);
                m_BoostImage.fillAmount = 1;
                Player.Instance.ActiveShip.IsCooldownBoost = false;
            }

            if (m_BoostImage != null)
            {
                m_BoostImage.fillAmount -= 1 / Player.Instance.ActiveShip.m_CooldownBoost * Time.deltaTime;
                if (m_BoostImage.fillAmount <= 0)
                {
                    m_BoostImage.fillAmount = 1;
                    Player.Instance.ActiveShip.IsCooldownBoost = false;
                    if (Player.Instance.ActiveShip.m_BoostEffect != null) Player.Instance.ActiveShip.m_BoostEffect.SetActive(false);
                    if (m_BoostIndicator != null) m_BoostIndicator.SetActive(false);
                    Player.Instance.ActiveShip.m_MaxLinearVelocity = m_LinearVelosity;
                    Player.Instance.ActiveShip.m_MaxAngularVelocity = m_AngularVelosity;
                }
            }
        }

        private void CooldownShield() // Индикатор таймера щита
        {
            if (Player.Instance.ActiveShip.IsCooldownShield == true)
            {
                m_ShieldIndicator.SetActive(true);
                m_ShieldImage.fillAmount = 1;
                Player.Instance.ActiveShip.IsCooldownShield = false;
            }

            if (m_ShieldImage != null)
            {
                m_ShieldImage.fillAmount -= 1 / Player.Instance.ActiveShip.m_CooldownShield * Time.deltaTime;
                if (m_ShieldImage.fillAmount <= 0)
                {
                    Player.Instance.ActiveShip.m_NoDamage = false;
                    m_ShieldImage.fillAmount = 1;
                    Player.Instance.ActiveShip.IsCooldownShield = false;
                    if (Player.Instance.ActiveShip.m_Shield != null) Player.Instance.ActiveShip.m_Shield.SetActive(false);
                    if (m_ShieldIndicator != null) m_ShieldIndicator.SetActive(false);
                }
            }
        }
    }
}