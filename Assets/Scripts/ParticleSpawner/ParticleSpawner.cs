using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class ParticleSpawner : MonoBehaviour
{

    private List<GameObject> m_currentParticles;

    #region Global Spawning
    /// <summary>
    /// creates a particle prefab at specified position
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="position">global position to spawn at</param>
    public static GameObject[] SpawnParticles(GameObject[] particles, Vector3 position)
    {
        List<GameObject> objs = new();
        foreach (var particle in particles)
        {
            GameObject newParticle = Instantiate(particle);
            newParticle.transform.position = position;
            objs.Add(newParticle);
        }
        return objs.ToArray();
    }
    /// <summary>
    /// creates a particle prefab at specified position and destroys itself after a set amount of time
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="position">global position to spawn at</param>
    /// <param name="time">time before the particle effect is destroyed</param>
    public static GameObject[] SpawnParticlesTime(GameObject[] particles, Vector3 position, float time = 0)
    {
        List<GameObject> objs = new();
        foreach (var particle in particles)
        {
            GameObject newParticle = Instantiate(particle);
            newParticle.transform.position = position;
            Destroy(newParticle, time > 0 ? time : newParticle.GetComponent<ParticleSystem>().main.duration);
            objs.Add(newParticle);
        }
        return objs.ToArray();
    }

    #endregion

    #region Parent Spawning
    /// <summary>
    /// creates a particle prefab parented to a specific object
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    public static GameObject[] SpawnParticles(GameObject[] particles, Transform parent)
    {
        List<GameObject> objs = new();
        foreach (var particle in particles)
        {
            GameObject newParticle = Instantiate(particle, parent);
            objs.Add(newParticle);
        }
        return objs.ToArray();
    }
    /// <summary>
    /// creates a particle prefab parented to a specific object with an offset
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    /// <param name="localPosition">offset for particle object</param>
    public static GameObject[] SpawnParticles(GameObject[] particles, Transform parent, Vector3 localPosition)
    {
        List<GameObject> objs = new();
        foreach (var particle in particles)
        {
            GameObject newParticle = Instantiate(particle, parent);
            newParticle.transform.localPosition = localPosition;
            objs.Add(newParticle);
        }
            return objs.ToArray();
    }
    /// <summary>
    /// creates a particle prefab parented to a specific object and destroys itself after a set amount of time
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    /// <param name="time">time before object destroys itself</param>
    public static GameObject[] SpawnParticlesTime(GameObject[] particles, Transform parent, float time = 0)
    {
        List<GameObject> objs = new();
        foreach (var particle in particles)
        {
            GameObject newParticle = Instantiate(particle, parent);
            Destroy(newParticle, time > 0 ? time : newParticle.GetComponent<ParticleSystem>().main.duration);
            objs.Add(newParticle);
        }
        return objs.ToArray();
    }
    /// <summary>
    /// creates a particle prefab parented to a specific object with an offset and destroys itself after a set amount of time
    /// </summary>
    /// <param name="particle">particle prefab to spawn</param>
    /// <param name="parent">parent object for the particle to attach to</param>
    /// <param name="localPosition">offset for particle object</param>
    /// <param name="time">time before object destroys itself</param>
    public static GameObject[] SpawnParticlesTime(GameObject[] particles, Transform parent, Vector3 localPosition, float time = 0)
    {
        List<GameObject> objs = new();
        foreach (var particle in particles)
        {
            GameObject newParticle = Instantiate(particle, parent);
            newParticle.transform.localPosition = localPosition;
            Destroy(newParticle, time > 0 ? time : newParticle.GetComponent<ParticleSystem>().main.duration);
            objs.Add(newParticle);
        }
        return objs.ToArray();
    }

    #endregion

}
