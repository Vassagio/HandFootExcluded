using HandFootExcluded.Core.GameServices;
using HandFootExcluded.Core.PlayerServices;
using HandFootExcluded.UI.Services.ScoringServices;
using HandFootExcluded.UI.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HandFootExcluded.UI.Services;

public interface IJsonService
{
    Task SaveAsync<T>(T data, string fileName);
    Task<T?> LoadAsync<T>(string fileName);
}

internal sealed class JsonService : IJsonService
{
    
    private readonly string _appDataPath;

    public JsonService()
    {
         _appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }

    public async Task SaveAsync<T>(T data, string fileName)
    {
        var filePath = Path.Combine(_appDataPath, fileName);
        var options = CreateOptions();
        var json = JsonSerializer.Serialize(data, options);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<T?> LoadAsync<T>(string fileName)
    {
        try
        {
            var filePath = Path.Combine(_appDataPath, fileName);

            if (!File.Exists(filePath)) return default;

            var options = CreateOptions();
            var json = await File.ReadAllTextAsync(filePath);

            return JsonSerializer.Deserialize<T>(json, options);
        }
        catch (Exception ex)
        {
            return default(T);
        }
    }

    private static JsonSerializerOptions CreateOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull & JsonIgnoreCondition.WhenWritingDefault,
            Converters =
            {
                new JsonStringEnumConverter(),
                new GameConverter(),
                new PlayerSettingConverter(),
                new RoundViewModelConverter(),
                new TeamViewModelConverter(),
                new TotalScoreViewModelConverter(),
                new ScoreLinesConverter(),
                new BottomScoreLineConverter(),
                new BonusScoreLineConverter(),
                new CumulativeScoreLineConverter(),
                new GrandTotalScoreLineConverter(),
                new RoundTotalScoreLinesConverter(),
                new RoundTotalScoreLineConverter(),
                new TopScoreLineConverter()
            }
        };
    }
}