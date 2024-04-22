using System.Text.Json;
using fw_shop_api.Models.Util;

namespace fw_shop_api
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, MetaData metadata)
        {
            var options = new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

            response.Headers.Append("Pagination", JsonSerializer.Serialize(metadata, options));
            response.Headers.Append("Access-Control-Expose-Headers", "Pagination");
        }
    }
}