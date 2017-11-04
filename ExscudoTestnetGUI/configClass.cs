﻿// Generated by Xamasoft JSON Class Generator
// http://www.xamasoft.com/json-class-generator

using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExscudoTestnetGUI
{

    internal partial class configClass
    {
        internal class Payouts2
        {

            [JsonProperty("enabled")]
            public bool Enabled { get; set; }

            [JsonProperty("interval")]
            public string Interval { get; set; }

            [JsonProperty("peer")]
            public string Peer { get; set; }

            [JsonProperty("seed")]
            public string Seed { get; set; }

            [JsonProperty("deadline")]
            public uint Deadline { get; set; }

            [JsonProperty("fee")]
            public uint Fee { get; set; }

            [JsonProperty("raw")]
            public bool Raw { get; set; }

            [JsonProperty("timeout")]
            public string Timeout { get; set; }

            [JsonProperty("threshold")]
            public long Threshold { get; set; }
        }
    }

    internal partial class configClass
    {

        [JsonProperty("threads")]
        public byte Threads { get; set; }

        [JsonProperty("coin")]
        public string Coin { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("upstreamCheckInterval")]
        public string UpstreamCheckInterval { get; set; }

        [JsonProperty("payouts")]
        public Payouts2 Payouts { get; set; }
    }

}
