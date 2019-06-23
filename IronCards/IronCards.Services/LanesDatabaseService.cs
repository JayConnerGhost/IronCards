﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using IronCards.Objects;

namespace IronCards.Services
{
    public class LanesDatabaseService : BaseDatabaseService, ILanesDatabaseService
    {
        public LanesDatabaseService() : base()
        {

        }

        public int Insert(string laneLabel, int projectId)
        {
            int id;
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var lanes = database.GetCollection<LaneDocument>();
                id = lanes.Insert(new LaneDocument() { Title = laneLabel,ProjectId=projectId });
            }
            return id;
        }

        public void Update(int targetId, string laneLabel)
        {
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var lanes = database.GetCollection<LaneDocument>();
                lanes.Update(targetId, new LaneDocument() { Title = laneLabel });
            }
        }

        public List<LaneDocument> GetAll()
        {
            var laneDocuments = new List<LaneDocument>();
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var lanes = database.GetCollection<LaneDocument>();
                laneDocuments=lanes.FindAll().ToList();
            }

            return laneDocuments;
        }

        public bool Delete(int laneId)
        {
            using (var database = new LiteDB.LiteDatabase(ConnectionString))
            {
                var lanes = database.GetCollection<LaneDocument>();
               var result= lanes.Delete(laneId);
               return result;
            }
        }
    }
}
