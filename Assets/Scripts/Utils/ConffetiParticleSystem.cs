using System;
using Levels;
using Scenes;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(ParticleSystem))]
public class ConffetiParticleSystem : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlesSystem;
    private ParticleSystem.Particle[] particles;
    private int _lastParticleCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (particlesSystem)
        {
            particlesSystem =  GetComponent<ParticleSystem>();
        }
        particles = new ParticleSystem.Particle[particlesSystem.main.maxParticles];
    }

    private void Start()
    {
        //We only trigger particles when player reached when we are in a level
        if (LevelManager.Instance.CurrentScene != Scenes.Scenes.WIN &&  LevelManager.Instance.CurrentScene != Scenes.Scenes.LOSE)
        {
            Level.Instance.OnEndReached += StartParticleSystem;
        }
        else if  (LevelManager.Instance.CurrentScene == Scenes.Scenes.WIN)
        {
            StartParticleSystem();
        }
    }

    private void StartParticleSystem()
    {
        particlesSystem.Play();
    }

    private void LateUpdate()
    {
        // if (!particlesSystem.isPlaying) return;
        
        var count = particlesSystem.GetParticles(particles);
        for (int i = _lastParticleCount; i < count; i++)
        {
            particles[i].startColor = Random.ColorHSV(
                0f, 1f,
            .8f, 1f,
                .8f, 1f
            );
        }
        _lastParticleCount = count;
        particlesSystem.SetParticles(particles, count);
    }

    private void OnDestroy()
    {
        if (Level.Instance)
        {
            Level.Instance.OnEndReached -= StartParticleSystem;
        }
    }
}
