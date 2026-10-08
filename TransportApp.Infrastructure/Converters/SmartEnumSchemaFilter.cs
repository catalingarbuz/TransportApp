using Ardalis.SmartEnum;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text.Json.Nodes;

namespace TransportApp.Infrastructure.Converters;

/// <summary>
/// This class is used to make the swagger compatible with the smart enums.
/// </summary>
public sealed class SmartEnumSchemaFilter : ISchemaFilter
{
    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema is not OpenApiSchema openApiSchema)
        {
            return;
        }

        var type = context.Type;

        if (!IsTypeDerivedFromGenericType(type, typeof(SmartEnum<>)) &&
            !IsTypeDerivedFromGenericType(type, typeof(SmartEnum<,>)))
        {
            return;
        }

        var enumValues = type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                             .Select(d => d.Name);

        List<JsonNode> jsonEnumValues = new List<JsonNode>();
        foreach (var name in enumValues)
        {
            jsonEnumValues.Add(JsonValue.Create(name));
        }

        openApiSchema.Type = JsonSchemaType.String;
        openApiSchema.Enum = jsonEnumValues;
        openApiSchema.Properties = null;
        openApiSchema.AdditionalPropertiesAllowed = false;
    }

    private static bool IsTypeDerivedFromGenericType(Type typeToCheck, Type genericType)
    {
        while (true)
        {
            if (typeToCheck == typeof(object))
            {
                return false;
            }

            if (typeToCheck == null)
            {
                return false;
            }

            if (typeToCheck.IsGenericType && typeToCheck.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }

            if (typeToCheck.BaseType != null)
            {
                typeToCheck = typeToCheck.BaseType;
            }
            else
            {
                return false;
            }
        }
    }
}

