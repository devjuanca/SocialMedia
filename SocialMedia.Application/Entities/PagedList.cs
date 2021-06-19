using Microsoft.EntityFrameworkCore;
using SocialMedia.Application.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia.Application.Entities
{
    //Paginacion Asincrona
    public  class PagedList <T>: List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int ItemsCount { get; set; }
        public int PageSize { get; set; }

        public bool  HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public int? NextPageNumber => HasNext ? CurrentPage + 1 : (int?) null;
        public int? PreviousPageNumber => HasPrevious ? CurrentPage - 1 : (int?) null;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            ItemsCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling( (ItemsCount / (double)pageSize));
            AddRange(items);
        }

        public static async Task< PagedList<T>> Create(IQueryable<T> items_source, int pageNumber, int pageSize)
        {
            
            int count = await items_source.CountAsync();
            if (count > 0)
            {
                var items = await items_source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

                return new PagedList<T>(items, count, pageNumber, pageSize);
            }
            throw new NotFoundException("Not Found",null);
        }
    }
}
