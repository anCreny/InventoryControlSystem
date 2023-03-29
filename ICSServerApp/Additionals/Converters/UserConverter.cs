using System.Text.Json;
using System.Text.Json.Serialization;
using ICSServerApp.Models.Database;

namespace ICSServerApp.Additionals.Converters;

public class UserConverter : JsonConverter<User>
{
    public override User? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var user = new User();
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();
                switch (propertyName?.ToLower())
                {
                    case "FullName":
                        user.FullName = reader.GetString();
                        break;
                    case "Login":
                        user.Login = reader.GetString();
                        break;
                    case "Password":
                        user.Password = reader.GetString();
                        break;
                    case "AccessRight":
                        user.AccessRight = reader.GetString();
                        break;
                }
            }
        }

        return user;
    }

    public override void Write(Utf8JsonWriter writer, User value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("Id", value.Id);
        writer.WriteString("FullName", value.FullName);
        writer.WriteString("Login", value.Login);
        writer.WriteString("Password", value.Password);
        writer.WriteString("AccessRight", value.AccessRight);
        writer.WriteEndObject();
    }
}