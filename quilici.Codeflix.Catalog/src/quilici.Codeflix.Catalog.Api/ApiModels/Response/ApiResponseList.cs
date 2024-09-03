using quilici.Codeflix.Catalog.Application.Common;

namespace quilici.Codeflix.Catalog.Api.ApiModels.Response
{
    public class ApiResponseList<TItempData> : ApiResponse<IReadOnlyList<TItempData>>
    {
        public ApiResponseListMeta Meta { get; private set; }

        public ApiResponseList(int currentPage, int perPage, int total, IReadOnlyList<TItempData> data) 
            : base(data)
        {
            Meta = new ApiResponseListMeta(currentPage, perPage, total);
        }

        public ApiResponseList(PaginatedListOutput<TItempData> paginatedListOutput) 
            : base(paginatedListOutput.Items)
        {
            Meta = new ApiResponseListMeta(paginatedListOutput.Page, paginatedListOutput.PerPage, paginatedListOutput.Total);
        }
    }
}
