using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Instance { get; private set; }
        
        private readonly Dictionary<Type, Service> serviceTable = new ();

        private void Awake()
        {
            if (Instance != null && Instance != this)
                Destroy(this);
            else
                Instance = this;

            var _services = GetComponentsInChildren<Service>();

            foreach (var _service in _services)
                Add(_service);
        }

        private void Start()
        {
            Get<AudioManager>   ().Init();
            Get<UIManager>      ().Init();
            Get<ParticleManager>().Init();
            Get<PlayerManager>  ().Init();
            Get<BulletManager>  ().Init();
            Get<EnemyManager>   ().Init();
            Get<LevelManager>   ().Init();
        }

        public void Add<T>(T _gui) where T : Service
        {
            if (!serviceTable.ContainsKey(_gui.GetType()))
                serviceTable.Add(_gui.GetType(), _gui);
        }

        public T Get<T>() where T : Service
        {
            if (serviceTable.TryGetValue(typeof(T), out var _requestedService))
                return _requestedService as T;

            return null;
        }
    }
}
