using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
public class CreatePeople : MonoBehaviour {
	public World world;
	public static Dictionary<Region,Rect> locations = new Dictionary<Region, Rect>();
	List<Person> People = new List<Person>();
	static int numberofpeople = 500;
	// Use this for initialization
	void Start () {
		using(Stream s = new FileStream("Assets//data.xml",FileMode.Open))
			world = (World)new XmlSerializer(typeof(World)).Deserialize(s);
		List<Rect> regionrects  = new List<Rect>(){new Rect(-10,-20,35,40),new Rect(-100,-50,90,95),new Rect(35,-5,65,55),new Rect(8,15,17,10),new Rect(-10,20,40,30),new Rect(30,-30,70,40)};
		for(int i = 0;i<world.regions.Count;i++)
		{
			
			locations.Add(world.regions[i],regionrects[i]);
			
		}
		int total = 0;
		foreach(Region r in world.regions) total+= r.population;
		float proportion = ((float)numberofpeople)/(float)total;
		foreach(Region r in world.regions)
		{
			Color regioncolour = new Color(Random.Range(0,25)*10,Random.Range(0,25)*10,Random.Range(0,25)*10,255);
			float pop = r.population;
			float number = r.population*proportion;
			
			for(int i = 0;i<number;i++)
				People.Add(Person.Create(r,regioncolour));
			
			
			
			
		}
			
			
		
		
		
	}
	
	// Update is called once per frame
	public int year = 0;
	public int updatetimer = 0;
	void Update () {
		if(updatetimer==100)
		{
			updatetimer = 0;
			year++;
			((GUIText)(GUIText.FindObjectOfType(typeof(GUIText)))).text = "Year: "+year.ToString(); 
			foreach(Person p in People)
					p.Update();
		}
		else
			updatetimer++;
	}
}
public class World
{
public List<Region> regions = new List<Region>();	
	
}
public class Region
{
	public string name;
	public List<Stat> stats = new List<Stat>();
	
	public int population = 259950;
	
	
}
public class Stat
{
	public int maxage;
	public  List<string> cause;
	public  List<float> number;
	
	
}
public class Person
{
	GameObject gameobject;
	Region region;
	int age = 0;
	bool alive = true;
	
	public Person(GameObject g,Region r)
	{
		gameobject = g;
		region = r;
	}
	public static Person Create(Region r,Color regioncolour)
	{
		
		
		RaycastHit hit = new RaycastHit();
		float x = 0,z = 0;
		x = Random.Range(CreatePeople.locations[r].x,CreatePeople.locations[r].width+CreatePeople.locations[r].x);
		z = Random.Range(CreatePeople.locations[r].y,CreatePeople.locations[r].height+CreatePeople.locations[r].y);
		
		Ray ray = new Ray(new Vector3(x,50,z),Vector3.down);
		bool collision = GameObject.Find("ReliefMap").collider.Raycast(ray,out hit,100);
		while(!(hit.point.y>0 && collision && hit.point!=new Vector3()) )
		{
			
			
		x = Random.Range(CreatePeople.locations[r].x,CreatePeople.locations[r].width+CreatePeople.locations[r].x);
		z = Random.Range(CreatePeople.locations[r].y,CreatePeople.locations[r].height+CreatePeople.locations[r].y);
		
		ray = new Ray(new Vector3(x,10,z),Vector3.down);
		collision = GameObject.Find("ReliefMap").collider.Raycast(ray,out hit,100);
		}
		Vector3 hitpoint = hit.point;
		hitpoint.y+=1;
		GameObject g = (GameObject)GameObject.Instantiate(GameObject.Find("Flame"),hitpoint,new Quaternion(0,0,0,0));
		Person toreturn = new Person(g,r);
		toreturn.gameobject.transform.FindChild("P").particleSystem.startColor = regioncolour;
		return new Person(g,r);
	}

	public void Update()
	{
		
		if(alive)
		{
				
			int agerange = 0;
			for(int i = 0; i<region.stats.Count-1;i++)
				if(age>region.stats[i].maxage)
					agerange=i;
			
			int death = 0;
			//TODO add real population 
			int randomdeath = Random.Range(0,100000000);
			float max = region.stats[agerange].number[region.stats[agerange].number.Count-1];
			if(randomdeath<max)
			{
				for(int i = 0; i<region.stats[agerange].number.Count-1;i++)
				{
					if(region.stats[agerange].number[i]<randomdeath)
						death = i;
						
					
				}
				Object.Instantiate(GameObject.Find("Shockwave"),gameobject.transform.position,gameobject.transform.rotation);
				Object obj = Object.Instantiate(GameObject.Find("Text"),gameobject.transform.position,GameObject.Find("Text").transform.rotation);
				((TextMesh)((GameObject)obj).GetComponent(typeof(TextMesh))).text = region.stats[agerange].cause[death];
				Object.Destroy(gameobject);
				
				alive = false;
			
			}
			else
			{
				
			age++;	
				
			}
				
		}
	}
				
	
}
