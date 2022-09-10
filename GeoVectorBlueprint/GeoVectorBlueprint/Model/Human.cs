using System;
using Mars.Components.Environments;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Environments;
using NetTopologySuite.Geometries;
using Position = Mars.Interfaces.Environments.Position;

namespace GeoVectorBlueprint.Model
{
    /// <summary>
    ///  A simple agent stub that has an Init() method for initialization and a
    ///  Tick() method for acting in every tick of the simulation.
    /// </summary>
    public class Human : IAgent<GraphLayer>, ISpatialGraphEntity
    {
        public void Init(GraphLayer layer)
        {
            RouteIndex = 0;
            GraphLayer = layer;
            
            // start and insert our self onto a random position on the graph
            var startNode = layer.Environment.GetRandomNode();
            Position = startNode.Position;
            layer.Environment.Insert(this, startNode);
            
            // find a destination
            Route = GetNewRoute();
        }

        private Route GetNewRoute()
        {
            // Increment counter to distinguish different Routes
            RouteIndex += 1;
            
            // get position of current where abouts and position of new destination.
            var myPosition = Position;

            // Find a random POI with the given OSM category (in this case "restaurant").
            // See the Prepare POIs Notebook for more categories.
            var poi = PoiLayer.GetRandomPoiForCategory("restaurant");

            // log the category / name of the new destination POI in the CSV output
            TargetPoiCategory = (string) poi.VectorStructured.Attributes["fclass"];
            TargetPoiName = (string) poi.VectorStructured.Attributes["name"];

            // get the Position from the POI
            var point = (Point) poi.VectorStructured.Geometry;
            var targetPosition = new Position(point.X, point.Y);

            // based on the Positions we need to find the corresponding "nodes" inside the graph, 
            // and create a path between them.
            var startNode = GraphLayer.Environment.NearestNode(myPosition);
            var goalNode = GraphLayer.Environment.NearestNode(targetPosition);

            return GraphLayer.Environment.FindShortestRoute(startNode, goalNode);
        }
        
        private Route Route { get;  set; }
        
        public GraphLayer GraphLayer { get; private set; }
        
        public PoiLayer PoiLayer { get; set; }

        public int RouteIndex { get; set; }
        public String TargetPoiCategory { get; set; }
        public String TargetPoiName { get; set; }
        public Position Position { get; set; }

        public void Tick()
        {
            // Get current time of simulation
            DateTime currentSimTime = GraphLayer.Context.CurrentTimePoint.GetValueOrDefault();

            if (Route.GoalReached)
            {
                Console.WriteLine("goal reached, start new route");
                Route = GetNewRoute();
            }
            else
            {
                // 2m per tick (implies 2m/s by a 1 tick / second) -> 2m/s -> 7.2km/h
                // in case we are near to our destination to the target might be <2m and the Move() would not "move"
                // the agent since we would overshoot the target. So we check if this is the case, and then use the 
                // remaining distance in the last tick.
                var distance = Math.Min(Route.RemainingRouteDistanceToGoal, 2);
                
                GraphLayer.Environment.Move(this, Route, distance);
                Position = this.CalculateNewPositionFor(Route, out var bearing);
            }
        }

        public Guid ID { get; set; } // identifies the agent
        public double Length { get; }
        public ISpatialEdge CurrentEdge { get; set; }
        public double PositionOnCurrentEdge { get; set; }
        public int LaneOnCurrentEdge { get; set; }
        public SpatialModalityType ModalityType { get; }
        public bool IsCollidingEntity { get; }
    }
}