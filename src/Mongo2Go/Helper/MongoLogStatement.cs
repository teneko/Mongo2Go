﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mongo2Go.Helper
{
    /// <summary>
    /// Structure of a log generated by mongod. Used to deserialize the logs
    /// and pass them to an ILogger.
    /// See: https://docs.mongodb.com/manual/reference/log-messages/#json-log-output-format
    /// Note: "truncated" and "size" are not parsed as we're unsure how to
    /// properly parse and use them.
    /// </summary>
    class MongoLogStatement
    {
        [JsonPropertyName("t")]
        public MongoDate MongoDate { get; set; }

        /// <summary>
        /// Severity of the logs as defined by MongoDB. Mapped to LogLevel
        /// as defined by Microsoft.
        /// D1-D2 mapped to Debug level. D3-D5 mapped Trace level.
        /// </summary>
        [JsonPropertyName("s")]
        public string Severity { get; set; }

        public LogLevel Level
        {
            get
            {
                if (string.IsNullOrEmpty(Severity))
                    return LogLevel.None;
                switch (Severity)
                {
                    case "F": return LogLevel.Critical;
                    case "E": return LogLevel.Error;
                    case "W": return LogLevel.Warning;
                    case "I": return LogLevel.Information;
                    case "D":
                    case "D1":
                    case "D2":
                        return LogLevel.Debug;
                    case "D3":
                    case "D4":
                    case "D5":
                    default:
                        return LogLevel.Trace;
                }
            }
        }

        [JsonPropertyName("c")]
        public string Component { get; set; }

        [JsonPropertyName("ctx")]
        public string Context { get; set; }

        [JsonPropertyName("id")]
        public int? Id { get; set; }

        [JsonPropertyName("msg")]
        public string Message { get; set; }

        [JsonPropertyName("tags")]
        public IEnumerable<string> Tags { get; set; }

        [JsonPropertyName("attr")]
        public IDictionary<string, JsonElement> Attributes { get; set; }
    }
    class MongoDate
    {
        [JsonPropertyName("$date")]
        public DateTime DateTime { get; set; }
    }
}