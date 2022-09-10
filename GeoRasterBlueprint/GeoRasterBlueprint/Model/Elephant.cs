using System;
using System.Linq;
using Mars.Common;
using Mars.Common.IO.Mapped;
using Mars.Interfaces.Agents;
using Mars.Interfaces.Annotations;
using Mars.Interfaces.Environments;
using NetTopologySuite.Geometries;
using Position = Mars.Interfaces.Environments.Position;

namespace GeoRasterBlueprint.Model
{
    /// <summary>
    ///  A simple agent stub that has an Init() method for initialization and a
    ///  Tick() method for acting in every tick of the simulation.
    /// </summary>
    public class Elephant : IAgent<LandscapeLayer>, IPositionable
    {
        [PropertyDescription]
        public double Latitude { get; set; }
        
        [PropertyDescription]
        public double Longitude { get; set; }
        
        public double Energy { get; set; }

        [PropertyDescription(Name = "WaterLayer")]
        public WaterLayer Water { get; set; }

        
        [PropertyDescription(Name = "Perimeter")]
        public Perimeter Perimeter { get; set; }
        
        public void Init(LandscapeLayer layer)
        {
            Layer = layer; // store layer for access within agent class
            Energy = 100;

            // Make sure we spawn our elephants only inside the perimeter
            var po = new Position(Longitude, Latitude);
            if (!Perimeter.IsPointInside(po))
            {
                throw new Exception("Start point is not inside perimeter.");
            }
            
            // Elephant is created at coordinates specified in input CSV file.
            Position = Position.CreateGeoPosition(Longitude, Latitude);
        }

        /// <summary>
        /// bearing of the elephant between 0 - 360deg
        /// </summary>
        private double bearing = 222.0;
        
        /// <summary>
        /// Distance in meters the elephant should move per tick.
        /// </summary>
        private double distance = 10.0;

        public void Tick()
        {
            Energy -= 1;
            
            // Create target position with current bearing and distance
            Position target = Position.CalculateRelativePosition(bearing, distance);
            
            // ...unless the energy level drops, search a water place
            if (Energy < 40)
            {
                var waterFeatures = Water.Explore(Position.PositionArray, 10000);
                if (waterFeatures.Any())
                {
                    // get coordinates of the water feature
                    var feature = waterFeatures.First();
                    var point = (Point) feature.VectorStructured.Geometry;
                    target = new Position(point.X, point.Y);
                    
                    // and change our bearing in the direction of the water feature
                    bearing = Position.GetBearing(target);
                    
                    // if we are in the vicinity of the water place, up our energy and change heading
                    if (target.DistanceInMTo(Position) < 20)
                    {
                        Energy += 5000;
                        bearing = (bearing + 45) % 360;
                    }
                } else
                {
                    Console.WriteLine("No water in area");                    
                }
            }
            else
            {
                // all 123 ticks just change the heading randomly
                if ((Layer.Context.CurrentTick % 123)  == 0)
                {
                    bearing = new Random().Next(0, 360);
                }
            }
            
            
            // Make sure the calculated target is still inside our perimeter!
            if (Perimeter.IsPointInside(target))
            {
                // If that's the case, move there
                Position = Layer.Environment.MoveTowards(this, bearing, distance);
            }
            else
            {
                // If not, we don't move and change our bearing to try a movement in the next tick
                bearing = (bearing + 45) % 360;
            }
        }
        
        private LandscapeLayer Layer { get; set; } // provides access to the main layer of this agent
        public Guid ID { get; set; } // identifies the agent
        public Position Position { get; set; }
        
        public Position Target { get; set; }
    }
}