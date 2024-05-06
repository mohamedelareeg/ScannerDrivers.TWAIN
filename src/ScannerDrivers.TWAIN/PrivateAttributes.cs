using ScannerDrivers.TWAIN.Contants.DataArgumentType;
using ScannerDrivers.TWAIN.Contants.Enum;
using ScannerDrivers.TWAIN.Contants.Structure;
using static ScannerDrivers.TWAIN.Contants.Enum.Codes;

namespace ScannerDrivers.TWAIN
{
    public partial class TWAINSDK
    {
        ///////////////////////////////////////////////////////////////////////////////
        // Private Attributes...
        ///////////////////////////////////////////////////////////////////////////////
        #region Private Attributes...

        /// <summary>
        /// Our application identity...
        /// </summary>
        private TW_IDENTITY m_twidentityApp;
        private TW_IDENTITY_LEGACY m_twidentitylegacyApp;
        private TW_IDENTITY_MACOSX m_twidentitymacosxApp;

        /// <summary>
        /// Our Data Source identity...
        /// </summary>
        private TW_IDENTITY m_twidentityDs;
        private TW_IDENTITY_LEGACY m_twidentitylegacyDs;
        private TW_IDENTITY_MACOSX m_twidentitymacosxDs;

        /// <summary>
        /// Our current TWAIN state...
        /// </summary>
        private STATE m_state;
        private bool m_blAcceptXferReady;

        /// <summary>
        /// DAT_NULL flags that we've seen after entering into
        /// state 5 through MSG_ENABLEDS or MSG_ENABLEDSUIONLY,
        /// or coming down from DAT_PENDINGXFERS, either
        /// MSG_ENDXFER or MSG_RESET...
        /// </summary>
        private bool m_blIsMsgxferready;
        private bool m_blIsMsgclosedsreq;
        private bool m_blIsMsgclosedsok;
        private bool m_blIsMsgdeviceevent;
        private bool m_blRunningDatUserinterface;
        private Thread m_threadRunningDatUserinterface;

        /// <summary>
        /// Automatically issue DataArgumentTypes.STATUS on TWRC_FAILURE...
        /// </summary>
        private bool m_blAutoDatStatus;

        /// <summary>
        /// Windows, pick between TWAIN_32.DLL and TWAINDSM.DLL...
        /// Mac OS X, pick between /System/Library/Frameworks/TWAIN.framework and /Library/Frameworks/TWAINDSM.framework
        /// </summary>
        private bool m_blUseLegacyDSM;

        /// <summary>
        /// Help us pick the right DSM for the current data source,
        /// the first one is for the session, the second one is for
        /// getfirst/getnext, and allows us to check for drivers
        /// using either one or both DSMs, depending on what is
        /// available...
        /// </summary>
        private LinuxDsm m_linuxdsm;
        private LinuxDsm m_linux64bitdsmDatIdentity;
        private bool m_blFoundLatestDsm;
        private bool m_blFoundLatestDsm64;
        private bool m_blFound020302Dsm64bit;

        /// <summary>
        /// Use the callback system (TWAINDSM.DLL only)...
        /// </summary>
        private bool m_blUseCallbacks;

        /// <summary>
        /// The platform we're running on...
        /// </summary>
        private static Platform ms_platform;
        private static Processor ms_processor;
        private static bool ms_blFirstPassGetPlatform = true;

        /// <summary>
        /// Delegates for DAT_CALLBACK...
        /// </summary>
        private NativeMethods.WindowsDsmEntryCallbackDelegate m_windowsdsmentrycontrolcallbackdelegate;
        private NativeMethods.LinuxDsmEntryCallbackDelegate m_linuxdsmentrycontrolcallbackdelegate;
        private NativeMethods.MacosxDsmEntryCallbackDelegate m_macosxdsmentrycontrolcallbackdelegate;

        /// <summary>
        /// We only allow one thread at a time to talk to the TWAIN driver...
        /// </summary>
        private Object m_lockTwain;

        /// <summary>
        /// Use this to wait for commands from the caller...
        /// </summary>
        private AutoResetEvent m_autoreseteventCaller;

        /// <summary>
        /// Use this to force the user's command to block until TWAIN has
        /// a response...
        /// </summary>
        private AutoResetEvent m_autoreseteventThread;

        /// <summary>
        /// Use this to force the user's rollback to block until TWAIN has
        /// a response...
        /// </summary>
        private AutoResetEvent m_autoreseteventRollback;

        /// <summary>
        /// One can get into a race condition with the thread, so we use
        /// this event to confirm that it's started and ready for use...
        /// </summary>
        private AutoResetEvent m_autoreseteventThreadStarted;

        /// <summary>
        /// The data we share with the thread...
        /// </summary>
        //private ThreadData m_threaddata;

        /// <summary>
        /// Our callback for device events...
        /// </summary>
        private DeviceEventCallback m_deviceeventcallback;

        /// <summary>
        /// Our callback function for scanning...
        /// </summary>
        private ScanCallback m_scancallback;

        /// <summary>
        /// Run stuff in a caller's UI thread...
        /// </summary>
        private RunInUiThreadDelegate m_runinuithreaddelegate;

        /// <summary>
        /// The event calls don't go through the thread...
        /// </summary>
        private TW_EVENT m_tweventPreFilterMessage;

        // Remember the window handle, so we can reuse it...
        private IntPtr m_intptrHwnd;

        /// <summary>
        /// Our thread...
        /// </summary>
        private Thread m_threadTwain;

        /// <summary>
        /// How we talk to our thread...
        /// </summary>
        private TwainCommand m_twaincommand;

        /// <summary>
        ///  Indecies for commands that have to do something a
        ///  bit more fancy, such as running the command in the
        ///  context of a GUI thread.  And flags to help know
        ///  when we are doing this...
        /// </summary>
        private ThreadData m_threaddataDatAudiofilexfer;
        private ThreadData m_threaddataDatAudionativexfer;
        private ThreadData m_threaddataDatCapability;
        private ThreadData m_threaddataDatEvent;
        private ThreadData m_threaddataDatExtimageinfo;
        private ThreadData m_threaddataDatIdentity;
        private ThreadData m_threaddataDatImagefilexfer;
        private ThreadData m_threaddataDatImageinfo;
        private ThreadData m_threaddataDatImagelayout;
        private ThreadData m_threaddataDatImagememfilexfer;
        private ThreadData m_threaddataDatImagememxfer;
        private ThreadData m_threaddataDatImagenativexfer;
        private ThreadData m_threaddataDatParent;
        private ThreadData m_threaddataDatPendingxfers;
        private ThreadData m_threaddataDatSetupfilexfer;
        private ThreadData m_threaddataDatSetupmemxfer;
        private ThreadData m_threaddataDatStatus;
        private ThreadData m_threaddataDatUserinterface;

        /// <summary>
        /// Our helper functions from the DSM...
        /// </summary>
        private TW_ENTRYPOINT_DELEGATES m_twentrypointdelegates;

        #endregion
    }
}
