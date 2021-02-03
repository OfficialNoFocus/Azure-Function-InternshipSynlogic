using System;
using System.Collections.Generic;
using System.Text;

namespace Incident.Models
{
    public class IncidentModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Discription { get; set; }
        public string Location { get; set; }
        public string Owner { get; set; }
        public DateTime CreationTime { get; set; }
        public string Status { get; set; }
        public string Weight { get; set; }
    }
}
