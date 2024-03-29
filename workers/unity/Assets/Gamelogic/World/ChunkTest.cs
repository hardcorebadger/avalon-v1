﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkTest : MonoBehaviour {

	public int[,] blockmap;

	public int startX, startZ;

	public WorldGeneratorTest1 settings;

	public void Setup(WorldGeneratorTest1 s, int sx, int sz) {
		settings = s;
		startX = sx;
		startZ = sz;
		blockmap = new int[settings.chunkSize, settings.chunkSize];
		for (int z = 0; z < settings.chunkSize; z++) {
			for (int x = 0; x < settings.chunkSize; x++) {
				blockmap[x,z] = settings.blockmap[x+startX,z+startZ];
			}
		}
	}

	public void BuildBlockTerrain() {

		Mesh mesh = new Mesh();

		List<Vector3> vertList = new List<Vector3>();
		List<Vector2> uvList = new List<Vector2>();
		List<int> triList = new List<int> ();
		List<Color> colorList = new List<Color> ();

		int i = 0;
		for (int zi = 0; zi < blockmap.GetLength(1); zi++) {
			for (int xi = 0; xi <  blockmap.GetLength(0); xi++) {
				addFace(xi, zi, ref vertList, ref uvList, ref triList, ref colorList, ref i, GetColor(blockmap[xi,zi]));

				int forward;
				if (zi == blockmap.GetLength (1) - 1)
					forward = blockmap [xi, zi];
				else
					forward = blockmap [xi, zi] - blockmap [xi, zi + 1];
				
				if (forward > 0)
					addForwardEdge(forward, xi, zi, ref vertList, ref uvList, ref triList, ref colorList, ref i, GetColor(blockmap[xi,zi]));

				int backward;
				if (zi == 0)
					backward = blockmap [xi, zi];
				else
					backward = blockmap [xi, zi] - blockmap [xi, zi - 1];
				if (backward > 0)
					addBackwardEdge(backward, xi, zi, ref vertList, ref uvList, ref triList, ref colorList, ref i, GetColor(blockmap[xi,zi]));

				int left;
				if (xi == 0)
					left = blockmap [xi, zi];
				else
					left = blockmap [xi, zi] - blockmap [xi-1, zi];
				if (left > 0)
					addLeftEdge(left, xi, zi, ref vertList, ref uvList, ref triList, ref colorList, ref i, GetColor(blockmap[xi,zi]));

				int right;
				if (xi == blockmap.GetLength(0)-1)
					right = blockmap [xi, zi];
				else
					right = blockmap [xi, zi] - blockmap [xi+1, zi];
				if (right > 0)
					addRightEdge(right, xi, zi, ref vertList, ref uvList, ref triList, ref colorList, ref i, GetColor(blockmap[xi,zi]));
			}
		}

		mesh.vertices = vertList.ToArray();
		mesh.uv = uvList.ToArray();
		mesh.triangles = triList.ToArray();
		mesh.colors = colorList.ToArray();
		mesh.RecalculateNormals();

		setMesh(mesh);

	}

	private void addForwardEdge(int length, int x, int z, ref List<Vector3> vertList, ref List<Vector2> uvList, ref List<int> triList, ref List<Color> colorList,  ref int i, Color color) {
		Vector3 v1 = ToPosition (x+1, z+1, blockmap[x,z]);
		Vector3 v2 = ToPosition (x, z+1, blockmap[x,z]);
		Vector3 v3 = ToPosition (x, z+1, blockmap[x,z]-length);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);

		v1 = ToPosition (x+1, z+1, blockmap[x,z]);
		v2 = ToPosition (x, z+1, blockmap[x,z]-length);
		v3 = ToPosition (x+1, z+1, blockmap[x,z]-length);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
	}

	private void addBackwardEdge(int length, int x, int z, ref List<Vector3> vertList, ref List<Vector2> uvList, ref List<int> triList, ref List<Color> colorList,  ref int i, Color color) {
		Vector3 v1 = ToPosition (x, z, blockmap [x, z] - length);
		Vector3 v2 = ToPosition (x, z, blockmap[x,z]);
		Vector3 v3 = ToPosition (x+1, z, blockmap[x,z]);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);

		v1 = ToPosition (x+1, z, blockmap[x,z]-length);
		v2 = ToPosition (x, z, blockmap[x,z]-length);
		v3 = ToPosition (x+1, z, blockmap[x,z]);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
	}

	private void addLeftEdge(int length, int x, int z, ref List<Vector3> vertList, ref List<Vector2> uvList, ref List<int> triList, ref List<Color> colorList,  ref int i, Color color) {
		Vector3 v1 = ToPosition (x, z+1, blockmap[x,z]);
		Vector3 v2 = ToPosition (x, z, blockmap[x,z]);
		Vector3 v3 = ToPosition (x, z, blockmap [x, z] - length);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);

		v1 = ToPosition (x, z+1, blockmap[x,z]);
		v2 = ToPosition (x, z, blockmap[x,z]-length);
		v3 = ToPosition (x, z+1, blockmap[x,z]-length);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
	}

	private void addRightEdge(int length, int x, int z, ref List<Vector3> vertList, ref List<Vector2> uvList, ref List<int> triList, ref List<Color> colorList,  ref int i, Color color) {
		Vector3 v1 = ToPosition (x+1, z, blockmap [x, z] - length);
		Vector3 v2 = ToPosition (x+1, z, blockmap[x,z]);
		Vector3 v3 = ToPosition (x+1, z+1, blockmap[x,z]);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);

		v1 = ToPosition (x+1, z+1, blockmap[x,z]-length);
		v2 = ToPosition (x+1, z, blockmap[x,z]-length);
		v3 = ToPosition (x+1, z+1, blockmap[x,z]);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
	}

	private void addFace(int x, int z, ref List<Vector3> vertList, ref List<Vector2> uvList, ref List<int> triList, ref List<Color> colorList,  ref int i, Color color) {
		Vector3 v1 = ToPosition (x, z, blockmap[x,z]);
		Vector3 v2 = ToPosition (x, z+1, blockmap[x,z]);
		Vector3 v3 = ToPosition (x+1, z, blockmap[x,z]);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);

		v1 = ToPosition (x, z+1, blockmap[x,z]);
		v2 = ToPosition (x+1, z+1, blockmap[x,z]);
		v3 = ToPosition (x+1, z, blockmap[x,z]);

		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
	}

	private void addTri(Vector3 v1, Vector3 v2, Vector3 v3, ref List<Vector3> vertList, ref List<Vector2> uvList, ref List<int> triList, ref List<Color> colorList,  ref int i, Color color) {
		addVertex(v1, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v2, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
		addVertex(v3, ref vertList, ref uvList, ref triList, ref colorList, ref i, color);
	}

	private void addVertex(Vector3 pos, ref List<Vector3> vert, ref List<Vector2> uv, ref List<int> tri, ref List<Color> color,  ref int i, Color c) {
		vert.Add (pos);
		uv.Add (new Vector2 (0, 0));
		tri.Add (i);
		color.Add (c);
		i++;
	}

	private void setMesh(Mesh m) {
		GetComponent<MeshFilter> ().mesh = m;
		GetComponent<MeshCollider> ().sharedMesh = m;
	}

	public struct HeightmapIndex {
		public int x;
		public int z;
		public HeightmapIndex(int xi, int zi) {
			x = xi;
			z = zi;
		}
	};

	public Vector3 ToPosition(int x, int z, float blockHeight) {
		Vector3 v = Vector3.zero;
		v.x = (startX+x) * settings.blockSize;
		v.z = (startZ+z) * settings.blockSize;
		v.y = blockHeight * settings.blockSize;
		return v;
	}

	private Color GetColor(int blockHeight) {
		float height = (float)blockHeight / (float)settings.maxBlockHeight;
		if (height <= settings.waterLevel)
			return settings.waterColor;

		height -= settings.tempurature;
		for (int i = 0; i < settings.gradient.GetLength(0); i++) {
			if (settings.gradient [i].point > height) {
				if (i == 0)
					return settings.gradient [i].color;
				else {
					float v = height - settings.gradient [i-1].point;
					float mix = v / (settings.gradient [i].point - settings.gradient [i-1].point);
					return Mix (settings.gradient [i-1].color, settings.gradient [i].color, mix);
				}
			}
		}
		if (settings.gradient.GetLength (0) > 0)
			return settings.gradient [settings.gradient.GetLength (0)-1].color;
		else
			return new Color (height, height, height);
	}

	private Color Mix(Color c1, Color c2, float point) {
		return (c2 - c1) * point + c1;
	}
		
}
