using System;
using System.IO;
using UnityEngine;

public class ObjLogger : MonoBehaviour, Eventable
{
    [Header("References")]
    public PhysicsObject obj;

    [Header("File Information")]
    public string fileLocation = "./Data/";

    private TextWriter csvFile;
    private float time;
    
    private void Awake()
    {
        createCSVFile();
        time = 0.0f;
    }
    
    private void createCSVFile()
    {
        csvFile = new StreamWriter(fileLocation, false);
        csvFile.WriteLine("Time;Position;Velocity;Acceleration;ActualTime");
    }
    
    private void OnDestroy()
    {
        closeFiles();
    }

    private void closeFiles()
    {
        csvFile.Close();
    }
    
    public Event? runTask(float dt, ref Event self)
    {
        int times = (int) (dt / obj.Period);
        obj.residualTime += dt % obj.Period;
        
        if (obj.residualTime > obj.Period)
        {
            times += (int) (obj.residualTime / obj.Period); 
            obj.residualTime %= obj.Period;
        }

        if (obj.selected)
        {
            for (int i = 0; i < times; i++)
            {
                addDataEntry(time);
                time += obj.Period;
            }
        }
        else
        {
            for (int i = 0; i < times; i++)
            {
                addDataEntry(time);
                time += obj.Period;
                
                obj.RunPhysics(obj.Period);
            }
        }

        self.PreviousTime = DateTime.Now;
        self.Time = DateTime.Now.Add(TimeSpan.FromSeconds(obj.Period));
        return self;
    }
    
    private void addDataEntry(float time)
    {
        csvFile.WriteLine(time + ";" + obj.State.Pos.y + ";" + obj.State.Vel.y + ";" + obj.State.Acc.y + ";" + DateTime.Now.Ticks);
    }
}
