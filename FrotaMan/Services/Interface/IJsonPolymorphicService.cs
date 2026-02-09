
using System.Text.Json;

namespace FrotaMan.Services;

public interface IJsonPolymorphicService
{
    string PrepareJson<T>(JsonElement rawJson);
}