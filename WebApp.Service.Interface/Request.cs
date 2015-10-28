using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WebApp.Service.Interface
{
    public interface IBaseRequest
    {
        int Take { get; set; }
        int Skip { get; set; }
        int Page { get; set; }
        List<Sort> Sort { get; set; }
        Filter Filter { get; set; }
        Int32 TotalRows { get; set; }
    }

    public class BaseRequest : IBaseRequest
    {
        public int Take { get; set; }
        public int Skip { get; set; }
        public int Page { get; set; }
        public List<Sort> Sort { get; set; }
        public Filter Filter { get; set; }
        public Int32 TotalRows { get; set; }

        public BaseRequest()
        {
            Sort = new List<Sort>();
            Take = 100000;
            Page = 1;
        }
    }

    //базовый запрос от клиента
    public class Request : BaseRequest
    {
        public Guid? ID { get; set; }
        public SearchData SearchData { get; set; }

        public Request()
        {

        }
    }

    //базовый ответ клиенту
    public class Response
    {
        public object Data { get; set; }
        public Int64 Total { get; set; }
        public bool Success { get; set; }
        public IList<String> ErrorList { get; set; }

        public Response()
        {
            Success = true;
            ErrorList = new List<String>();
        }

        public static Response CreateErrorResponse(Object data, IList<String> errorList)
        {
            return new Response
            {
                Data = data,
                Success = false,
                ErrorList = errorList
            };
        }
    }

    public abstract class CreateDocumentRequest
    {

    }

    public class Sort
    {
        public String Field { get; set; }
        public String Dir { get; set; }

        public String Member { get { return Field; } }
        public ListSortDirection SortDirection { get { return !String.IsNullOrEmpty(Dir) && Dir.ToLower() == "asc" ? ListSortDirection.Ascending : ListSortDirection.Descending; } }
    }

    public interface IFilterData
    {

    }

    public class Filter : IFilterData
    {
        public Filter()
        {
            Filters = new List<Filter>();
        }

        public String Logic { get; set; }
        public String Field { get; set; }
        public String Operator { get; set; }
        public String Value { get; set; }
        public IList<Filter> Filters { get; set; }
    }

    public class SearchData : IFilterData
    {
        public String Search { get; set; }
        public List<String> Fields { get; set; }

        public SearchData()
        {
            Fields = new List<String>();
        }
    }

    /// <summary>
    /// ACL
    /// </summary>
    public class AccessTypeResponse
    {
        public Boolean List { get; set; }
        public Boolean View { get; set; }
        public Boolean Add { get; set; }
        public Boolean Edit { get; set; }
        public Boolean Remove { get; set; }
        public Boolean Search { get; set; }
    }
}
