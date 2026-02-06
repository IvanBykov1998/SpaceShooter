using UnityEngine;
using Common;

namespace SpaceShooter
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        [SerializeField] private Sprite m_PreviewImage;

        /// <summary>
        /// ����� ��� �������������� ��������� � ������.
        /// </summary>
        [Header("Space ship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// ��������� ������ ����.
        /// </summary>
        [SerializeField] private float m_Thrust;

        /// <summary>
        /// ��������� ����.
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// ������������ �������� ��������.
        /// </summary>
        [SerializeField] public float m_MaxLinearVelocity;

        /// <summary>
        /// ������������ ������������ ��������. � ��������/���.
        /// </summary>
        [SerializeField] public float m_MaxAngularVelocity;

        /// <summary>
        /// ����������� ������ �� �����.
        /// </summary>
        private Rigidbody2D m_Rigid;

        public float MaxLinearVelocity => m_MaxLinearVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;
        public Sprite PrewiewImage => m_PreviewImage;

        #region Public API
        /// <summary>
        /// ���������� �������� �����. -1.0 �� +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// ���������� ������������ �����. -1.0 �� +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        /// <summary>
        /// ��������� ����� ��� ��������� PowerupShied
        /// </summary>
        public override void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            // ���������, ������� �� ���
            if (m_NoDamage == true)
            {
                // ��� �������, ���� �� �����������
                return;
            }

            base.ApplyDamage(damage);
        }
        #endregion

        #region Unity Event
        protected override void Start()
        {
            base.Start();

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;
            m_Rigid.inertia = 1;

            m_MaxRepair = HitPoints;

            InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();
            UpdateEnergyRegen();
        }
        #endregion

        /// <summary>
        /// ����� ���������� ��� ������� ��� ��������.
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.linearVelocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        [SerializeField] private Turret[] m_Turret;

        public void Fire(TurretMode mode)
        {
            for (int i = 0; i < m_Turret.Length; ++i)
            {
                if (m_Turret[i].Mode == mode)
                {
                    m_Turret[i].Fire();
                }
            }
        }

        [Header("Weapon")]
        [SerializeField] private int m_MaxEnergy; // ������������ �������
        [HideInInspector] private float m_PrimaryEnergy; // ������� �������� �������
        [SerializeField] private int m_EnergyRegenPerSecond; // ����������� ������� � �������
        public int MaxEnergy => m_MaxEnergy;
        public float PrimaryEnergy => m_PrimaryEnergy;

        [SerializeField] private int m_MaxAmmo; // ������������ ����� �����������
        [HideInInspector] private int m_SecondaryAmmo; // ������� �������� �����������
        public int SecondaryAmmo => m_SecondaryAmmo;

        [SerializeField] public GameObject m_BoostEffect;
        [SerializeField] public GameObject m_Shield;

        [HideInInspector] public bool IsCooldownBoost;
        [HideInInspector] public bool IsCooldownShield;
        [HideInInspector] public bool m_NoDamage;
        [HideInInspector] public float m_CooldownBoost;
        [HideInInspector] public float m_CooldownShield;

        [HideInInspector] private int m_MaxRepair;

        public void AddEnergy(int e)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + e, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }

        public void AddShield(float duration)
        {
            m_Shield.SetActive(true);
            m_NoDamage = true;
            IsCooldownShield = true;
            m_CooldownShield = duration;
        }

        public void AddBoost(float duration)
        {
            m_BoostEffect.SetActive(true);
            IsCooldownBoost = true;
            m_CooldownBoost = duration;

            m_MaxLinearVelocity *= 1.5f;
            m_MaxAngularVelocity *= 1.5f;
        }

        public void AddRepair(int repair)
        {
            m_CurrentHitPoints = Mathf.Clamp(m_CurrentHitPoints + repair, 0, m_MaxRepair);
        }

        private void InitOffensive() // ��������� ��������
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = 0;
        }

        private void UpdateEnergyRegen() // ����������� ������� � ���
        {
            m_PrimaryEnergy += (float)m_EnergyRegenPerSecond * Time.deltaTime;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        public bool DrawEnergy(int count) // ����������� ������� ��� ��������
        {
            if (count == 0)
            {
                return true;
            }

            if (m_PrimaryEnergy >= count)
            {
                m_PrimaryEnergy -= count;
                return true;
            }

            return false;
        }

        public bool DrawAmmo(int count) // ���������� ����������� ��� ��������
        {
            if (count == 0)
            {
                return true;
            }

            if (m_SecondaryAmmo >= count)
            {
                m_SecondaryAmmo -= count;
                return true;
            }

            return false;
        }

        public void AssignWeapon(TurretProperties props) // ��������� ������
        {
            for (int i = 0; i < m_Turret.Length; i++)
            {
                m_Turret[i].AssignLoadout(props);
            }
        }
    }
}