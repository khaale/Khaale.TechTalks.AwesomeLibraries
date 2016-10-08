using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BusinessServiceCore.Controllers
{
    [Route("api/[controller]")]
    public class ItemsController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new List<string>
            {
                "Item 1"
            };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Item 1";
        }

        [HttpPost]
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