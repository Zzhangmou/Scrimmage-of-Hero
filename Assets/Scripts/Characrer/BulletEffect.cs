using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ns
{
    /// <summary>
    /// 子弹效果
    /// </summary>
    public class BulletEffect : MonoBehaviour
    {
        /// <summary>
        /// 击中特效
        /// </summary>
        public GameObject impactParticle; // Effect spawned when projectile hits a collider
        /// <summary>
        /// 子弹效果
        /// </summary>
        public GameObject projectileParticle; // Effect attached to the gameobject as child
        /// <summary>
        /// 开火特效
        /// </summary>
        public GameObject muzzleParticle; // Effect instantly spawned when gameobject is spawned
        [Header("Adjust if not using Sphere Collider")]
        public float colliderRadius = 1f;
        [Range(0f, 1f)] // This is an offset that moves the impact effect slightly away from the point of impact to reduce clipping of the impact effect
        public float collideOffset = 0.15f;

        public void Init()
        {
            projectileParticle = GameObjectPool.Instance.CreateObject("projectileParticle", projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.parent = transform;
            if (muzzleParticle)
            {
                //释放回收
                muzzleParticle = GameObjectPool.Instance.CreateObject("muzzleParticle", muzzleParticle, transform.position, transform.rotation) as GameObject;
                GameObjectPool.Instance.CollectObject(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
            }
        }

        void FixedUpdate()
        {
            if (GetComponent<Rigidbody>().velocity.magnitude != 0)
            {
                transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity); // Sets rotation to look at direction of movement
            }

            RaycastHit hit;

            float radius; // Sets the radius of the collision detection
            if (transform.GetComponent<SphereCollider>())
                radius = transform.GetComponent<SphereCollider>().radius;
            else
                radius = colliderRadius;

            Vector3 direction = transform.GetComponent<Rigidbody>().velocity; // Gets the direction of the projectile, used for collision detection
            if (transform.GetComponent<Rigidbody>().useGravity)
                direction += Physics.gravity * Time.deltaTime; // Accounts for gravity if enabled
            direction = direction.normalized;

            float detectionDistance = transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime; // Distance of collision detection for this frame

            //当球体扫描与任何碰撞体交叠时为 true，否则为 false
            if (Physics.SphereCast(transform.position, radius, direction, out hit, detectionDistance)) // Checks if collision will happen
            {
                transform.position = hit.point + (hit.normal * collideOffset); // Move projectile to point of collision

                //击中特效
                GameObject impactP = GameObjectPool.Instance.CreateObject("impactParticle", impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; // Spawns impact effect

                ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                                     //Component at [0] is that of the parent i.e. this object (if there is any)
                for (int i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
                {
                    ParticleSystem trail = trails[i];
                    if (trail.gameObject.name.Contains("Trail"))
                    {
                        //trail.transform.SetParent(null); // Detaches the trail from the projectile
                        GameObjectPool.Instance.CollectObject(trail.gameObject, 2f); // Removes the trail after seconds
                    }
                }

                GameObjectPool.Instance.CollectObject(projectileParticle, 3f); // Removes particle effect after delay
                GameObjectPool.Instance.CollectObject(impactP, 3.5f); // Removes impact effect after delay
                GameObjectPool.Instance.CollectObject(gameObject); // Removes the projectile
            }
        }
    }
}