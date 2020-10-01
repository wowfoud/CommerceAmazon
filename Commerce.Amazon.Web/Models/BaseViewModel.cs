using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Response.Base;

namespace Commerce.Amazon.Web.Models
{
    public class BaseViewModel : ModelBase
    {
        public ProfileModel ProfileModel { get; set; }
    }
}