using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IronCards.Objects;

namespace IronCards.Services
{
    public class DatabaseService : IDatabaseService
    {
        public int Insert(string laneLabel)
        {
            int Id;
            using (var database = new LiteDB.LiteDatabase("Lanes.db"))
            {
                var lanes = database.GetCollection<LaneDocument>();
                Id = lanes.Insert(new LaneDocument() { Title = laneLabel });
            }
            return Id;
        }

        public void Update(int targetId, string eNewTitle)
        {
            throw new System.NotImplementedException();
        }
    }
}
