using System.Collections.Generic;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Data;
using Mars.Interfaces.Environments;
using Mars.Interfaces.Layers;
using Mars.Interfaces.Model;
using Mars.Interfaces.Model.Options;

namespace GeodataBlueprint.Model;

public class GraphLayer : AbstractLayer
{
    public SpatialGraphEnvironment Environment {get; private set;}

    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle = null,
        UnregisterAgent unregisterAgent = null)
    {
        base.InitLayer(layerInitData, registerAgentHandle, unregisterAgent);

        var file = layerInitData.LayerInitConfig.File;
        // Environment = new SpatialGraphEnvironment(file); //TODO sufficient for graphml
        Environment = new SpatialGraphEnvironment(new SpatialGraphOptions()
        {
            GraphImports = new List<Input>()
            {
                new Input()
                {
                    File = file,
                    InputConfiguration = new InputConfiguration()
                    {
                        IsBiDirectedImport = true
                    }
                }
            }
        });
            
        //TODO export the current environment to a geojson file
        // File.WriteAllText("exported.geojson",Environment.ToGeoJson()); 
            
        Goal = Environment.GetRandomNode().Position;
        
        var agentManager = layerInitData.Container.Resolve<IAgentManager>();
        agentManager.Spawn<Human, GraphLayer>().ToList();
            
        return true;
    }

    public Position Goal {get; private set;}

}