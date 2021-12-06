using System;
using System.Threading.Tasks;

namespace BikeScanner.Application.Interfaces
{
    public interface IContentIndexator
    {
        Task Execute(DateTime loadSince);
    }
}