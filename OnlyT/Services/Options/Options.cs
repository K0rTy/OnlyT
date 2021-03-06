﻿namespace OnlyT.Services.Options
{
    // ReSharper disable MemberCanBePrivate.Global
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Windows;
    using Models;

    /// <summary>
    /// All program options. The full structure is written to disk in JSON format on change
    /// of data, and read from disk during app startup
    /// </summary>
    public class Options
    {
        public const int DefaultPort = 8096;
        public const int MaxPossiblePorts = 0x80;

        public Options()
        {
            OperatingMode = OperatingMode.Automatic;
            AlwaysOnTop = true;
            IsBellEnabled = true;
            IsCountdownEnabled = false;
            BellVolumePercent = 70;
            MidWeekAdaptiveMode = AdaptiveMode.None;
            WeekendAdaptiveMode = AdaptiveMode.None;
            AnalogueClockWidthPercent = 50;
            FullScreenClockMode = FullScreenClockMode.AnalogueAndDigital;
            ShowDurationSector = true;
            HttpServerPort = DefaultPort;
            PersistDurationSecs = 90;
            IsApiThrottled = true;
            PersistStudentTime = true;
            MeetingStartTimes = new MeetingStartTimes.MeetingStartTimes();

            var dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;

            bool clock24 = dateFormat.Contains("H");
            bool leadingZero = dateFormat.Contains("HH") || dateFormat.Contains("hh");
            bool ampm = dateFormat.Contains("tt");

            if (clock24)
            {
                ClockHourFormat = leadingZero ? ClockHourFormat.Format24LeadingZero : ClockHourFormat.Format24;
            }
            else
            {
                if (leadingZero)
                {
                    ClockHourFormat = ampm 
                        ? ClockHourFormat.Format12LeadingZeroAMPM : ClockHourFormat.Format12LeadingZero;
                }
                else
                {
                    ClockHourFormat = ampm
                        ? ClockHourFormat.Format12AMPM : ClockHourFormat.Format12;
                }
            }
        }

        public string TimerMonitorId { get; set; }

        public string AppWindowPlacement { get; set; }

        public Size SettingsPageSize { get; set; }

        public OperatingMode OperatingMode { get; set; }

        public MidWeekOrWeekend MidWeekOrWeekend { get; set; }

        public bool IsCircuitVisit { get; set; }

        public bool PersistStudentTime { get; set; }

        public bool AlwaysOnTop { get; set; }

        public bool IsBellEnabled { get; set; }

        public int BellVolumePercent { get; set; }

        public ClockHourFormat ClockHourFormat { get; set; }

        public AdaptiveMode MidWeekAdaptiveMode { get; set; }

        public AdaptiveMode WeekendAdaptiveMode { get; set; }

        public int AnalogueClockWidthPercent { get; set; }

        public FullScreenClockMode FullScreenClockMode { get; set; }

        public bool ShowTimeOfDayUnderTimer { get; set; }

        public bool ShowDurationSector { get; set; }

        public bool CountUp { get; set; }

        public int HttpServerPort { get; set; }

        public int PersistDurationSecs { get; set; }

        public bool IsWebClockEnabled { get; set; }

        public bool AllowCountUpToggle { get; set; }

        public MeetingStartTimes.MeetingStartTimes MeetingStartTimes { get; set; }

        public bool IsCountdownEnabled { get; set; }

        public bool IsApiEnabled { get; set; }

        public string ApiCode { get; set; }

        public bool IsApiThrottled { get; set; }

        public bool ShowMousePointerInTimerDisplay { get; set; }

        /// <summary>
        /// Validates the data, correcting automatically as required
        /// </summary>
        public void Sanitize()
        {
            if (BellVolumePercent < 0)
            {
                BellVolumePercent = 0;
            }

            if (BellVolumePercent > 100)
            {
                BellVolumePercent = 100;
            }

            if (AnalogueClockWidthPercent < 0 || AnalogueClockWidthPercent > 100)
            {
                AnalogueClockWidthPercent = 50;
            }

            if (HttpServerPort < DefaultPort || HttpServerPort > DefaultPort + MaxPossiblePorts)
            {
                HttpServerPort = DefaultPort;
            }
            
            var persistDurations = GetPersistDurationItems().ToArray();
            if (persistDurations.FirstOrDefault(x => x.Seconds == PersistDurationSecs) == null)
            {
                PersistDurationSecs = persistDurations[persistDurations.Length / 2].Seconds;
            }

            MeetingStartTimes.Sanitize();
        }

        public IEnumerable<PersistDurationItem> GetPersistDurationItems()
        {
            var result = new List<PersistDurationItem>();

            const int NumItems = 11;
            const int SecsIncrement = 15;

            int secs = SecsIncrement;

            for (int n = 0; n < NumItems; ++n)
            {
                result.Add(new PersistDurationItem(secs));
                secs += SecsIncrement;
            }

            return result;
        }
    }
}
