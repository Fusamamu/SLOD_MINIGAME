using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MUGCUP
{
	public class ParticlePool : PoolSystem<ParticleUnit>
	{
		[SerializeField] private Transform ParticleCollection;
        
		public override void Initialized()
		{
			base.Initialized();
		}
        
		protected override ParticleUnit CreateObject()
		{
			var _particleUnit = Instantiate(Prefab, ParticleCollection);

			_particleUnit.name = $"_particle_";
			_particleUnit.SetPool(Pool);
			_particleUnit.Init();
            
			return _particleUnit;
		}

		protected override void OnGetObject(ParticleUnit _particleUnit)
		{
			_particleUnit.gameObject.SetActive(true);
			_particleUnit.IsInPool = false;
            
			ActivePoolObjects.Add(_particleUnit);
		}

		protected override void OnRelease(ParticleUnit _particleUnit)
		{
			_particleUnit.IsInPool = true;
			_particleUnit.gameObject.SetActive(false);

			ActivePoolObjects.Remove(_particleUnit);
		}

		protected override void OnObjectDestroyed(ParticleUnit _particleUnit)
		{
			Destroy(_particleUnit.gameObject);
		}
        
		public override void ClearPool()
		{
			if (ActivePoolObjects.Count > 0)
			{
				ActivePoolObjects.ForEach(_bullet => _bullet.ReturnToPool());
				ActivePoolObjects.Clear();
			}
		}
	}
}
