﻿using System.IO;

namespace PokemonGoGUI.Extensions
{
    public static class LargeAddressAware
    {
        public static bool IsLargeAware(string file)
        {
            using (var fs = File.OpenRead(file))
            {
                return IsLargeAware(fs);
            }
        }
        /// <summary>
        /// Checks if the stream is a MZ header and if it is large address aware
        /// </summary>
        /// <param name="stream">Stream to check, make sure its at the start of the MZ header</param>
        /// <exception cref=""></exception>
        /// <returns></returns>
        public static bool IsLargeAware(Stream stream)
        {
            const int IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x20;

            var br = new BinaryReader(stream);

            if (br.ReadInt16() != 0x5A4D)       //No MZ Header
                return false;

            br.BaseStream.Position = 0x3C;
            var peloc = br.ReadInt32();         //Get the PE header location.

            br.BaseStream.Position = peloc;
            if (br.ReadInt32() != 0x4550)       //No PE header
                return false;

            br.BaseStream.Position += 0x12;
            return (br.ReadInt16() & IMAGE_FILE_LARGE_ADDRESS_AWARE) == IMAGE_FILE_LARGE_ADDRESS_AWARE;
        }
    }
}
