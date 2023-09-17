using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MUGCUP
{
	public interface IPoolAble<T> where T : Component
	{
		public IObjectPool<T> Pool { get; }
        
		public bool IsInPool { get; set; }

		public void SetPool(IObjectPool<T> _pool);
		
		public void ReturnToPool();
	}
}
