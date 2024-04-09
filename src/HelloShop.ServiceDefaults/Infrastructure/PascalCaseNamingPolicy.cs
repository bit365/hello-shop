using System.Text.Json;

namespace HelloShop.ServiceDefaults;

public class PascalCaseNamingPolicy : JsonNamingPolicy
{
    public static PascalCaseNamingPolicy PascalCase { get; } = new PascalCaseNamingPolicy();

    public override string ConvertName(string name) => string.Concat(char.ToUpper(name[0]), name[1..]);
}
