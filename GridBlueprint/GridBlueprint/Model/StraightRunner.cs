using System;
using System.Collections.Generic;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Environments;

namespace GridBlueprint.Model;

public class StraightRunner : IAgent<GridLayer>, IPositionable
{
    #region Init

    /// <summary>
    ///     The initialization method of the StraightRunner which is executed once at the beginning of a simulation.
    ///     It sets an initial position and generates a list of movement directions.
    /// </summary>
    /// <param name="layer">The GridLayer that manages the agent</param>
    public void Init(GridLayer layer)
    {
        _layer = layer;
        Position = new Position(1, 1);
        _directions = new List<Position>
        {
            MovementDirections.North,
            MovementDirections.East,
            MovementDirections.South,
            MovementDirections.West
        };
        _layer.StraightRunnerEnvironment.Insert(this);
    }

    #endregion

    #region Tick

    /// <summary>
    ///     The tick method of the StraightRunner which is executed during each time step of the simulation.
    ///     A StraightRunner can move randomly along straight lines. It must stay within the bounds of the GridLayer
    ///     and cannot move onto grid cells that are not routable.
    /// </summary>
    public void Tick()
    {
        var nextDirection = _directions[_random.Next(_directions.Count)];
        var newX = Position.X + nextDirection.X;
        var newY = Position.Y + nextDirection.Y;

        // Check if chosen move is within the bounds of the grid
        if (0 <= newX && newX < _layer.Width && 0 <= newY && newY < _layer.Height)
        {
            // Check if chosen move goes to a cell that is routable
            if (_layer.IsRoutable(newX, newY))
            {
                Position = new Position(newX, newY);
                _layer.StraightRunnerEnvironment.MoveTo(this, new Position(newX, newY));
                Console.WriteLine("{0} moved to a new cell: {1}", GetType().Name, Position);
            }
            else
            {
                Console.WriteLine("{0} tried to move to a blocked cell: ({1}, {2})", GetType().Name, newX, newY);
            }
        }
        else
        {
            Console.WriteLine("{0} tried to leave the world: ({1}, {2})", GetType().Name, newX, newY);
        }
    }

    #endregion

    #region Fields and Properties

    public Guid ID { get; set; }
    public Position Position { get; set; }
    private GridLayer _layer;
    private List<Position> _directions;
    private readonly Random _random = new();

    #endregion
}