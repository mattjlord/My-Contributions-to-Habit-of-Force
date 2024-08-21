using UnityEngine;

namespace CombatRevampScripts.Extensions
{
	public static class RayExtension
	{
		public static bool RaycastToXZPlane(this Ray ray, out Vector3 hitPoint)
		{
			var hPlane = new Plane(Vector3.up, Vector3.zero);

			if (hPlane.Raycast(ray, out var distance))
			{
				hitPoint = ray.GetPoint(distance);
				return true;
			}

			hitPoint = Vector3.zero;
			return false;
		}
	}
}