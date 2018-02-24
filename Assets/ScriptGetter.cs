using UnityEngine;
using System.Collections;

public class ScriptGetter : MonoBehaviour {

	public    GameObject []    objects;    //  Objects you want to get scripts from


	public void Start () {
		scripts    = new Component [ objects.Length ];
		//  Find the Scripts in the GameObjects
		for ( int i = 0 ; i < scripts.Length ; i++ ) {
			Component [] scriptsOnObject    = objects [ i ].GetComponents < Component > ();
			//  Search each script on the component, and filter out default Unity ones
			for ( int j = 0 ; j < scriptsOnObject.Length ; j++ ) {
				if ( !UnwantedTypes ( scriptsOnObject [ j ] ) ) {
					scripts [ i ] = scriptsOnObject [ j ];
				}
			}
		}
		//  Display our scripts from our objects
		for ( int i = 0 ; i < scripts.Length ; i++ ) {
			Debug.Log ( scripts [ i ] );
		}
	}


	private    Component []    scripts;  //  Our cached Scripts will live here

	private System.Type [] unwantedTypes = { typeof ( Transform ) , typeof ( Collider ) , 
		typeof ( MeshRenderer ) , typeof ( MeshFilter ) };

	//  Only allow types we want through
	private bool UnwantedTypes ( Component _component ) {
		for ( int i = 0 ; i < unwantedTypes.Length ; i++ ) {
			if ( _component.GetType ().Equals ( unwantedTypes [ i ] ) ) {
				return true;
			}
		}
		return false;
	}

}