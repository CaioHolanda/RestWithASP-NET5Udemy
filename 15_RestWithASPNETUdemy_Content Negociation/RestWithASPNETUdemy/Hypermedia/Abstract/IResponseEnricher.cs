using Microsoft.AspNetCore.Mvc.Filters;

namespace RestWithASPNETUdemy.Hypermedia.Abstract
{
    public interface IResponseEnricher
    {
        bool CanEnricher(ResultExecutingContext context);
        Task Enrich(ResultExecutingContext context);
    }
}
