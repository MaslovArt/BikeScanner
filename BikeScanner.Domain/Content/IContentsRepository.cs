using BikeScanner.Domain.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeScanner.Domain.Content
{
    public interface IContentsRepository : IRepository<ContentEntity>
    {
        /// <summary>
        /// Add new contents to db; Remove expired and not actual content from db;
        /// </summary>
        /// <param name="contents">Actual content</param>
        /// <param name="saveSince">Remove content since date</param>
        /// <returns>Changes count</returns>
        Task<int> Refresh(IEnumerable<ContentEntity> contents, DateTime saveSince);
    }
}
