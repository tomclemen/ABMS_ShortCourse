using System.Collections.Generic;
using System.Linq;
using Mars.Components.Environments;
using Mars.Components.Layers;
using Mars.Core.Data;
using Mars.Interfaces.Data;
using Mars.Interfaces.Layers;

namespace WorkshopGrid.Model;

public class GridLayer : RasterLayer
{
    #region Init

    /// <summary>
    ///     The initialization method of the GridLayer which spawns and stores the specified number of each agent type
    /// </summary>
    /// <param name="layerInitData"> Initialization data that is passed to an agent manager which spawns the specified
    /// number of each agent type</param>
    /// <param name="registerAgentHandle">A handle for registering agents</param>
    /// <param name="unregisterAgentHandle">A handle for unregistering agents</param>
    /// <returns>A boolean that states if initialization was successful</returns>
    public override bool InitLayer(LayerInitData layerInitData, RegisterAgent registerAgentHandle,
        UnregisterAgent unregisterAgentHandle)
    {
        var initLayer = base.InitLayer(layerInitData, registerAgentHandle, unregisterAgentHandle);

        DiagonalRunnerEnvironment = new SpatialHashEnvironment<DiagonalRunner>(Width, Height);
        StraightRunnerEnvironment = new SpatialHashEnvironment<StraightRunner>(Width, Height);

        var agentManager = layerInitData.Container.Resolve<IAgentManager>();

        DiagonalRunners = agentManager.Spawn<DiagonalRunner, GridLayer>().ToList();
        StraightRunners = agentManager.Spawn<StraightRunner, GridLayer>().ToList();
        HelperAgents = agentManager.Spawn<HelperAgent, GridLayer>().ToList();

        return initLayer;
    }

    #endregion

    #region Methods

    /// <summary>
    ///     Checks if the grid cell (x,y) is accessible
    /// </summary>
    /// <param name="x">x-coordinate of grid cell</param>
    /// <param name="y">y-coordinate of grid cell</param>
    /// <returns>Boolean representing if (x,y) is accessible</returns>
    public override bool IsRoutable(int x, int y) => this[x, y] == 0;

    #endregion

    #region Fields and Properties

    /// <summary>
    ///     The environment of the DiagonalRunner agents
    /// </summary>
    public SpatialHashEnvironment<DiagonalRunner> DiagonalRunnerEnvironment { get; set; }

    /// <summary>
    ///     The environment of the StraightRunner agents
    /// </summary>
    public SpatialHashEnvironment<StraightRunner> StraightRunnerEnvironment { get; set; }

    /// <summary>
    ///     A collection that holds the DiagonalRunner instances
    /// </summary>
    public List<DiagonalRunner> DiagonalRunners { get; private set; }

    /// <summary>
    ///     A collection that holds the StraightRunner instances
    /// </summary>
    public List<StraightRunner> StraightRunners { get; private set; }

    /// <summary>
    ///     A collection that holds the HelperAgent instance
    /// </summary>
    public List<HelperAgent> HelperAgents { get; private set; }

    #endregion
}