using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MUGCUP
{
	public class PoolSystem<T> : MonoBehaviour where T : Component
	{
		public IObjectPool<T> Pool { get; private set; }

		[SerializeField] protected int PoolSize = 100;
		[SerializeField] protected T Prefab;

		protected readonly List<T> ActivePoolObjects = new List<T>();

		public virtual void Initialized()
		{
			Pool = new ObjectPool<T>(CreateObject, OnGetObject, OnRelease, OnObjectDestroyed, maxSize: PoolSize);
		}
        
		protected virtual T CreateObject()
		{
			throw new NotImplementedException();
		}

		protected virtual void OnGetObject(T _placement)
		{
			throw new NotImplementedException();
		}

		protected virtual void OnRelease(T _placement)
		{
			throw new NotImplementedException();
		}

		protected virtual void OnObjectDestroyed(T _placement)
		{
			throw new NotImplementedException();
		}
        
		public virtual void ClearPool()
		{
			throw new NotImplementedException();
		}
	}
}
