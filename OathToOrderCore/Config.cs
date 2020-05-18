#region Header

// OathToOrder >OathToOrder >Config.cs\n Copyright (C) Adonis Deliannis, 2020\nCreated 18 04, 2020

#endregion

using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace OathToOrderCore {
    public partial class Config {
        [JsonProperty("weather-underground-ftp-url")]
        public string WeatherUndergroundFtpUri { get; set; }

        [JsonProperty("weather-underground-id")]
        public string WeatherUndergroundId { get; set; }

        [JsonProperty("weather-underground-key")]
        public string WeatherUndergroundKey { get; set; }

        [JsonProperty("camera-ip")]
        public string CameraIp { get; set; }

        [JsonProperty("api-version")]
        public string ApiVersion { get; set; }

        [JsonProperty("camera-username")]
        public string User { get; set; }

        [JsonProperty("camera-password")]
        public string Password { get; set; }
    }

    public partial class Config {
        private const string SettingsFile = "settings.json";
        private const string ErrorModifyConfig = "Modify Configuration file before running again.";

        private static readonly ILog _log = LogManager.GetLogger(typeof(Config));

        private static Config FromJson(string json) {
            return JsonConvert.DeserializeObject< Config >(json, Converter.Settings);
        }

        public static Config Load(string file = SettingsFile) {
            if (!File.Exists(file)) {
                new Config().Save(file);
                _log.Error(ErrorModifyConfig);
                Environment.Exit(-1);
            }

            using var reader = new StreamReader(file);
            return FromJson(reader.ReadToEnd());
        }

        public static async Task< Config > LoadAsync(string file = SettingsFile) {
            if (!File.Exists(file)) {
                await new Config().SaveAsync(file);
                _log.Error(ErrorModifyConfig);
                Environment.Exit(-1);
            }

            using var reader = new StreamReader(file);
            return FromJson(await reader.ReadToEndAsync());
        }

        public void Save(string file = SettingsFile) {
            using var writer = new StreamWriter(file);
            writer.WriteLine(this.ToJson());
        }

        public async Task SaveAsync(string file = SettingsFile) {
            using var writer = new StreamWriter(file);
            await writer.WriteLineAsync(this.ToJson());
        }
    }

    internal static class Converter {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Formatting = Formatting.Indented,
            Converters = {
                new IsoDateTimeConverter {DateTimeStyles = DateTimeStyles.AssumeUniversal}
            }
        };
    }
}