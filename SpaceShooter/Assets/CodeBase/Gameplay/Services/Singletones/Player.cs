using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        public static SpaceShip SelectedSpaceShip;

        [SerializeField] private int m_NumLives;

        [SerializeField] private SpaceShip m_PlayerShipPrefab;
        public SpaceShip ActiveShip => m_Ship;

        [SerializeField] private GameObject m_ExplosionPrefab;

        private FollowCamera m_FollowCamera;
        private ShipInputController m_ShipInputController;
        private Transform m_SpawnPoint;

        public FollowCamera FollowCamera => m_FollowCamera;

        public void Construct(FollowCamera followCamera, ShipInputController shipInputController, Transform spawnPoint)
        {
            m_FollowCamera = followCamera;
            m_ShipInputController = shipInputController;
            m_SpawnPoint = spawnPoint;

        }

        private SpaceShip m_Ship;

        private int m_Score;
        private int m_NumKills;

        public int Score => m_Score;
        public int NumKills => m_NumKills;
        public int NumLives => m_NumLives;

        public SpaceShip ShipPrefab
        {
            get
            {
                if (SelectedSpaceShip == null)
                {
                    return m_PlayerShipPrefab;
                }
                else
                {
                    return SelectedSpaceShip;
                }
            }
        }

        private void Start()
        {
            Respawn();
        }

        private void OnShipDeath() // Взрыв и вычитание жизней корабля при смерти и вызов меню GameOver при 0
        {
            Vector3 positionExplosion = m_Ship.transform.position;
            var explosion = Instantiate(m_ExplosionPrefab, positionExplosion, Quaternion.identity);
            Destroy(explosion, 1f);
            
            m_NumLives--;

            if (m_NumLives > 0)
                Invoke("Respawn", 1f);
        }

        private void Respawn() // Респавн игрока при смерти
        {
            var newPlayerShip = Instantiate(ShipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);
            
            m_Ship = newPlayerShip.GetComponent<SpaceShip>();

            m_FollowCamera.SetTarget(m_Ship.transform);
            m_ShipInputController.SetTargetShip(m_Ship);

            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }

        public void AddKill() // Добавление килов
        {
            m_NumKills += 1;
        }

        public void AddScore(int num) // Добавление очков
        {
            m_Score += num;
        }
    }
}