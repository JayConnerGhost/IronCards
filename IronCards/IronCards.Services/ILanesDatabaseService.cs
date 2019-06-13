﻿using System.Collections.Generic;
using IronCards.Objects;

namespace IronCards.Services
{
    public interface ILanesDatabaseService
    {
        int Insert(string laneLabel);
        void Update(int targetId, string laneLabel);
        List<LaneDocument> GetAll();
        bool Delete(int laneId);
    }
}