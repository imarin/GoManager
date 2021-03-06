﻿using Newtonsoft.Json;
using POGOProtos.Enums;
using POGOProtos.Inventory.Item;
using PokemonGo.RocketAPI;
using PokemonGo.RocketAPI.Enums;
using PokemonGoGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PokemonGoGUI
{
    public class Settings : ISettings
    {
        public string AccountName { get; set; }
        public AuthType AuthType { get; set; }
        public string PtcUsername { get; set; }
        public string PtcPassword { get; set; }
        public string GoogleRefreshToken { get; set; }
        public string GooglePassword { get; set; }
        public string GoogleUsername { get; set; }
        public double DefaultLatitude { get; set; }
        public double DefaultLongitude { get; set; }
        public double DefaultAltitude { get; set; }
        public bool MimicWalking { get; set; }
        public int WalkingSpeed { get; set; }
        public bool EncounterWhileWalking { get; set; }
        public int MaxTravelDistance { get; set; }
        public bool UseLuckyEgg { get; set; }
        public int MinPokemonBeforeEvolve { get; set; }
        public bool RecycleItems { get; set; }
        public bool TransferPokemon { get; set; }
        public bool EvolvePokemon { get; set; }
        public bool CatchPokemon { get; set; }
        public bool IncubateEggs { get; set; }
        public bool SnipePokemon { get; set; }
        public int MaxLevel { get; set; }
        public int SnipeAfterPokestops { get; set; }

        public string DeviceId { get; set; }
        public string AndroidBoardName { get; set; }
        public string AndroidBootloader { get; set; }
        public string DeviceBrand { get; set; }
        public string DeviceModel { get; set; }
        public string DeviceModelIdentifier { get; set; }
        public string DeviceModelBoot { get; set; }
        public string HardwareManufacturer { get; set; }
        public string HardwareModel { get; set; }
        public string FirmwareBrand { get; set; }
        public string FirmwareTags { get; set; }
        public string FirmwareType { get; set; }
        public string FirmwareFingerprint { get; set; }

        public string ProxyIP { get; set; }
        public int ProxyPort { get; set; }
        public string ProxyUsername { get; set; }
        public string ProxyPassword { get; set; }

        public ProxyEx Proxy
        {
            get
            {
                return new ProxyEx
                {
                    Address = ProxyIP,
                    Port = ProxyPort,
                    Username = ProxyUsername,
                    Password = ProxyPassword
                };
            }
        }

        public List<InventoryItemSetting> ItemSettings { get; set; }
        public List<CatchSetting> CatchSettings { get; set; }
        public List<EvolveSetting> EvolveSettings { get; set; }
        public List<TransferSetting> TransferSettings { get; set; }
        //public List<CatchSetting> SniperSettings { get; set; }

        [JsonConstructor]
        public Settings(bool jsonConstructor = true)
        {
            LoadDefaults();
        }

        public Settings()
        {
            //Defaults
            LoadDefaults();

            ItemSettings = new List<InventoryItemSetting>();
            CatchSettings = new List<CatchSetting>();

            LoadDeviceSettings();
            LoadInventorySettings();
            LoadCatchSettings();
            LoadEvolveSettings();
            LoadTransferSettings();
        }

        public void LoadDefaults()
        {
            AuthType = AuthType.Ptc;
            GoogleRefreshToken = String.Empty;
            DefaultLatitude = -33.870225;
            DefaultLongitude = 151.208343;
            MimicWalking = true;
            CatchPokemon = true;
            SnipePokemon = true;
            WalkingSpeed = 7;
            MaxTravelDistance = 1000;
            EncounterWhileWalking = false;
            MinPokemonBeforeEvolve = 100;
            SnipeAfterPokestops = 5;
        }

        public void LoadDeviceSettings()
        {
            DeviceId = RandomString(16);
            AndroidBoardName = "universal7420";
            AndroidBootloader = "universal7420";
            DeviceBrand = "universal7420";
            DeviceModel = "zeroflte";
            DeviceModelIdentifier = "SM-G920F";
            DeviceModelBoot = "qcom";
            HardwareManufacturer = "samsung";
            HardwareModel = "SM-G920F";
            FirmwareBrand = "zerofltexx";
            FirmwareTags = "release-keys";
            FirmwareFingerprint = "samsung/zerofltexx/zeroflte:6.0.1/MMB29K/G920FXXS3DPD2:user/release-keys";
            FirmwareType = "user";
        }

        public void LoadCatchSettings()
        {
            CatchSettings = new List<CatchSetting>();

            foreach(PokemonId pokemon in Enum.GetValues(typeof(PokemonId)))
            {
                if(pokemon == PokemonId.Missingno)
                {
                    continue;
                }

                CatchSetting cSettings = new CatchSetting
                {
                    Id = pokemon
                };

                CatchSettings.Add(cSettings);
            }
        }

        public void LoadInventorySettings()
        {
            ItemSettings = new List<InventoryItemSetting>();

            foreach(ItemId item in Enum.GetValues(typeof(ItemId)))
            {
                if(item == ItemId.ItemUnknown)
                {
                    continue;
                }

                InventoryItemSetting itemSetting = new InventoryItemSetting
                {
                    Id = item
                };

                ItemSettings.Add(itemSetting);
            }
        }

        public void LoadEvolveSettings()
        {
            EvolveSettings = new List<EvolveSetting>();

            foreach (PokemonId pokemon in Enum.GetValues(typeof(PokemonId)))
            {
                if (pokemon == PokemonId.Missingno)
                {
                    continue;
                }

                EvolveSetting setting = new EvolveSetting
                {
                    Id = pokemon
                };

                EvolveSettings.Add(setting);
            }
        }

        public void LoadTransferSettings()
        {
            TransferSettings = new List<TransferSetting>();

            foreach (PokemonId pokemon in Enum.GetValues(typeof(PokemonId)))
            {
                if (pokemon == PokemonId.Missingno)
                {
                    continue;
                }

                TransferSetting setting = new TransferSetting
                {
                    Id = pokemon
                };

                TransferSettings.Add(setting);
            }
        }

        /*
        public void LoadSniperSettings()
        {
            SniperSettings = new List<CatchSetting>();

            foreach (PokemonId pokemon in Enum.GetValues(typeof(PokemonId)))
            {
                if (pokemon == PokemonId.Missingno)
                {
                    continue;
                }

                CatchSetting setting = new CatchSetting
                {
                    Id = pokemon,
                    Catch = true
                };

                SniperSettings.Add(setting);
            }
        }
        */

        private string RandomString(int length, string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789")
        {
            var outOfRange = Byte.MaxValue + 1 - (Byte.MaxValue + 1) % alphabet.Length;

            return string.Concat(
                Enumerable
                    .Repeat(0, Int32.MaxValue)
                    .Select(e => this.RandomByte())
                    .Where(randomByte => randomByte < outOfRange)
                    .Take(length)
                    .Select(randomByte => alphabet[randomByte % alphabet.Length])
            );
        }

        private byte RandomByte()
        {
            using (var randomizationProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[1];
                randomizationProvider.GetBytes(randomBytes);
                return randomBytes.Single();
            }
        }
    }
}
