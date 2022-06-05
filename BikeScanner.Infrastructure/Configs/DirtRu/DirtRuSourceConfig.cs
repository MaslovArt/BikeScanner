using System;

namespace BikeScanner.Infrastructure.Configs.DirtRu
{
	/// <summary>
    /// Dirt.ru market source congig
    /// </summary>
	public class DirtRuSourceConfig
	{
		/// <summary>
        /// Max number of pages in forum
        /// </summary>
		public int MaximumParsePages { get; set; }

		/// <summary>
		/// Stop words
		/// </summary>
		public string[] ExcludeKeys { get; set; } = Array.Empty<string>();

		/// <summary>
		/// Forums
		/// </summary>
		public DirtRuForumConfig[] Forums { get; set; } = Array.Empty<DirtRuForumConfig>();
	}
}

