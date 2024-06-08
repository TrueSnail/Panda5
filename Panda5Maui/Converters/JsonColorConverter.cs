using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Panda5Maui.Converters;

public class JsonColorConverter : JsonConverter<Color>
{

    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert.Name == nameof(Color)) return true;
        else return false;
    }

    public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        List<double> colorChannels = [];
        while (reader.Read()) 
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Number:
                    colorChannels.Add(reader.GetDouble());
                    break;
                case JsonTokenType.EndObject:
                    return Color.FromRgb(colorChannels[0], colorChannels[1], colorChannels[2]);
                default:
                    break;
            }
        }
        throw new Exception("JsonColorConverter read too much");
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Red", value.Red);
        writer.WriteNumber("Green", value.Green);
        writer.WriteNumber("Blue", value.Blue);
        writer.WriteEndObject();
    }
}

