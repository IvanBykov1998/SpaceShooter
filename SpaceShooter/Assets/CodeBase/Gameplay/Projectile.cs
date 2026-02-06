using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class Projectile : Entity
    {
        [SerializeField] private float m_Velocity; // Скорость снаряда
        public float Velosity => m_Velocity;
        [SerializeField] private float m_LifeTime; // Время через которое снаряд уничтожится
        [SerializeField] private int m_Damage; // Урон снаряда
        [SerializeField] private float m_ExplosionRadius; // Радиус взрыва снаряда
        [SerializeField] private GameObject m_ExplosionPrefab; // Префаб взрыва

        [SerializeField] private bool m_InHoming = false; // Является ли снаряд самонаводящимся
        [SerializeField] private float m_HomingDist; // Дистанция для срабатывания самонаведения снаряда
        private Destructible m_SelectedTarget; // Цель для самонаводящейся ракеты

        private float m_Timer;

        private void Update()
        {
            float stepLength = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLength;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLength);

            m_SelectedTarget = FindNearestEnemyTarget(); // Цель в которую полетит снаряд

            // Самонаведение ракеты
            if (m_SelectedTarget != null && m_InHoming == true) 
            {
                var signedAngle = Vector2.SignedAngle(transform.up, (m_SelectedTarget.transform.position - transform.position));

                if (Mathf.Abs(signedAngle) >= 1e-3f)
                {
                    var angles = transform.eulerAngles;
                    angles.z += signedAngle * Time.deltaTime * 5.0f;
                    transform.eulerAngles = angles;
                }
            }

            if (hit)
            {
                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_LifeTime)
            {
                OnProjectileLifeEnd(hit.collider, hit.point);
            }

            transform.position += new Vector3(step.x, step.y, 0);
        }

        private void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            Vector3 positionExplosion = transform.position;
            var explosion = Instantiate(m_ExplosionPrefab, positionExplosion, Quaternion.identity);
            Destroy(explosion, 1f);

            // Нанесение урона всем объектам в радиусе взрыва
            Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(positionExplosion, m_ExplosionRadius);

            foreach (Collider2D obj in objectsInRange)
            {
                Destructible dest = obj.transform.root.GetComponent<Destructible>();

                if (dest != null && dest != m_Parent && dest.TeamId != m_Parent.TeamId)
                {
                    dest.ApplyDamage(m_Damage);

                    if (dest.HitPoints <= 0)
                    {
                        if (m_Parent == Player.Instance.ActiveShip)
                        {
                            Player.Instance.AddScore(dest.ScoreValue); // Добавление очков при уничтожении объекта

                            if (dest is SpaceShip)
                            {
                                Player.Instance.AddKill(); // Добавление кила при уничтожении вражеского корабля
                            }
                        }
                    }
                }
            }
            Destroy(gameObject);
        }

        private Destructible m_Parent;

        public void SetParentShooter(Destructible purent)
        {
            m_Parent = purent;
        }
    
        private Destructible FindNearestEnemyTarget() // Поиск ближайшей цели для самонаводящейся ракеты
        {
            Destructible potentialTarget = null; // Потенциальная цель, вначале равна null - нет цели

            foreach (var v in Destructible.AllDestructibles) // Цикл перебирающий объекты 
            {
                if (v.GetComponent<SpaceShip>() == m_Parent) continue; // Если объект наш SpaceShip, переключаемся на сдедующий объект

                if (v.TeamId == Destructible.TeamIdNeutral) continue; // Если объект нейтральный, переключаемся на следующий объект

                if (v.TeamId == m_Parent.TeamId) continue; // Если объект наш союзник, переключаемся на следующий объект

                float dist = Vector2.Distance(transform.position, v.transform.position); // Дистанция от снаряда до цели

                if (dist <= m_HomingDist)
                {
                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }
    }
}