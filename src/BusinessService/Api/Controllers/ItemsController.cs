using System.Collections.Generic;
using System.Web.Http;

namespace Khaale.TechTalks.AwesomeLibs.BusinessService.Api.Controllers
{
    public class ItemsController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new List<string>
            {
                "Item 1"
            };
        }

        public string Get(int id)
        {
            return "Item 1";
        }

        public ItemModel Update(ItemModel input)
        {
            return input;
        }
    }

    public class ItemModel
    {
        public string Prop1 { get; set; }
        public IEnumerable<SubItemModel> InnerItems { get; set; }
        public MyEnum Enum { get; set; }
    }

    public enum MyEnum
    {
        Value1,
        Value2
    }

    public class SubItemModel
    {
        public decimal Value { get; set; }
    }
}
