using UnityEngine;

public static class Utils {

	public static Vector3 RandomVec3(float min, float max){
		return new Vector3(
			Random.Range(min, max),
			Random.Range(min, max),
			Random.Range(min, max)
		);
	}

	public static Vector2 RandomVec2(float min, float max){
		return new Vector2(
			Random.Range(min, max),
			Random.Range(min, max)
		);
	}

}