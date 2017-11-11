﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Module: GridCreation
/// Team: Client
/// Description: creates 3D grid based on data from CityDataManager
/// Author: 
///	 Name: Dongwon(Shawn) Kim    Date: 2017-09-18
/// Modified by:	
///	 Name: Dongwon(Shawn) Kim   Change:	Fix bug 						Date: 2017-09-19
///  Name: Dongwon(Shawn) Kim   Change:	adding data by CityDataManager  Date: 2017-10-18
/// Based on:  
/// 	https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
/// 	http://answers.unity3d.com/questions/718778/trying-to-create-a-grid.html
/// 	https://docs.unity3d.com/Manual/InstantiatingPrefabs.html
/// 	http://answers.unity3d.com/questions/25352/add-texture-to-gameobject-through-code.html
/// 	https://www.youtube.com/watch?v=810UVUWGlWw
/// </summary>
public class GridCreation : MonoBehaviour {

	// 3D prefab of grid
	public Transform cellPrefab;

	// Size for the cell
	public Vector3 size;

// not using now
//	public int gridWidth;
//	public int gridHeight;

	// Reference for the CityDataManager class
	public CityDataManager cityDataManager;
	// Parent grid object to organize the object in Hierarchy
	public GameObject parentGrid;

	// Use this for initialization
	void Start () {
		parentGrid = GameObject.Find ("Grid");
		cityDataManager = this.GetComponent<CityDataManager> ();
		createGrid ();
		//ShowGrid (false);
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){
	}

	/// <summary>
	/// Creates the grid.
	/// </summary>
	void createGrid(){
		size.x = cityDataManager.Size_x;
		size.z = cityDataManager.Size_z;

        //		Debug.Log (cellPrefab.localScale.x +", " + cellPrefab.localScale.z);
        float totalX = 0;
        float totalZ = 0;
		for(int x = 0; x < size.x; x++){
			for(int z = 0; z < size.z; z++){

				// getting random number for zone( its temporally used for prototype)
//				cellPrefab.GetChild (0).GetComponent<TextMesh> ().text = (Random.Range (0, 4)).ToString();
				cellPrefab.GetChild (1).GetComponent<TextMesh> ().text = "(" + x + ", " + z + ")";
                cellPrefab.GetChild(1).GetComponent<TextMesh>().characterSize = 0;
                cellPrefab.GetChild (0).GetComponent<TextMesh> ().text = cityDataManager.getIndexOfXZ(x, z).ToString();
                cellPrefab.GetChild(0).GetComponent<TextMesh>().characterSize = 0;

                // put the tag plane on the object
                cellPrefab.tag = "plane";
					
				// set color index to GridColor to color the grid
				cellPrefab.GetComponent<GridColor> ().colorIndex = int.Parse(cellPrefab.GetChild (0).GetComponent<TextMesh> ().text);

                // creates each cell of the grid

                float positionX = x + (cellPrefab.localScale.x * x) * 10;
                float positionZ = z + (cellPrefab.localScale.z * z) * 10;

                 Instantiate(cellPrefab, 
							new Vector3(positionX - 27f, 0, positionZ - 26f),
							Quaternion.identity,
							parentGrid.transform);
                print(positionX);
                print(positionZ);
                totalX += positionX;
                totalZ += positionZ;
			}
		}
        print(totalX);
        print(totalZ);
	}

	/// <summary>
	/// Shows the grid.
	/// </summary>
	/// <param name="onOff">If set to <c>true</c> on.</param>
	void showGrid(bool onOff){
		// important this will inactivate all grid objects, so the building and other objects will not be rendered.
		parentGrid.SetActive(onOff);
	}

}