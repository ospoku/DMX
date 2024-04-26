﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace DMX.ViewModels
{
    public class AddAttendanceVM
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public SelectList Attendees { get; set; }
        public List<string>SelectedParticipants { get; set; }
    }
}
