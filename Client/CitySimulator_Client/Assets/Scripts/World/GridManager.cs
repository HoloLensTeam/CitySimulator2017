﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Module: GridManager
/// Team: Client
/// Description: creates 3D grid based on data from CityDataManager
/// Author: 
///	 Name: Dongwon(Shawn) Kim    Date: 2017-09-18
/// Modified by:	
///	 Name: Dongwon(Shawn) Kim   Change:	Fix bug 						Date: 2017-09-19
///  Name: Dongwon(Shawn) Kim   Change:	adding data by CityDataManager  Date: 2017-10-18
///  Name: Dongwon(Shawn) Kim   Change:	turnGrid function        		Date: 2017-11-13
///  Name: Dongwon(Shawn) Kim   Change:	Changed this from Creation to GridManager		Date: 2017-11-28
/// Based on:  
/// 	https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
/// 	http://answers.unity3d.com/questions/718778/trying-to-create-a-grid.html
/// 	https://docs.unity3d.com/Manual/InstantiatingPrefabs.html
/// 	http://answers.unity3d.com/questions/25352/add-texture-to-gameobject-through-code.html
/// 	https://www.youtube.com/watch?v=810UVUWGlWw
/// </summary>
public class GridManager : MonoBehaviour {

	// 3D prefab of grid
	public Transform cellPrefab;

	// Size for the cell
	public Vector3 size;

	public Texture roadTexture;

	public bool onOffSwitch;
	
// not using now
//	public int gridWidth;
//	public int gridHeight;

	// Reference for the CityDataManager class
	public CityDataManager cityDataManager;
	// Parent grid object to organize the object in Hierarchy
	public GameObject parentGrid;

	public bool turnOnGrid;
	public bool turnOffGrid;

	// Use this for initialization
	void Start () {
		parentGrid = GameObject.Find ("Grid");
		cityDataManager = this.GetComponent<CityDataManager> ();
		//ShowGrid (false);
	}

	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update(){

		if(turnOnGrid){
			turnEntireGrid(true);
			turnOnGrid=false;
		} else if(turnOffGrid) {
			turnEntireGrid(false);
			turnOffGrid=false;
		}

	}

	public void createRoad(int x, int z){
		Debug.Log("Grid created at " + x + " " + z);
		cellPrefab.GetChild (0).GetComponent<TextMesh> ().text = "0";
		cellPrefab.GetChild (1).GetComponent<TextMesh> ().text = "(" + x + ", " + z + ")";	

		// put the tag plane on the object
		cellPrefab.tag = "plane";
			
		// set color index to GridColor to color the grid
		cellPrefab.GetComponent<GridColor> ().colorIndex = int.Parse(cellPrefab.GetChild (0).GetComponent<TextMesh> ().text);

		// creates each cell of the grid
	 	Instantiate(cellPrefab, 
					new Vector3(
					x + (cellPrefab.localScale.x * x)*8,
					0,
					z + (cellPrefab.localScale.z * z)*8),
					Quaternion.identity,
					parentGrid.transform);	            			
	}


	/// <summary>
	/// Creates the grid.
	/// </summary>
	public bool createEntireGrid(){
		size.x = cityDataManager.Size_x;
		size.z = cityDataManager.Size_z;

		for(int x = 0; x < size.x; x++){
			for(int z = 0; z < size.z; z++){
				
				// apply text to the each plane
				cellPrefab.GetChild (0).GetComponent<TextMesh> ().text = cityDataManager.getIndexOfXZ(x, z).ToString();
				cellPrefab.GetChild (1).GetComponent<TextMesh> ().text = "(" + x + ", " + z + ")";				

				// put the tag plane on the object
				cellPrefab.tag = "plane";
					
				// set color index to GridColor to color the grid
				cellPrefab.GetComponent<GridColor> ().colorIndex = int.Parse(cellPrefab.GetChild (0).GetComponent<TextMesh> ().text);
				
				MeshRenderer component = cellPrefab.GetComponent<MeshRenderer>();
				//component.material.mainTexture = roadTexture;

				// creates each cell of the grid
			 	Instantiate(cellPrefab, 
							new Vector3(
							x + (cellPrefab.localScale.x * x)*8,
							0,
							z + (cellPrefab.localScale.z * z)*8),
							Quaternion.identity,
							parentGrid.transform);

				cellPrefab.GetChild (0).GetComponent<MeshRenderer>().enabled = onOffSwitch;
				cellPrefab.GetChild (1).GetComponent<MeshRenderer>().enabled = onOffSwitch;


			}
		}
        return true;
	}

	/// <summary>
	/// Shows the grid.
	/// </summary>
	void turnEntireGrid(bool on){

		this.onOffSwitch = on;
		// important this will inactivate all grid objects, so the building and other objects will not be rendered.
		// parentGrid.SetActive(onOff);
		GameObject[] planes = GameObject.FindGameObjectsWithTag ("plane");

		foreach (GameObject plane in planes) {
			Transform planeTransform = plane.transform;

			if(planeTransform.GetChild(0).GetComponent<TextMesh>().text != "0"){
				MeshRenderer component = plane.GetComponent<MeshRenderer>();
				component.enabled = on;
			} else {
				MeshRenderer component = plane.GetComponent<MeshRenderer>();
				component.material.mainTexture = roadTexture;
			}

			planeTransform.GetChild (0).GetComponent<MeshRenderer>().enabled = on;
			planeTransform.GetChild (1).GetComponent<MeshRenderer>().enabled = on;

		}
	}

}