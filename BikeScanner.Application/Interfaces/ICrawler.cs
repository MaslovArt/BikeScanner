using System;
using System.Threading.Tasks;
using BikeScanner.Application.Models;

namespace BikeScanner.Application.Interfaces
{
	/// <summary>
    /// Ads crawler interface
    /// </summary>
	public interface ICrawler
	{
		/// <summary>
        /// Get ads from specific resource
        /// </summary>
        /// <param name="since">Get since datetime</param>
        /// <returns>Array of ads</returns>
		Task<AdItem[]> Run(DateTime since);

		/// <summary>
        /// Sections array
        /// </summary>
		string[] NeedValidityCheck { get; }
	}
}

