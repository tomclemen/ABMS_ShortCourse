﻿using System;
using System.IO;
using GeodataBlueprint.Model;
using Mars.Components.Starter;
using Mars.Interfaces.Model;

namespace GeodataBlueprint
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            // the scenario consist of the model (represented by the model description)
            // an the simulation configuration (see config.json)
            
            // Create a new model description that holds all parts of the model (agents, entities, layers)
            var description = new ModelDescription();
            
            // All environments where the agent move on resolve routes.
            description.AddLayer<GraphLayer>();
            description.AddLayer<PoiLayer>();

            description.AddAgent<Human, GraphLayer>();
            
            // scenario definition
            // use config.json that holds the specification of the scenario
            var file = File.ReadAllText("config.json");
            var config = SimulationConfig.Deserialize(file);
            
            // Create simulation task
            var task = SimulationStarter.Start(description, config);
            
            // Run simulation
            var loopResults = task.Run();
            
            // Feedback to user that simulation run was successful
            Console.WriteLine($"Simulation execution finished after {loopResults.Iterations} steps");
        }
    }
}