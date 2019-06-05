using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using IronCards.Objects;

namespace IronCards.Services
{
    public class DatabaseService : IDatabaseService
    {
        public int Insert(string laneLabel)
        {
            int id;
            using (var database = new LiteDB.LiteDatabase("Lanes.db"))
            {
                var lanes = database.GetCollection<LaneDocument>();
                id = lanes.Insert(new LaneDocument() { Title = laneLabel });
            }
            return id;
        }

        public void Update(int targetId, string laneLabel)
        {
            using (var database = new LiteDB.LiteDatabase("Lanes.db"))
            {
                var lanes = database.GetCollection<LaneDocument>();
                lanes.Update(targetId, new LaneDocument() { Title = laneLabel });
            }
        }

        public List<LaneDocument> GetAll()
        {
            var laneDocuments = new List<LaneDocument>();
            using (var database = new LiteDB.LiteDatabase("Lanes.db"))
            {
                var lanes = database.GetCollection<LaneDocument>();
                laneDocuments=lanes.FindAll().ToList();
            }

            return laneDocuments;
        }
    }
}
