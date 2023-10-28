﻿namespace SoftwareFest.Pagination.Contracts
{
    public interface IPage<T> : IPage, IEnumerable<T>
    {

    }

    public interface IPage
    {
        public int CurrentPage { get; }

        public int PageSize { get; }

        public int TotalCount { get; }
    }
}
