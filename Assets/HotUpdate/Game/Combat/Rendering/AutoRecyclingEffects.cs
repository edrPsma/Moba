using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRecyclingEffects : MonoBehaviour
{
    [SerializeField] float _duration;
    bool _startPlay;
    ParticleSystem _particle;
    float _timer;

    public void PlayAndRecycle()
    {
        _startPlay = true;
        _particle = GetComponentInChildren<ParticleSystem>();
        _timer = _duration;
    }

    void Update()
    {
        if (!_startPlay) return;

        if (_timer <= 0)
        {
            GameObject.Destroy(gameObject);
        }
        _timer -= Time.deltaTime;
        float speed = MVCContainer.Get<ICombatSystem>().GameSpeed.Value;
        _particle.Simulate(Time.deltaTime * speed, withChildren: true, restart: false);
    }
}
