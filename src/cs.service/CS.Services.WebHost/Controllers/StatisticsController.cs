﻿using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using CS.Service.Data;
using CS.Service.DataAccess;
using CS.Service.WebHost.Models;

namespace CS.Service.WebHost.Controllers
{
    public class StatisticsController : ApiController
    {
        private IMapper _mapper;

        private IMapper Mapper => _mapper ?? (_mapper = new AutoMapperConfigurator().Initialize());

        public HttpResponseMessage GetLatestStatistics()
        {
            return Request.CreateResponse(GetStatsFromDatabase());
        }

        private IEnumerable<StatisticsEntry> GetStatsFromDatabase()
        {
            List<StatisticsEntry> stats;

            using (DatabaseContext dbContext = new DatabaseContext())
            {
                stats = dbContext.Statistics.ProjectToList<StatisticsEntry>(Mapper.ConfigurationProvider);
            }

            return stats;
        }

        //public void PostNewStatisticsBatch(IEnumerable<StatisticsEntry> newStatistics)
        //{

        //}

        public HttpResponseMessage PostNewStatistic(StatisticsEntry newStatistic)
        {
            AddToDatabase(newStatistic);

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        private void AddToDatabase(StatisticsEntry newStatistic)
        {
            using (DatabaseContext dbContext = new DatabaseContext())
            {
                Statistics newEntity = Mapper.Map<StatisticsEntry, Statistics>(newStatistic);

                dbContext.Statistics.Add(newEntity);

                dbContext.SaveChanges();
            }
        }
    }
}
