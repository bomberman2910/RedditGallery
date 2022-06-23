using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace RedditGallery;

public class SettingsController
{
    private const string SettingsFileName = "settings.json";

    private static SettingsController instance;
    private static readonly object PadLock = new();
    private readonly string applicationName;

    private readonly string baseFolderPath;

    private readonly JsonSerializerOptions defaultSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };

    private bool initialized;

    public static SettingsController Instance
    {
        get
        {
            lock (PadLock)
                return instance ??= new SettingsController();
        }
    }

    private Dictionary<string, string> Settings { get; set; }

    private SettingsController()
    {
        baseFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        if (string.IsNullOrEmpty(assemblyName))
            throw new Exception("Application name could not be retrieved. SettingsController cannot be initialized");
        applicationName = assemblyName;
        Settings = new Dictionary<string, string>();
    }

    public void Initialize()
    {
        var applicationFolderPath = Path.Combine(baseFolderPath, applicationName);
        var applicationFolderExists = Directory.Exists(applicationFolderPath);
        if (!applicationFolderExists)
            Directory.CreateDirectory(applicationFolderPath);
        var settingsFilePath = Path.Combine(applicationFolderPath, SettingsFileName);
        var settingsFileExists = File.Exists(settingsFilePath);
        if (!settingsFileExists)
        {
            var jsonString = JsonSerializer.Serialize(Settings, defaultSerializerOptions);
            File.WriteAllText(settingsFilePath, jsonString);
        }

        initialized = true;
    }

    public void Load()
    {
        if (!initialized)
            throw new Exception("SettingsController not initialized");
        var settingsFilePath = Path.Combine(baseFolderPath, applicationName, SettingsFileName);
        var jsonString = File.ReadAllText(settingsFilePath);
        Settings = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString, defaultSerializerOptions);
    }

    public void Save()
    {
        if (!initialized)
            throw new Exception("SettingsController not initialized");
        var settingsFilePath = Path.Combine(baseFolderPath, applicationName, SettingsFileName);
        var jsonString = JsonSerializer.Serialize(Settings, defaultSerializerOptions);
        File.WriteAllText(settingsFilePath, jsonString);
    }

    public void Insert(string name, object element)
    {
        var nameExists = Settings.ContainsKey(name);
        if (nameExists)
            throw new Exception($"Entry with name {name} already exists");

        if (element.GetType().IsPrimitive)
        {
            Settings.Add(name, element.ToString());
            return;
        }

        if (element is DateTime dateTime)
        {
            Settings.Add(name, dateTime.Ticks.ToString());
            return;
        }

        if (element is Guid guid)
        {
            Settings.Add(name, guid.ToString());
            return;
        }

        if (!element.GetType().IsClass)
            throw new Exception($"Type {element.GetType().FullName} is not supported");

        var serializedElement = JsonSerializer.Serialize(element, new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = false
        });
        Settings.Add(name, serializedElement);
    }

    public void Update(string name, object element)
    {
        var nameExists = Settings.ContainsKey(name);
        if (!nameExists)
            throw new Exception($"Entry with name {name} does not exist");

        if (element.GetType().IsPrimitive)
        {
            Settings[name] = element.ToString();
            return;
        }

        if (element is DateTime dateTime)
        {
            Settings[name] = dateTime.Ticks.ToString();
            return;
        }

        if (element is Guid guid)
        {
            Settings[name] = guid.ToString();
            return;
        }

        if (!element.GetType().IsClass)
            throw new Exception($"Type {element.GetType().FullName} is not supported");

        var serializedElement = JsonSerializer.Serialize(element, new JsonSerializerOptions
        {
            WriteIndented = false,
            PropertyNameCaseInsensitive = false
        });
        Settings[name] = serializedElement;
    }

    public T Read<T>(string name)
    {
        var nameExists = Settings.ContainsKey(name);
        if (!nameExists)
            throw new Exception($"Entry with name {name} does not exist");

        if (typeof(T).IsPrimitive)
            return (T)Convert.ChangeType(Settings[name], typeof(T));
        throw new Exception($"Cannot convert to type {typeof(T)}");
    }
}