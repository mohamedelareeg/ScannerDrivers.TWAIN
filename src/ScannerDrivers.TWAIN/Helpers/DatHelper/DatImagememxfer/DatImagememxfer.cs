using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {

        /// <summary>
        /// Issue memory image transfer commands...
        /// </summary>
        /// <param name="a_dg">Data group</param>
        /// <param name="a_msg">Operation</param>
        /// <param name="a_twimagememxfer">IMAGEMEMXFER structure</param>
        /// <returns>TWAIN status</returns>
        public STS DatImagememxfer(DataGroups a_dg, Messages a_msg, ref TW_IMAGEMEMXFER a_twimagememxfer)
        {
            STS sts;

            // Submit the work to the TWAIN thread...
            if (this.m_runinuithreaddelegate == null)
            {
                if ((m_threadTwain != null) && (m_threadTwain.ManagedThreadId != Thread.CurrentThread.ManagedThreadId))
                {
                    lock (m_lockTwain)
                    {
                        // Set our command variables...
                        ThreadData threaddata = default(ThreadData);
                        threaddata.twimagememxfer = a_twimagememxfer;
                        threaddata.dg = a_dg;
                        threaddata.msg = a_msg;
                        threaddata.dat = DataArgumentTypes.IMAGEMEMXFER;
                        long lIndex = m_twaincommand.Submit(threaddata);

                        // Submit the command and wait for the reply...
                        CallerToThreadSet();
                        ThreadToCallerWaitOne();

                        // Return the result...
                        a_twimagememxfer = m_twaincommand.Get(lIndex).twimagememxfer;
                        sts = m_twaincommand.Get(lIndex).sts;

                        // Clear the command variables...
                        m_twaincommand.Delete(lIndex);
                    }
                    return (sts);
                }
            }

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendBefore(a_dg.ToString(), DataArgumentTypes.IMAGEMEMXFER.ToString(), a_msg.ToString(), ImagememxferToCsv(a_twimagememxfer));
            }

            // Windows...
            if (ms_platform == Platform.WINDOWS)
            {
                // Issue the command...
                try
                {
                    if (m_threaddataDatImagememxfer.blIsInuse || (this.m_runinuithreaddelegate == null))
                    {
                        if (m_blUseLegacyDSM)
                        {
                            sts = (STS)NativeMethods.WindowsTwain32DsmEntryImagememxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGEMEMXFER, a_msg, ref a_twimagememxfer);
                        }
                        else
                        {
                            sts = (STS)NativeMethods.WindowsTwaindsmDsmEntryImagememxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGEMEMXFER, a_msg, ref a_twimagememxfer);
                        }
                    }
                    else
                    {
                        if (m_blUseLegacyDSM)
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagememxfer = default(ThreadData);
                                m_threaddataDatImagememxfer.blIsInuse = true;
                                m_threaddataDatImagememxfer.dg = a_dg;
                                m_threaddataDatImagememxfer.msg = a_msg;
                                m_threaddataDatImagememxfer.dat = DataArgumentTypes.IMAGEMEMXFER;
                                m_threaddataDatImagememxfer.twimagememxfer = a_twimagememxfer;
                                RunInUiThread(DatImagememxferWindowsTwain32);
                                a_twimagememxfer = m_threaddataDatImagememxfer.twimagememxfer;
                                sts = m_threaddataDatImagememxfer.sts;
                                m_threaddataDatImagememxfer = default(ThreadData);
                            }
                        }
                        else
                        {
                            lock (m_lockTwain)
                            {
                                m_threaddataDatImagememxfer = default(ThreadData);
                                m_threaddataDatImagememxfer.blIsInuse = true;
                                m_threaddataDatImagememxfer.dg = a_dg;
                                m_threaddataDatImagememxfer.msg = a_msg;
                                m_threaddataDatImagememxfer.dat = DataArgumentTypes.IMAGEMEMXFER;
                                m_threaddataDatImagememxfer.twimagememxfer = a_twimagememxfer;
                                RunInUiThread(DatImagememxferWindowsTwainDsm);
                                a_twimagememxfer = m_threaddataDatImagememxfer.twimagememxfer;
                                sts = m_threaddataDatImagememxfer.sts;
                                m_threaddataDatImagememxfer = default(ThreadData);
                            }
                        }
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Linux...
            else if (ms_platform == Platform.LINUX)
            {
                // Issue the command...
                try
                {
                    if (m_blFoundLatestDsm64 && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.Linux64DsmEntryImagememxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGEMEMXFER, a_msg, ref a_twimagememxfer);
                    }
                    else if (m_blFoundLatestDsm && (m_linuxdsm == LinuxDsm.IsLatestDsm))
                    {
                        sts = (STS)NativeMethods.LinuxDsmEntryImagememxfer(ref m_twidentitylegacyApp, ref m_twidentitylegacyDs, a_dg, DataArgumentTypes.IMAGEMEMXFER, a_msg, ref a_twimagememxfer);
                    }
                    else if (m_blFound020302Dsm64bit && (m_linuxdsm == LinuxDsm.Is020302Dsm64bit))
                    {
                        TW_IMAGEMEMXFER_LINUX64 twimagememxferlinux64 = default(TW_IMAGEMEMXFER_LINUX64);
                        twimagememxferlinux64.BytesPerRow = a_twimagememxfer.BytesPerRow;
                        twimagememxferlinux64.BytesWritten = a_twimagememxfer.BytesWritten;
                        twimagememxferlinux64.Columns = a_twimagememxfer.Columns;
                        twimagememxferlinux64.Compression = a_twimagememxfer.Compression;
                        twimagememxferlinux64.MemoryFlags = a_twimagememxfer.Memory.Flags;
                        twimagememxferlinux64.MemoryLength = a_twimagememxfer.Memory.Length;
                        twimagememxferlinux64.MemoryTheMem = a_twimagememxfer.Memory.TheMem;
                        twimagememxferlinux64.Rows = a_twimagememxfer.Rows;
                        twimagememxferlinux64.XOffset = a_twimagememxfer.XOffset;
                        twimagememxferlinux64.YOffset = a_twimagememxfer.YOffset;
                        sts = (STS)NativeMethods.Linux020302Dsm64bitEntryImagememxfer(ref m_twidentityApp, ref m_twidentityDs, a_dg, DataArgumentTypes.IMAGEMEMXFER, a_msg, ref twimagememxferlinux64);
                        a_twimagememxfer.BytesPerRow = (uint)twimagememxferlinux64.BytesPerRow;
                        a_twimagememxfer.BytesWritten = (uint)twimagememxferlinux64.BytesWritten;
                        a_twimagememxfer.Columns = (uint)twimagememxferlinux64.Columns;
                        a_twimagememxfer.Compression = (ushort)twimagememxferlinux64.Compression;
                        a_twimagememxfer.Memory.Flags = (uint)twimagememxferlinux64.MemoryFlags;
                        a_twimagememxfer.Memory.Length = (uint)twimagememxferlinux64.MemoryLength;
                        a_twimagememxfer.Memory.TheMem = twimagememxferlinux64.MemoryTheMem;
                        a_twimagememxfer.Rows = (uint)twimagememxferlinux64.Rows;
                        a_twimagememxfer.XOffset = (uint)twimagememxferlinux64.XOffset;
                        a_twimagememxfer.YOffset = (uint)twimagememxferlinux64.YOffset;
                    }
                    else
                    {
                        Log.Error("apparently we don't have a DSM...");
                        sts = STS.BUMMER;
                    }
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Mac OS X, which has to be different...
            else if (ms_platform == Platform.MACOSX)
            {
                // Issue the command...
                try
                {
                    TW_IMAGEMEMXFER_MACOSX twimagememxfermacosx = default(TW_IMAGEMEMXFER_MACOSX);
                    twimagememxfermacosx.BytesPerRow = a_twimagememxfer.BytesPerRow;
                    twimagememxfermacosx.BytesWritten = a_twimagememxfer.BytesWritten;
                    twimagememxfermacosx.Columns = a_twimagememxfer.Columns;
                    twimagememxfermacosx.Compression = a_twimagememxfer.Compression;
                    twimagememxfermacosx.Memory.Flags = a_twimagememxfer.Memory.Flags;
                    twimagememxfermacosx.Memory.Length = a_twimagememxfer.Memory.Length;
                    twimagememxfermacosx.Memory.TheMem = a_twimagememxfer.Memory.TheMem;
                    twimagememxfermacosx.Rows = a_twimagememxfer.Rows;
                    twimagememxfermacosx.XOffset = a_twimagememxfer.XOffset;
                    twimagememxfermacosx.YOffset = a_twimagememxfer.YOffset;
                    if (m_blUseLegacyDSM)
                    {
                        sts = (STS)NativeMethods.MacosxTwainDsmEntryImagememxfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.IMAGEMEMXFER, a_msg, ref twimagememxfermacosx);
                    }
                    else
                    {
                        sts = (STS)NativeMethods.MacosxTwaindsmDsmEntryImagememxfer(ref m_twidentitymacosxApp, ref m_twidentitymacosxDs, a_dg, DataArgumentTypes.IMAGEMEMXFER, a_msg, ref twimagememxfermacosx);
                    }
                    a_twimagememxfer.BytesPerRow = twimagememxfermacosx.BytesPerRow;
                    a_twimagememxfer.BytesWritten = twimagememxfermacosx.BytesWritten;
                    a_twimagememxfer.Columns = twimagememxfermacosx.Columns;
                    a_twimagememxfer.Compression = (ushort)twimagememxfermacosx.Compression;
                    a_twimagememxfer.Memory.Flags = twimagememxfermacosx.Memory.Flags;
                    a_twimagememxfer.Memory.Length = twimagememxfermacosx.Memory.Length;
                    a_twimagememxfer.Memory.TheMem = twimagememxfermacosx.Memory.TheMem;
                    a_twimagememxfer.Rows = twimagememxfermacosx.Rows;
                    a_twimagememxfer.XOffset = twimagememxfermacosx.XOffset;
                    a_twimagememxfer.YOffset = twimagememxfermacosx.YOffset;
                }
                catch (Exception exception)
                {
                    // The driver crashed...
                    Log.Error("crash - " + exception.Message);
                    Log.LogSendAfter(STS.BUMMER, "");
                    return (STS.BUMMER);
                }
            }

            // Uh-oh...
            else
            {
                Log.LogSendAfter(STS.BUMMER, "");
                return (STS.BUMMER);
            }

            // Get DAT_STATUS, if needed...
            Codes.STS stsRcOrCc = AutoDatStatus(sts);

            // Log it...
            if (Log.GetLevel() > 0)
            {
                Log.LogSendAfter(stsRcOrCc, ImagememxferToCsv(a_twimagememxfer));
            }

            // If we had a successful transfer, then change state...
            if (sts == STS.XFERDONE)
            {
                m_state = STATE.S7;
            }

            // All done...
            return (stsRcOrCc);
        }
    }
}
