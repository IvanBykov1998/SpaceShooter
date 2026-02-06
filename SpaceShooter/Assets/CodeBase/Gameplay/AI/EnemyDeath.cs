using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class EnemyDead : MonoBehaviour
    {
        [SerializeField] private GameObject m_EnemyShip;
        [SerializeField] private GameObject m_ExplosionPrefab;

        [SerializeField] private GameObject[] m_PowerupPrefab;

        private void Start()
        {
            m_EnemyShip.GetComponent<Destructible>().EventOnDeath.AddListener(OnShipDeath);
        }

        private void OnShipDeath()
        {
            Vector3 positionExplosion = m_EnemyShip.transform.position;
            var explosion = Instantiate(m_ExplosionPrefab, positionExplosion, Quaternion.identity);

            int index = Random.Range(0, m_PowerupPrefab.Length);
            if (index < m_PowerupPrefab.Length && m_PowerupPrefab[index] != null)
            {
                GameObject powerup = Instantiate(m_PowerupPrefab[index], explosion.transform.position, Quaternion.identity);
            }
            
            Destroy(explosion, 1f);
        }
    }
}