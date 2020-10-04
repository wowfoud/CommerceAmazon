using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models.Response.Auth;
using Commerce.Amazon.Domain.Models.Response.Base;

namespace Commerce.Amazon.Domain.Models
{
    public class AccountProcessModel : ModelBase
    {
        public BaseViewModel BaseViewModel { get; set; }
        public SendLinkEmailResponse SendLinkEmailResponse { get; set; }
        public CheckLinkResetCodeResponse CheckLinkResetCodeResponse { get; set; }
        public ResetPasswordResponse ResetPasswordResponse { get; set; }
        public TResult<int> RegisterResponse { get; set; }
    }
}