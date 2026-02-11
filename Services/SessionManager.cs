using System.Text.Json;

namespace wildcat_one_windows.Services;

public class SessionManager
{
    public static SessionManager Instance { get; } = new();

    private readonly Dictionary<string, object?> _state = new()
    {
        ["token"] = null,
        ["userData"] = null,
        ["studentInfo"] = null,
        ["academicYears"] = null,
        ["currentAcademicYearId"] = null,
        ["currentAcademicYearName"] = null,
        ["currentTermId"] = null,
        ["currentTermName"] = null,
        ["availableTerms"] = null,
    };

    private SessionManager() { }

    public T? Get<T>(string key)
    {
        if (!_state.TryGetValue(key, out var value) || value is null)
            return default;

        if (value is T typed)
            return typed;

        if (value is JsonElement element)
        {
            var deserialized = element.Deserialize<T>();
            if (deserialized is not null)
            {
                _state[key] = deserialized;
                return deserialized;
            }
        }

        return default;
    }

    public void Set(string key, object? value)
    {
        _state[key] = value;
    }

    public void Reset()
    {
        foreach (var key in _state.Keys.ToList())
            _state[key] = null;
    }

    public string? Token
    {
        get => Get<string>("token");
        set => Set("token", value);
    }

    public JsonElement? UserData
    {
        get
        {
            if (_state.TryGetValue("userData", out var value) && value is JsonElement el)
                return el;
            return null;
        }
        set => Set("userData", value);
    }

    public bool HasValidSession()
    {
        return Token is not null
            && _state["userData"] is not null
            && _state["studentInfo"] is not null
            && _state["academicYears"] is not null
            && _state["currentAcademicYearId"] is not null
            && _state["currentTermId"] is not null;
    }
}
