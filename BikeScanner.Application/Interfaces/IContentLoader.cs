using BikeScanner.Domain.Models;
using System;
using System.Threading.Tasks;

namespace BikeScanner.Application.Interfaces
{
    /// <summary>
    /// Interface for content load services
    /// </summary>
    public interface IContentLoader
    {
        /// <summary>
        /// Load items since specific date
        /// </summary>
        /// <param name="loadSince">Load content since date</param>
        /// <returns></returns>
        Task<ContentEntity[]> Load(DateTime loadSince);
    }
}
