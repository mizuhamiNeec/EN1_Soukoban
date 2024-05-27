using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class wereHouseGenerator : MonoBehaviour {
	[SerializeField] private Vector3Int array = Vector3Int.one;
	[SerializeField] private Vector3 bounds = Vector3.one;

	[SerializeField] private Material material;
	[SerializeField] private Mesh mesh;

	public List<Vector3> positions = new List<Vector3>();

	private GraphicsBuffer posBuffer;

	//void OnValidate() {
	//	Generate();
	//}


	void Start() {
		Generate();
	}

	private void Generate() {
		if(array.x < 1) {
			array.x = 1;
		}

		if(array.y < 1) {
			array.y = 1;
		}

		if(array.z < 1) {
			array.z = 1;
		}

		if(positions.Count > 0) {
			positions.Clear();
		}

		for(int i = 0; i < array.z; i++) {
			for(int j = 0; j < array.y; j++) {
				for(int k = 0; k < array.x; k++) {
					positions.Add(transform.position + new Vector3(bounds.x * k, bounds.y * j, bounds.z * i));
				}
			}
		}

		if(posBuffer != null) {
			posBuffer.Dispose();
		}

		posBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, positions.Count, Marshal.SizeOf<Vector3>());
		posBuffer.SetData(positions);
	}

	private void OnDisable() {
		if(posBuffer != null) {
			posBuffer.Dispose();
		}
	}

	void Update() {
		Generate();

		material.SetBuffer("_Positions", posBuffer);
		Graphics.DrawMeshInstancedProcedural(mesh, 0, material, mesh.bounds, posBuffer.count);
	}
}