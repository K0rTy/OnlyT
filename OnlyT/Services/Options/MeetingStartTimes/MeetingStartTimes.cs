﻿namespace OnlyT.Services.Options.MeetingStartTimes
{
    using System;
    using System.Collections.Generic;
    using Utils;

    public class MeetingStartTimes
    {
        public List<MeetingStartTime> Times { get;  }

        public MeetingStartTimes()
        {
            Times = new List<MeetingStartTime>();
        }

        public void Sanitize()
        {
            foreach (var startTime in Times)
            {
                startTime.Sanitize();
            }
        }

        public void FromText(string value)
        {
            Times.Clear();

            if (!string.IsNullOrWhiteSpace(value))
            {
                var lines = value.SplitIntoLines();
                foreach (var line in lines)
                {
                    var startTime = MeetingStartTime.FromText(line);
                    if (startTime != null)
                    {
                        Times.Add(startTime);
                    }
                }
            }
        }

        public string AsText()
        {
            var result = new List<string>();

            foreach (var startTime in Times)
            {
                AddStartTime(result, startTime);
            }

            return string.Join(Environment.NewLine, result);
        }

        private void AddStartTime(List<string> result, MeetingStartTime meetingStartTime)
        {
            string startTime = meetingStartTime?.AsText();
            if (!string.IsNullOrWhiteSpace(startTime))
            {
                result.Add(startTime);
            }
        }
    }
}
