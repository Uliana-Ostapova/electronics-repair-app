using System.Text.Json;
using System.Text.Json.Serialization;

public class RequestStorage
{
    private string _filePath; //путь к файлу в котором хранятся заявки
    private JsonSerializerOptions _options; //настройки сериализации и десериализации json

    public RequestStorage(string filePath) //задан путь к файлу при создании объекта хранилища
    {
        _filePath = filePath;
        _options = new JsonSerializerOptions();
        _options.WriteIndented = true;
        _options.Converters.Add(new JsonStringEnumConverter());
    }

    public List<RepairRequest> Load() //загружает заявки из json файла
    {
        if (!File.Exists(_filePath))
        {
            return new List<RepairRequest>();
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            return new List<RepairRequest>();
        }

        List<RepairRequest>? requests = JsonSerializer.Deserialize<List<RepairRequest>>(json, _options);

        if (requests == null)
        {
            return new List<RepairRequest>();
        }

        return requests;
    }

    public void Save(List<RepairRequest> requests) //сохраняет переданный список в json файл
    {
        string json = JsonSerializer.Serialize(requests, _options);

        File.WriteAllText(_filePath, json);
    }
}