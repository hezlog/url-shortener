﻿namespace UrlShortener.Infrastructure.Data.Filter
{
    public interface ILimit<in TFilter>
        where TFilter : Restful.Query.Filter.Filter
    {
        int Apply(TFilter filter);
    }
}