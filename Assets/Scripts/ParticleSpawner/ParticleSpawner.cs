using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ParticleSpawner : MonoBehaviour
{

    private List<GameObject> m_currentParticles;

    #region Global Spawning
    /// <summary>
    /// creates a particle prefab at specified position
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="position">global position to spawn at</param>
    public GameObject SpawnParticle(GameObject particle, Vector3 position)
    {
        GameObject newParticle = Instantiate(particle);
        newParticle.transform.position = position;
        return newParticle;
    }
    /// <summary>
    /// creates a particle prefab at specified position and destroys itself after it has reached its duration
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="position">global position to spawn at</param>
    public GameObject SpawnParticleTemp(GameObject particle, Vector3 position)
    {
        GameObject newParticle = Instantiate(particle);
        newParticle.transform.position = position;
        Destroy(newParticle, newParticle.GetComponent<ParticleSystem>().main.duration);
        return newParticle;
    }
    /// <summary>
    /// creates a particle prefab at specified position and destroys itself after a set amount of time
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="position">global position to spawn at</param>
    /// <param name="time">time before the particle effect is destroyed</param>
    public GameObject SpawnParticleTime(GameObject particle, Vector3 position, float time)
    {
        GameObject newParticle = Instantiate(particle);
        newParticle.transform.position = position;
        Destroy(newParticle, time);
        return newParticle;
    }

    #endregion

    #region Parent Spawning
    /// <summary>
    /// creates a particle prefab parented to a specific object
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    public GameObject SpawnParticle(GameObject particle, Transform parent)
    {
        GameObject newParticle = Instantiate(particle, parent);
        return newParticle;
    }
    /// <summary>
    /// creates a particle prefab parented to a specific object with an offset
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    /// <param name="localPosition">offset for particle object</param>
    public GameObject SpawnParticle(GameObject particle, Transform parent, Vector3 localPosition)
    {
        GameObject newParticle = Instantiate(particle, parent);
        newParticle.transform.localPosition = localPosition;
        return newParticle;
    }
    /// <summary>
    /// creates a particle prefab parented to a specific object and destroys itself after it has reached its duration
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    public GameObject SpawnParticleTemp(GameObject particle, Transform parent)
    {
        GameObject newParticle = Instantiate(particle, parent);
        Destroy(newParticle, newParticle.GetComponent<ParticleSystem>().main.duration);
        return newParticle;
    }
    /// <summary>
    /// creates a particle prefab parented to a specific object with an offset and destroys itself after it has reached its duration
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    /// <param name="localPosition">offset for particle object</param>
    public GameObject SpawnParticleTemp(GameObject particle, Transform parent, Vector3 localPosition)
    {
        GameObject newParticle = Instantiate(particle, parent);
        newParticle.transform.localPosition = localPosition;
        Destroy(newParticle, newParticle.GetComponent<ParticleSystem>().main.duration);
        return newParticle;
    }
    /// <summary>
    /// creates a particle prefab parented to a specific object and destroys itself after a set amount of time
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    /// <param name="time">time before object destroys itself</param>
    public GameObject SpawnParticleTime(GameObject particle, Transform parent, float time)
    {
        GameObject newParticle = Instantiate(particle,parent);
        Destroy(newParticle, time);
        return newParticle;
    }
    /// <summary>
    /// creates a particle prefab parented to a specific object with an offset and destroys itself after a set amount of time
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    /// <param name="localPosition">offset for particle object</param>
    /// <param name="time">time before object destroys itself</param>
    public GameObject SpawnParticleTime(GameObject particle, Transform parent, Vector3 localPosition, float time)
    {
        GameObject newParticle = Instantiate(particle, parent);
        newParticle.transform.localPosition = localPosition;
        Destroy(newParticle, time);
        return newParticle;
    }

    #endregion

}
