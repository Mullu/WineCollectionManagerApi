using System.Text.Json.Serialization;

namespace WineCollectionManagerApi.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WineStyle
    {
        Dry,
        Sweet,
        SemiDry,
        SemiSweet,
        Sparkling,
        Fortified
    }
}
