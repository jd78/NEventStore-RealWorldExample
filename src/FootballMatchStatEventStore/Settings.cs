using System;
using System.Configuration;

namespace FootballMatchStatEventStore
{
    public class Settings
    {
        public int SnapshotEvery { get; private set; }

        public Settings()
        {
            SnapshotEvery = Convert.ToInt32(ConfigurationManager.AppSettings["SnapshotEvery"]);
        }
    }
}
