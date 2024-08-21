using System.Linq;
using UnityEngine;

namespace CombatRevampScripts.Extensions
{
	public static class GameObjectExtension
	{
		public static GameObject FindChildWithTag<T>(this GameObject parent, string tag) {
			Transform t = parent.transform;
			return (from Transform tr in t where tr.CompareTag(tag) select tr.gameObject).FirstOrDefault();
		}	
	}
}