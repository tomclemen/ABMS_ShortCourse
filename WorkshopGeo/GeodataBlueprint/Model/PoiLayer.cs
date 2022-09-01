using System;
using System.Linq;
using Mars.Components.Layers;
using Mars.Interfaces.Layers;

namespace GeodataBlueprint.Model;

public class PoiLayer : VectorLayer
{
    
    public IVectorFeature GetRandomPoiForCategory (string category)
    {
        // Shuffle all available features randomly so we get a chance for a
        // new POI every time we call the function
        var rng = new Random();
        var shuffledFeatures = Features.OrderBy(a => rng.Next()).ToList();
        
        foreach (var feature in shuffledFeatures)
        {
            if ((string)feature.VectorStructured.Attributes["fclass"] == category)
            {
                return feature;
            }
        }
        
        // if we reach this code, no POI with this category is available -> abort the simulation
        throw new ArgumentException($"No POIs found with category '{category}");
    }
}