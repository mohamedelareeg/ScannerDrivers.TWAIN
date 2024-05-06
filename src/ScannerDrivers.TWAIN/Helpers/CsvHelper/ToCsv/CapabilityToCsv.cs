using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Generic;
using ScannerDrivers.TWAIN.Contants.Structure;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        /// <summary>
        /// Convert the contents of a capability to a string that we can show in
        /// our simple GUI....
        /// </summary>
        /// <param name="a_twcapability">A TWAIN structure</param>
        /// <returns>A CSV string of the TWAIN structure</returns>
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust", Unrestricted = false)]
        public string CapabilityToCsv(TW_CAPABILITY a_twcapability, bool a_blUseSymbols)
        {
            IntPtr intptr;
            IntPtr intptrLocked;
            TWTY ItemType;
            uint NumItems;

            // Handle the container...
            switch (a_twcapability.ConType)
            {
                default:
                    return ("(unrecognized container)");

                case TWON.ARRAY:
                    {
                        uint uu;
                        CSV csvArray;

                        // Mac has a level of indirection and a different structure (ick)...
                        if (ms_platform == Platform.MACOSX)
                        {
                            // Crack the container...
                            TW_ARRAY_MACOSX twarraymacosx = default(TW_ARRAY_MACOSX);
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twarraymacosx = (TW_ARRAY_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_ARRAY_MACOSX));
                            ItemType = (TWTY)twarraymacosx.ItemType;
                            NumItems = twarraymacosx.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twarraymacosx));
                        }
                        else
                        {
                            // Crack the container...
                            TW_ARRAY twarray = default(TW_ARRAY);
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twarray = (TW_ARRAY)Marshal.PtrToStructure(intptrLocked, typeof(TW_ARRAY));
                            ItemType = twarray.ItemType;
                            NumItems = twarray.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twarray));
                        }

                        // Start building the string...
                        csvArray = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);
                        csvArray.Add(NumItems.ToString());

                        // Tack on the stuff from the ItemList...
                        if (a_blUseSymbols)
                        {
                            string szValue;
                            for (uu = 0; uu < NumItems; uu++)
                            {
                                string szItem = GetIndexedItem(a_twcapability, ItemType, intptr, (int)uu);
                                szValue = CvtCapValueToEnum(a_twcapability.Cap, szItem);
                                csvArray.Add(szValue);
                            }
                        }
                        else
                        {
                            for (uu = 0; uu < NumItems; uu++)
                            {
                                csvArray.Add(GetIndexedItem(a_twcapability, ItemType, intptr, (int)uu));
                            }
                        }

                        // All done...
                        DsmMemUnlock(a_twcapability.hContainer);
                        return (csvArray.Get());
                    }

                case TWON.ENUMERATION:
                    {
                        uint uu;
                        CSV csvEnum;

                        // Mac has a level of indirection and a different structure (ick)...
                        if (ms_platform == Platform.MACOSX)
                        {
                            // Crack the container...
                            TW_ENUMERATION_MACOSX twenumerationmacosx = default(TW_ENUMERATION_MACOSX);
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twenumerationmacosx = (TW_ENUMERATION_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_ENUMERATION_MACOSX));
                            ItemType = (TWTY)twenumerationmacosx.ItemType;
                            NumItems = twenumerationmacosx.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twenumerationmacosx));

                            // Start building the string...
                            csvEnum = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);
                            csvEnum.Add(NumItems.ToString());
                            csvEnum.Add(twenumerationmacosx.CurrentIndex.ToString());
                            csvEnum.Add(twenumerationmacosx.DefaultIndex.ToString());
                        }
                        // Windows or the 2.4+ Linux DSM...
                        else if ((ms_platform == Platform.WINDOWS) || ((m_blFoundLatestDsm || m_blFoundLatestDsm64) && (m_linuxdsm == LinuxDsm.IsLatestDsm)))
                        {
                            // Crack the container...
                            TW_ENUMERATION twenumeration = default(TW_ENUMERATION);
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twenumeration = (TW_ENUMERATION)Marshal.PtrToStructure(intptrLocked, typeof(TW_ENUMERATION));
                            ItemType = twenumeration.ItemType;
                            NumItems = twenumeration.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twenumeration));

                            // Start building the string...
                            csvEnum = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);
                            csvEnum.Add(NumItems.ToString());
                            csvEnum.Add(twenumeration.CurrentIndex.ToString());
                            csvEnum.Add(twenumeration.DefaultIndex.ToString());
                        }
                        // The -2.3 Linux DSM...
                        else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                        {
                            // Crack the container...
                            TW_ENUMERATION_LINUX64 twenumerationlinux64 = default(TW_ENUMERATION_LINUX64);
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twenumerationlinux64 = (TW_ENUMERATION_LINUX64)Marshal.PtrToStructure(intptrLocked, typeof(TW_ENUMERATION_LINUX64));
                            ItemType = twenumerationlinux64.ItemType;
                            NumItems = (uint)twenumerationlinux64.NumItems;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twenumerationlinux64));

                            // Start building the string...
                            csvEnum = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);
                            csvEnum.Add(NumItems.ToString());
                            csvEnum.Add(twenumerationlinux64.CurrentIndex.ToString());
                            csvEnum.Add(twenumerationlinux64.DefaultIndex.ToString());
                        }
                        // This shouldn't be possible, but what the hey...
                        else
                        {
                            Log.Error("This is serious, you win a cookie for getting here...");
                            DsmMemUnlock(a_twcapability.hContainer);
                            return ("");
                        }

                        // Tack on the stuff from the ItemList...
                        if (a_blUseSymbols)
                        {
                            string szValue;
                            for (uu = 0; uu < NumItems; uu++)
                            {
                                string szItem = GetIndexedItem(a_twcapability, ItemType, intptr, (int)uu);
                                szValue = CvtCapValueToEnum(a_twcapability.Cap, szItem);
                                csvEnum.Add(szValue);
                            }
                        }
                        else
                        {
                            for (uu = 0; uu < NumItems; uu++)
                            {
                                csvEnum.Add(GetIndexedItem(a_twcapability, ItemType, intptr, (int)uu));
                            }
                        }

                        // All done...
                        DsmMemUnlock(a_twcapability.hContainer);
                        return (csvEnum.Get());
                    }

                case TWON.ONEVALUE:
                    {
                        CSV csvOnevalue;

                        // Mac has a level of indirection and a different structure (ick)...
                        if (ms_platform == Platform.MACOSX)
                        {
                            // Crack the container...
                            TW_ONEVALUE_MACOSX twonevaluemacosx = default(TW_ONEVALUE_MACOSX);
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twonevaluemacosx = (TW_ONEVALUE_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_ONEVALUE_MACOSX));
                            ItemType = (TWTY)twonevaluemacosx.ItemType;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twonevaluemacosx));
                        }
                        else
                        {
                            // Crack the container...
                            TW_ONEVALUE twonevalue = default(TW_ONEVALUE);
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twonevalue = (TW_ONEVALUE)Marshal.PtrToStructure(intptrLocked, typeof(TW_ONEVALUE));
                            ItemType = (TWTY)twonevalue.ItemType;
                            intptr = (IntPtr)((UInt64)intptrLocked + (UInt64)Marshal.SizeOf(twonevalue));
                        }

                        // Start building the string...
                        csvOnevalue = Common(a_twcapability.Cap, a_twcapability.ConType, ItemType);

                        // Tack on the stuff from the Item...
                        if (a_blUseSymbols)
                        {
                            string szValue;
                            string szItem = GetIndexedItem(a_twcapability, ItemType, intptr, 0);
                            szValue = CvtCapValueToEnum(a_twcapability.Cap, szItem);
                            csvOnevalue.Add(szValue);
                        }
                        else
                        {
                            csvOnevalue.Add(GetIndexedItem(a_twcapability, ItemType, intptr, 0));
                        }

                        // All done...
                        DsmMemUnlock(a_twcapability.hContainer);
                        return (csvOnevalue.Get());
                    }

                case TWON.RANGE:
                    {
                        CSV csvRange;
                        string szTmp;
                        TW_RANGE twrange;
                        TW_RANGE_LINUX64 twrangelinux64;
                        TW_RANGE_MACOSX twrangemacosx;
                        TW_RANGE_FIX32 twrangefix32;
                        TW_RANGE_FIX32_MACOSX twrangefix32macosx;

                        // Mac has a level of indirection and a different structure (ick)...
                        twrange = default(TW_RANGE);
                        if (ms_platform == Platform.MACOSX)
                        {
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twrangemacosx = (TW_RANGE_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_MACOSX));
                            twrangefix32macosx = (TW_RANGE_FIX32_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_FIX32_MACOSX));
                            twrange.ItemType = (TWTY)twrangemacosx.ItemType;
                            twrange.MinValue = twrangemacosx.MinValue;
                            twrange.MaxValue = twrangemacosx.MaxValue;
                            twrange.StepSize = twrangemacosx.StepSize;
                            twrange.DefaultValue = twrangemacosx.DefaultValue;
                            twrange.CurrentValue = twrangemacosx.CurrentValue;
                            twrangefix32.ItemType = (TWTY)twrangefix32macosx.ItemType;
                            twrangefix32.MinValue = twrangefix32macosx.MinValue;
                            twrangefix32.MaxValue = twrangefix32macosx.MaxValue;
                            twrangefix32.StepSize = twrangefix32macosx.StepSize;
                            twrangefix32.DefaultValue = twrangefix32macosx.DefaultValue;
                            twrangefix32.CurrentValue = twrangefix32macosx.CurrentValue;
                        }
                        // Windows or the 2.4+ Linux DSM...
                        else if ((ms_platform == Platform.WINDOWS) || (m_linuxdsm == LinuxDsm.IsLatestDsm) || ((m_blFoundLatestDsm || m_blFoundLatestDsm64) && (m_linuxdsm == LinuxDsm.IsLatestDsm)))
                        {
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twrange = (TW_RANGE)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE));
                            twrangefix32 = (TW_RANGE_FIX32)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_FIX32));
                        }
                        // The -2.3 Linux DSM...
                        else
                        {
                            intptrLocked = DsmMemLock(a_twcapability.hContainer);
                            twrangelinux64 = (TW_RANGE_LINUX64)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_LINUX64));
                            twrangefix32macosx = (TW_RANGE_FIX32_MACOSX)Marshal.PtrToStructure(intptrLocked, typeof(TW_RANGE_FIX32_MACOSX));
                            twrange.ItemType = (TWTY)twrangelinux64.ItemType;
                            twrange.MinValue = (uint)twrangelinux64.MinValue;
                            twrange.MaxValue = (uint)twrangelinux64.MaxValue;
                            twrange.StepSize = (uint)twrangelinux64.StepSize;
                            twrange.DefaultValue = (uint)twrangelinux64.DefaultValue;
                            twrange.CurrentValue = (uint)twrangelinux64.CurrentValue;
                            twrangefix32.ItemType = (TWTY)twrangefix32macosx.ItemType;
                            twrangefix32.MinValue = twrangefix32macosx.MinValue;
                            twrangefix32.MaxValue = twrangefix32macosx.MaxValue;
                            twrangefix32.StepSize = twrangefix32macosx.StepSize;
                            twrangefix32.DefaultValue = twrangefix32macosx.DefaultValue;
                            twrangefix32.CurrentValue = twrangefix32macosx.CurrentValue;
                        }

                        // Start the string...
                        csvRange = Common(a_twcapability.Cap, a_twcapability.ConType, twrange.ItemType);

                        // Tack on the data...
                        switch ((TWTY)twrange.ItemType)
                        {
                            default:
                                DsmMemUnlock(a_twcapability.hContainer);
                                return ("(Get Capability: unrecognized data type)");

                            case TWTY.INT8:
                                csvRange.Add(((char)(twrange.MinValue)).ToString());
                                csvRange.Add(((char)(twrange.MaxValue)).ToString());
                                csvRange.Add(((char)(twrange.StepSize)).ToString());
                                csvRange.Add(((char)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((char)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.INT16:
                                csvRange.Add(((short)(twrange.MinValue)).ToString());
                                csvRange.Add(((short)(twrange.MaxValue)).ToString());
                                csvRange.Add(((short)(twrange.StepSize)).ToString());
                                csvRange.Add(((short)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((short)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.INT32:
                                csvRange.Add(((int)(twrange.MinValue)).ToString());
                                csvRange.Add(((int)(twrange.MaxValue)).ToString());
                                csvRange.Add(((int)(twrange.StepSize)).ToString());
                                csvRange.Add(((int)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((int)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.UINT8:
                                csvRange.Add(((byte)(twrange.MinValue)).ToString());
                                csvRange.Add(((byte)(twrange.MaxValue)).ToString());
                                csvRange.Add(((byte)(twrange.StepSize)).ToString());
                                csvRange.Add(((byte)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((byte)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.BOOL:
                            case TWTY.UINT16:
                                csvRange.Add(((ushort)(twrange.MinValue)).ToString());
                                csvRange.Add(((ushort)(twrange.MaxValue)).ToString());
                                csvRange.Add(((ushort)(twrange.StepSize)).ToString());
                                csvRange.Add(((ushort)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((ushort)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.UINT32:
                                csvRange.Add(((uint)(twrange.MinValue)).ToString());
                                csvRange.Add(((uint)(twrange.MaxValue)).ToString());
                                csvRange.Add(((uint)(twrange.StepSize)).ToString());
                                csvRange.Add(((uint)(twrange.DefaultValue)).ToString());
                                csvRange.Add(((uint)(twrange.CurrentValue)).ToString());
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());

                            case TWTY.FIX32:
                                szTmp = ((double)twrangefix32.MinValue.Whole + ((double)twrangefix32.MinValue.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                szTmp = ((double)twrangefix32.MaxValue.Whole + ((double)twrangefix32.MaxValue.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                szTmp = ((double)twrangefix32.StepSize.Whole + ((double)twrangefix32.StepSize.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                szTmp = ((double)twrangefix32.DefaultValue.Whole + ((double)twrangefix32.DefaultValue.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                szTmp = ((double)twrangefix32.CurrentValue.Whole + ((double)twrangefix32.CurrentValue.Frac / 65536.0)).ToString("0." + new string('#', 339));
                                csvRange.Add(szTmp);
                                DsmMemUnlock(a_twcapability.hContainer);
                                return (csvRange.Get());
                        }
                    }
            }
        }
    }
}
