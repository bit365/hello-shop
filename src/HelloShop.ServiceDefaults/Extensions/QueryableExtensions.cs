// Copyright (c) HelloShop Corporation. All rights reserved.
// See the license file in the project root for more information.

using HelloShop.ServiceDefaults.Constants;
using HelloShop.ServiceDefaults.Models.Paging;
using System.Linq.Expressions;
using System.Reflection;

namespace HelloShop.ServiceDefaults.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> SortBy<TEntity>(this IQueryable<TEntity> query, string? orderBy = null)
    {
        PropertyInfo[] properties = typeof(TEntity).GetProperties();

        IOrderedQueryable<TEntity>? orderedQueryable = null;

        if (!string.IsNullOrWhiteSpace(orderBy))
        {
            // Convert expressions of the form field1 desc,field2 asc

            string[] orderBySubs = orderBy.Split(',');

            foreach (var orderBySub in orderBySubs)
            {
                string[] orderByParts = orderBySub.Trim().Split(' ');

                if (orderByParts.Length >= 1)
                {
                    string propertyName = PascalCaseNamingPolicy.PascalCase.ConvertName(orderByParts[0]);

                    bool ascending = orderByParts.Length == 1 || (orderByParts.Length == 2 && orderByParts[1].Equals("asc", StringComparison.OrdinalIgnoreCase));

                    if (properties.Any(x => x.Name == propertyName))
                    {
                        if (ascending)
                        {
                            orderedQueryable = orderedQueryable is null ? query.OrderBy(propertyName) : orderedQueryable.ThenBy(propertyName);
                        }
                        else
                        {
                            orderedQueryable = orderedQueryable is null ? query.OrderByDescending(propertyName) : orderedQueryable.ThenByDescending(propertyName);
                        }
                    }
                }
            }
        }

        if (orderedQueryable is null)
        {
            string defaultPropertyName = properties.Any(x => x.Name == EntityConnstants.DefaultKey) ? EntityConnstants.DefaultKey : properties.First().Name;

            orderedQueryable = query.OrderByDescending(defaultPropertyName);
        }

        return orderedQueryable;
    }

    public static IQueryable<TEntity> PageBy<TEntity>(this IQueryable<TEntity> query, PagedRequest pagedRequest)
    {
        return query.Skip((pagedRequest.PageNumber - 1) * pagedRequest.PageSize).Take(pagedRequest.PageSize);
    }

    public static IQueryable<TEntity> SortAndPageBy<TEntity>(this IQueryable<TEntity> query, PagedAndSortedRequest? pagedAndSortedRequest = null)
    {
        pagedAndSortedRequest ??= new PagedAndSortedRequest { PageNumber = 1, PageSize = PagingConstants.DefaultPageSize };

        return query.SortBy(pagedAndSortedRequest.OrderBy).PageBy(pagedAndSortedRequest);
    }

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }

    public static IQueryable<TSource> WhereIf<TSource>(this IQueryable<TSource> source, bool condition, Expression<Func<TSource, int, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}
