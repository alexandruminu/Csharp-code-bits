using UnityEngine;

public class PlayParticlesOnImpact : MonoBehaviour
{
    public ParticleVelocityPair[] particleVelocityPairs;

    private void Start()
    {
        GetComponent<CollisionManager>().onCollision.AddListener(PlayCollisionParticle);
        for (int i = 0; i < particleVelocityPairs.Length; i++)
        {
            particleVelocityPairs[i].particlesTransform = particleVelocityPairs[i].particles.transform;
        }
    }

    void PlayCollisionParticle(Collision collision)
    {
        float magnitude = collision.relativeVelocity.magnitude;
        for (int i = 0; i < particleVelocityPairs.Length; i++)
        {
            if (magnitude >= particleVelocityPairs[i].velocityMin && magnitude < particleVelocityPairs[i].velocityMax)
            {
                particleVelocityPairs[i].particlesTransform.position = collision.contacts[0].point;
                particleVelocityPairs[i].particles.Play();
            }
        }
    }

    [System.Serializable]
    public class ParticleVelocityPair
    {
        public float velocityMin = 1f;
        public float velocityMax = 1f;
        public ParticleSystem particles;
        [HideInInspector]public Transform particlesTransform;
    }
}
