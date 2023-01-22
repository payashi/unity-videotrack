using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;


public class TestScript: MonoBehaviour
{
    [SerializeField]
    GameObject origin;

    List<Item> items = new List<Item>();
    private float timeElapsed;

    // Start is called before the first frame update
    void Start()
    {
        // load json file
        string path = "Assets/Resources/iplots.json";
        StreamReader reader = new StreamReader(path);
        String json = reader.ReadToEnd();
        items.AddRange(JsonConvert.DeserializeObject<Item[]>(json));
        items = items.GetRange(0, 3);
        reader.Close();

        foreach (Item item in items)
        {
            float angle = Mathf.Atan2(
                item.plots[item.size-1][1]-item.plots[0][1],
                item.plots[item.size-1][0]-item.plots[0][0]
            ) * Mathf.Rad2Deg - 90;
            item.obj = Instantiate(
                origin, 
                new Vector3(0.0f, -2.0f, 0.0f), 
                Quaternion.Euler(0, angle, 0)
            );
        }
	}

    // Update is called once per frame
    void Update()
    {
	timeElapsed += Time.deltaTime;
	int tidx = (int)(timeElapsed*10);
	foreach (Item item in items)
	{
		if (tidx < item.start)
		{
			continue;
		}else if (tidx <= item.end){
			int offset = tidx - item.start;
			item.obj.transform.position = new Vector3(
				item.plots[offset][0],
				item.plots[offset][2],
				item.plots[offset][1]
			);
		}else if (item.end+1 == tidx){
			Destroy(item.obj);
		}
    	}
     }
}

class Item{
	public float loss;
	public int size;
	public int start, end;
	public float[][] plots;
	public GameObject obj;
} 


