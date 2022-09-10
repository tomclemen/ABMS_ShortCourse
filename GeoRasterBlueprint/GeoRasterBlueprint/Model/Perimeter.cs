using System;
using Mars.Components.Layers;
using Mars.Interfaces.Environments;

namespace GeoRasterBlueprint.Model;

/// <summary>
/// A raster layer that ingests an .asc raster file with the walkable area by the agents.
/// </summary>
public class Perimeter : RasterLayer
{
 
    /// <summary>
    ///     Checks for the coordinate whether this point is inside the fence (the defined polygon).
    /// </summary>
    /// <param name="coordinate">The coordinate to check</param>
    /// <returns>
    ///     Returns true when the coordinate is inside the fence.
    /// </returns>
    public bool IsPointInside(Position coordinate)
    {
        // we first check if the coordinate is inside the area of the .asc file
        // the comparison with "1" ensures, that it's not a "unwalkable" field (the area outside of our polygon).
        // it's the NoData value we set previously in QGIS.
        return Extent.Contains(coordinate.X, coordinate.Y) && GetValue(coordinate) != 1;
    }
}