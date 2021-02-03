using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Incident.Models;

namespace Incident.Data
{
    public class IncidentContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = "Server=tcp:incidentenmelder-test-sql.database.windows.net,1433;Initial Catalog=incidentenmelder-test-db;Persist Security Info=False;User ID=mvdwoude;Password=fFJLrlzw-aChCgjJ;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<IncidentModel> Incidents { get; set; }
    }
}
