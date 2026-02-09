using System.Text.Json;
using FrotaMan.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;

namespace FrotaMan.Services;

public class JsonPolymorphicService : IJsonPolymorphicService
{
    public string PrepareJson<T>(JsonElement rawJson)
    {
        if (!typeof(VehicleModel).IsAssignableFrom(typeof(T)))
        {
            throw new ArgumentException("Tipo inválido");
        }
        var attr = typeof(VehicleModel)
            .GetCustomAttribute<JsonPolymorphicAttribute>();
        
        var discriminatorKey = attr!.TypeDiscriminatorPropertyName;
        if(discriminatorKey == null || discriminatorKey.Count() <= 0)
        {
            throw new ArgumentException("Tipo inválido");
        }

        var rootNode = JsonNode.Parse(rawJson.GetRawText())?.AsObject();

        if (rootNode == null) return rawJson.GetRawText();

        if (rootNode.TryGetPropertyValue(discriminatorKey, out JsonNode? discriminatorNode))
        {
            var value = discriminatorNode?.DeepClone();
            rootNode.Remove(discriminatorKey);

            var reorderedObject = new JsonObject();
            reorderedObject.Add(discriminatorKey, value);

            foreach (var property in rootNode)
            {
                reorderedObject.Add(property.Key, property.Value?.DeepClone());
            }

            return reorderedObject.ToJsonString();
        }

        return rawJson.GetRawText();
    }
}