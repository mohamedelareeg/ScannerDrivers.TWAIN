using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Generic;
using ScannerDrivers.TWAIN.Contants.Structure;
using ScannerDrivers.TWAIN.Contants.Type;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a string into something we can poke into a
        /// TW_CAPABILITY structure...
        /// </summary>
        /// <param name="a_twcapability">A TWAIN structure</param>
        /// <param name="a_szSetting">A CSV string of the TWAIN structure</param>
        /// <param name="a_szValue">The container for this capability</param>
        /// <returns>True if the conversion is successful</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public bool CsvToCapability(ref TW_CAPABILITY a_twcapability, ref string a_szSetting, string a_szValue)
        {
            int ii = 0;
            TWTY twty = TWTY.BOOL;
            uint u32NumItems = 0;
            IntPtr intptr = IntPtr.Zero;
            string szResult;
            string[] asz;

            // We need some protection for this one...
            try
            {
                // Tokenize our values...
                asz = CSV.Parse(a_szValue);
                if (asz.GetLength(0) < 1)
                {
                    a_szSetting = "Set Capability: (insufficient number of arguments)";
                    return (false);
                }

                // Set the capability from text or hex...
                try
                {
                    // see if it is a custom cap
                    if ((asz[0][0] == '8') || asz[0].StartsWith("0x8"))
                    {
                        a_twcapability.Cap = (Capabilities)0xFFFF;
                    }
                    else if (asz[0].StartsWith("0x"))
                    {
                        int iNum = 0;
                        int.TryParse(asz[0].Substring(2), NumberStyles.AllowHexSpecifier | NumberStyles.HexNumber, CultureInfo.InvariantCulture, out iNum);
                        a_twcapability.Cap = (Capabilities)iNum;
                    }
                    else
                    {
                        a_twcapability.Cap = (Capabilities)Enum.Parse(typeof(Capabilities), asz[0], true);
                    }
                }
                catch
                {
                    // don't log this exception...
                    a_twcapability.Cap = (Capabilities)0xFFFF;
                }
                if ((a_twcapability.Cap == (Capabilities)0xFFFF) || !asz[0].Contains("_"))
                {
                    a_twcapability.Cap = (Capabilities)Convert.ToUInt16(asz[0], 16);
                }

                // Set the container from text or decimal...
                if (asz.GetLength(0) >= 2)
                {
                    try
                    {
                        a_twcapability.ConType = (TWON)Enum.Parse(typeof(TWON), asz[1].Replace("TWON_", "").Replace("twon_", ""), true);
                    }
                    catch
                    {
                        // don't log this exception...
                        a_twcapability.ConType = (TWON)ushort.Parse(asz[1]);
                    }
                }

                // Set the item type from text or decimal...
                if (asz.GetLength(0) >= 3)
                {
                    try
                    {
                        twty = (TWTY)Enum.Parse(typeof(TWTY), asz[2].Replace("TWTY_", "").Replace("twty_", ""), true);
                    }
                    catch
                    {
                        // don't log this exception...
                        twty = (TWTY)ushort.Parse(asz[2]);
                    }
                }

                // Assign the new value...
                if (asz.GetLength(0) >= 4)
                {
                    switch (a_twcapability.ConType)
                    {
                        default:
                            a_szSetting = "(unrecognized container)";
                            return (false);

                        case TWON.ARRAY:
                            // Validate...
                            if (asz.GetLength(0) < 4)
                            {
                                a_szSetting = "Set Capability: (insufficient number of arguments)";
                                return (false);
                            }

                            // Get the values...
                            u32NumItems = uint.Parse(asz[3]);

                            // Allocate the container (go for worst case, which is TW_STR255)...
                            if (ms_platform == Platform.MACOSX)
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ARRAY_MACOSX)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ARRAY_MACOSX twarraymacosx = default(TW_ARRAY_MACOSX);
                                twarraymacosx.ItemType = (uint)twty;
                                twarraymacosx.NumItems = u32NumItems;
                                Marshal.StructureToPtr(twarraymacosx, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twarraymacosx));
                            }
                            else
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ARRAY)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ARRAY twarray = default(TW_ARRAY);
                                twarray.ItemType = twty;
                                twarray.NumItems = u32NumItems;
                                Marshal.StructureToPtr(twarray, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twarray));
                            }

                            // Set the ItemList...
                            for (ii = 0; (ii < u32NumItems) && ((ii + 4) < asz.Length); ii++)
                            {
                                szResult = SetIndexedItem(a_twcapability, twty, intptr, ii, asz[ii + 4]);
                                if (szResult != "")
                                {
                                    return (false);
                                }
                            }

                            // All done...
                            DsmMemUnlock(a_twcapability.hContainer);
                            return (true);

                        case TWON.ENUMERATION:
                            // Validate...
                            if (asz.GetLength(0) < 6)
                            {
                                a_szSetting = "Set Capability: (insufficient number of arguments)";
                                return (false);
                            }

                            // Get the values...
                            u32NumItems = uint.Parse(asz[3]);

                            // Allocate the container (go for worst case, which is TW_STR255)...
                            if (ms_platform == Platform.MACOSX)
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION_MACOSX)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ENUMERATION_MACOSX twenumerationmacosx = default(TW_ENUMERATION_MACOSX);
                                twenumerationmacosx.ItemType = (uint)twty;
                                twenumerationmacosx.NumItems = u32NumItems;
                                twenumerationmacosx.CurrentIndex = uint.Parse(asz[4]);
                                twenumerationmacosx.DefaultIndex = uint.Parse(asz[5]);
                                Marshal.StructureToPtr(twenumerationmacosx, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twenumerationmacosx));
                            }
                            // Windows or the 2.4+ Linux DSM...
                            else if ((ms_platform == Platform.WINDOWS) || ((m_linuxdsm == LinuxDsm.IsLatestDsm) || ((m_blFoundLatestDsm || m_blFoundLatestDsm64) && (m_linuxdsm == LinuxDsm.IsLatestDsm))))
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ENUMERATION twenumeration = default(TW_ENUMERATION);
                                twenumeration.ItemType = twty;
                                twenumeration.NumItems = u32NumItems;
                                twenumeration.CurrentIndex = uint.Parse(asz[4]);
                                twenumeration.DefaultIndex = uint.Parse(asz[5]);
                                Marshal.StructureToPtr(twenumeration, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twenumeration));
                            }
                            // The -2.3 Linux DSM...
                            else
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ENUMERATION_LINUX64)) + (((int)u32NumItems + 1) * Marshal.SizeOf(default(TW_STR255)))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ENUMERATION_LINUX64 twenumerationlinux64 = default(TW_ENUMERATION_LINUX64);
                                twenumerationlinux64.ItemType = twty;
                                twenumerationlinux64.NumItems = u32NumItems;
                                twenumerationlinux64.CurrentIndex = uint.Parse(asz[4]);
                                twenumerationlinux64.DefaultIndex = uint.Parse(asz[5]);
                                Marshal.StructureToPtr(twenumerationlinux64, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twenumerationlinux64));
                            }

                            // Set the ItemList...
                            for (ii = 0; ii < u32NumItems; ii++)
                            {
                                szResult = SetIndexedItem(a_twcapability, twty, intptr, ii, asz[ii + 6]);
                                if (szResult != "")
                                {
                                    return (false);
                                }
                            }

                            // All done...
                            DsmMemUnlock(a_twcapability.hContainer);
                            return (true);

                        case TWON.ONEVALUE:
                            // Validate...
                            if (asz.GetLength(0) < 4)
                            {
                                a_szSetting = "Set Capability: (insufficient number of arguments)";
                                return (false);
                            }

                            // Allocate the container (go for worst case, which is TW_STR255)...
                            if (ms_platform == Platform.MACOSX)
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ONEVALUE_MACOSX)) + Marshal.SizeOf(default(TW_STR255))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ONEVALUE_MACOSX twonevaluemacosx = default(TW_ONEVALUE_MACOSX);
                                twonevaluemacosx.ItemType = (uint)twty;
                                Marshal.StructureToPtr(twonevaluemacosx, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twonevaluemacosx));
                            }
                            else
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_ONEVALUE)) + Marshal.SizeOf(default(TW_STR255))));
                                intptr = DsmMemLock(a_twcapability.hContainer);

                                // Set the meta data...
                                TW_ONEVALUE twonevalue = default(TW_ONEVALUE);
                                twonevalue.ItemType = twty;
                                Marshal.StructureToPtr(twonevalue, intptr, true);

                                // Get the pointer to the ItemList...
                                intptr = (IntPtr)((UInt64)intptr + (UInt64)Marshal.SizeOf(twonevalue));
                            }

                            // Set the Item...
                            szResult = SetIndexedItem(a_twcapability, twty, intptr, 0, asz[3]);
                            if (szResult != "")
                            {
                                return (false);
                            }

                            // All done...
                            DsmMemUnlock(a_twcapability.hContainer);
                            return (true);

                        case TWON.RANGE:
                            // Validate...
                            if (asz.GetLength(0) < 8)
                            {
                                a_szSetting = "Set Capability: (insufficient number of arguments)";
                                return (false);
                            }

                            // Allocate the container (go for worst case, which is TW_STR255)...
                            if (ms_platform == Platform.MACOSX)
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_RANGE_MACOSX))));
                                intptr = DsmMemLock(a_twcapability.hContainer);
                            }
                            // Windows or the 2.4+ Linux DSM...
                            else if ((ms_platform == Platform.WINDOWS) || ((m_linuxdsm == LinuxDsm.IsLatestDsm) || ((m_blFoundLatestDsm || m_blFoundLatestDsm64) && (m_linuxdsm == LinuxDsm.IsLatestDsm))))
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_RANGE))));
                                intptr = DsmMemLock(a_twcapability.hContainer);
                            }
                            // The -2.3 Linux DSM...
                            else
                            {
                                // Allocate...
                                a_twcapability.hContainer = DsmMemAlloc((uint)(Marshal.SizeOf(default(TW_RANGE_LINUX64))));
                                intptr = DsmMemLock(a_twcapability.hContainer);
                            }

                            // Set the Item...
                            szResult = SetRangeItem(twty, intptr, asz);
                            if (szResult != "")
                            {
                                return (false);
                            }

                            // All done...
                            DsmMemUnlock(a_twcapability.hContainer);
                            return (true);
                    }
                }

                // All done (this is good for a get where only the cap was specified)...
                return (true);
            }
            catch (Exception exception)
            {
                Log.Error("CsvToCapability exception - " + exception.Message);
                Log.Error("setting=<" + a_szSetting + ">");
                a_szValue = "(data error)";
                return (false);
            }
        }
    }
}
