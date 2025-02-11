﻿using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class DeathScript : MonoBehaviour {

	public static Dictionary<string,KeyValuePair<Rect,int>> regions = new Dictionary<string,KeyValuePair<Rect,int>>();
	public List<Person> people = new List<Person>();
	public Dictionary<string,Root> data = new Dictionary<string,Root>();
	public System.Data.DataSet dataset;
	// Use this for initialization
	void Start () {
		
		
		
		int count = 0;
		
		dataset = new System.Data.DataSet();
		foreach(string file in Directory.GetFiles("Assets/Data"))
		{
			
			using(Stream xmlStream = new FileStream(file,FileMode.Open,FileAccess.Read))
			{
			dataset.ReadXml(xmlStream);
			
			}
			FileStream stream = new FileStream(file,FileMode.Open);
			data.Add(file,(Root)new XmlSerializer(typeof(Root)).Deserialize(stream));
			;
			//regions.Add("Europe",new KeyValuePair<Rect,int>( new Rect(-10,20,110,30),);
			if(count==0)
			{
			//	regions.Add(file,new KeyValuePair<Rect,int>( new Rect(-10,-20,40,40),400801630));
				regions.Add(file,new KeyValuePair<Rect,int>( new Rect(-100,-200,400,400),100));
			}
		}
		GameObject flame = GameObject.Find("Flame");
		flame.transform.position = new Vector3(100000000000000,100000000000000000,100000000000000);
		Random rand = new Random();
		for(int i = 0;i<20;i++)
		{
			foreach(KeyValuePair<string,KeyValuePair<Rect,int>> r in regions)
			{
				people.Add(new Person((GameObject)Instantiate(flame,new Vector3(Random.Range(r.Value.Key.xMin,r.Value.Key.xMax),0,Random.Range(r.Value.Key.yMin,r.Value.Key.yMax)),new Quaternion(0,0,0,0)),r.Key));
				
				
			}
		}
		
		
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Person p in people)
		{
			int r = Random.Range(0,regions[p.file].Value);
				
			
			p.Update();
			if(p.age>80)
			{
				string death = null;
				//foreach(System.Data.DataColumn collection in dataset.Tables[0].Columns[8].)
					
			//		Debug.Log(collection);
			//		if(r>collection)
			//			death = row.Column1;
				
			//	if(death!=null)	
			//		p.State(death);
				
			
				foreach (System.Data.DataRow row in dataset.Tables[0].Rows)
				{
					foreach(object cell in row.ItemArray)
					{
						Debug.Log (cell);
						
						
					}
					
				}	
				
			}
		
		}
	}
		
		
}
public class Root
{
	public DataRow Row;
	
	
	
}
public class DataRow
{
	public string Column1;
	public float Column_0_4;
	public float Column_5_14;
	public float Column_15_29;
	public float Column_30_44;
	public float Column_45_59;
	public float Column_60_69;
	public float Column_70_79;
	public float Column_80;
}
public class Person
{
	public int age = 0;
	public GameObject flame ;
	public string file;
	public Person(GameObject g,string f)
	{
		file = f;
		flame = g;
	}
	public void Update()
	{
	age++;	
		
	}
	public void State(string state)
	{
		
		Object.Destroy(flame);
	}
			
			
			
	
}


 

