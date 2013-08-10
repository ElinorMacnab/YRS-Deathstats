using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Text;
public class CreatePeople : MonoBehaviour {
	public World world;
	public static Dictionary<Region,Rect> locations = new Dictionary<Region, Rect>();
	List<Person> People = new List<Person>();
	public static Total pop = new Total();
	static int numberofpeople = 500;
	// Use this for initialization
	void Start () {
        TextAsset dataasset = Resources.Load("data") as TextAsset;
        TextAsset popasset = Resources.Load("Population") as TextAsset;
        byte[] databytes = Encoding.ASCII.GetBytes(dataasset.text);
        byte[] popbytes = Encoding.ASCII.GetBytes(popasset.text); 
        
		//using(Stream s = new FileStream("Assets//data.xml",FileMode.Open))
		//	world = (World)new XmlSerializer(typeof(World)).Deserialize(s);
		//using(Stream s = new FileStream("Assets//Population.xml",FileMode.Open))
		//	pop = (Total)new XmlSerializer(typeof(Total)).Deserialize(s);
		
        using(Stream s = new MemoryStream(databytes))
			world = (World)new XmlSerializer(typeof(World)).Deserialize(s);
		using(Stream s = new MemoryStream(popbytes))
			pop = (Total)new XmlSerializer(typeof(Total)).Deserialize(s);
        
        List<Rect> regionrects  = new List<Rect>(){new Rect(-10,-20,35,40),new Rect(-100,-35,90,80),new Rect(35,-5,65,55),new Rect(8,15,17,10),new Rect(-10,20,40,30),new Rect(30,-30,70,40)};
		for(int i = 0;i<world.regions.Count;i++)
		{
			
			locations.Add(world.regions[i],regionrects[i]);
			
		}
		int total = 0;
		foreach(Region r in world.regions) total+= r.population;
		float proportion = ((float)numberofpeople)/(float)total;
		foreach(Region r in world.regions)
		{
			var index = world.regions.IndexOf(r);
			Color regioncolour = Color.yellow;
			if(index==0)
			{
				regioncolour = Color.yellow;
				
			}
			if(index==1)
			{
				regioncolour = Color.red;
				
			}
			if(index==2)
			{
				regioncolour = Color.blue;
					
				
			}
			if(index==3)
			{
				regioncolour = Color.green;	
				
			}
			if(index==4)
			{
				regioncolour = Color.magenta;	
				
			}
			if(index==5)
			{
				regioncolour = Color.white;
				
			}
				
			
			float pop = r.population;
			float number = r.population*proportion;
			
			for(int i = 0;i<number;i++)
				People.Add(Person.Create(r,regioncolour,index));
			
			
			
			
		}
			
			
		
		
		
	}
	
	// Update is called once per frame
	public int Deaths = 0;
	public int updatetimer = 0;
	void Update () {
		if(updatetimer==50)
		{
			updatetimer = 0;
			((GUIText)(GUIText.FindObjectOfType(typeof(GUIText)))).text = "Death: "+Deaths.ToString(); 
			foreach(Person p in People)
					if(p.Update())
                    Deaths++;
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
    int RegionIndex;
	
	public Person(GameObject g,Region r)
	{
		gameobject = g;
		region = r;
	}
	public static Person Create(Region r,Color regioncolour,int regionindex)
	{
		
		
		
		var hit = new RaycastHit();
		float x = 0,z = 0;
		x = Random.Range(CreatePeople.locations[r].x,CreatePeople.locations[r].width+CreatePeople.locations[r].x);
		z = Random.Range(CreatePeople.locations[r].y,CreatePeople.locations[r].height+CreatePeople.locations[r].y);
		
		Ray ray = new Ray(new Vector3(x,50,z),Vector3.down);
		bool collision = GameObject.Find("ReliefMap").collider.Raycast(ray,out hit,100);
		while(!(hit.point.y>0 && collision && hit.point!=new Vector3()))
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
		toreturn.RegionIndex = regionindex;
        return toreturn;
	}
	public bool Update()
	{
		
		if(alive)
		{
				
			int agerange = 0;
			for(int i = 0; i<region.stats.Count-1;i++)
				if(age>region.stats[i].maxage)
					agerange=i;
			
			int death = 0;
			float randomdeath = Random.Range(0,CreatePeople.pop.stat[RegionIndex][agerange]*1000);
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
                return true;;
			
			}
			else
			{
				
			age++;
            return false;;
				
			}
				
		}
        return false;;
	}
				
	
}
public class Total
{
	
	public List<List<float>> stat = new List<List<float>>();
	
}
