using Commerce.Amazon.Domain.Entities.CoreBase;
using Commerce.Amazon.Domain.Models;
using Commerce.Amazon.Domain.Models.Request.Auth;
using Commerce.Amazon.Domain.Models.Response.Auth;
using Commerce.Amazon.Web.Models;
using Microsoft.AspNetCore.Http;

namespace Commerce.Amazon.Web.ActionsProcess
{
    public class AuthenticationProcess : BaseActionProcess<AccountProcessRequest, AccountProcessModel>
	{

		public AuthenticationProcess(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
		{
		}

		protected override AccountProcessModel BuildModel(AccountProcessRequest request)
		{
			AccountProcessModel accountProcessModel = new AccountProcessModel();
			ProfileModel profile = GetProfile();
			try
			{

				if (request.AuthenticationRequest != null)
				{
					accountProcessModel.BaseViewModel = Authenticate(request.AuthenticationRequest);
				}
				if (request.SendLinkEmailRequest != null)
				{
					//accountProcessModel.SendLinkEmailResponse = silicieClient.SendLinkEmail(request.SendLinkEmailRequest).GetAwaiter().GetResult();
				}
				if (request.CheckLinkResetCodeRequest != null)
				{
					//accountProcessModel.CheckLinkResetCodeResponse = silicieClient.CheckLinkResetCode(request.CheckLinkResetCodeRequest).GetAwaiter().GetResult();
				}
				if (request.ResetPasswordRequest != null)
				{
					accountProcessModel.ResetPasswordResponse = ResetPassword(request.ResetPasswordRequest);
				}
				if (request.RegisterRequest != null)
				{
					if (profile != null)
					{
						request.RegisterRequest.IdSociete = profile.IdSociete;

						//accountProcessModel.RegisterResponse = silicieClient.Register(request.RegisterRequest, profile.Token).GetAwaiter().GetResult();
						
					}
					else
					{
						accountProcessModel.NoToken = true;
					}
					
				}
			}
			catch (System.Exception ex)
			{
				//LogWriter.LogWrite($"AccountProcess {ex}");
			}
			return accountProcessModel;
		}

		private ResetPasswordResponse ResetPassword(ResetPasswordRequest request)
		{
			ResetPasswordResponse response = null;// silicieClient.ResetPassword(request).GetAwaiter().GetResult();
			return response;
		}

		private BaseViewModel Authenticate(AuthenticationRequest authenticationRequest)
		{
			TResult<AuthenticationResponse> result = null;// silicieClient.Authenticate(authenticationRequest).GetAwaiter().GetResult();
			BaseViewModel authenticationResponse;
			if (result == null || result.Result == null || result.Status == StatusResponse.KO)
			{
				authenticationResponse = new BaseViewModel
				{
					Status = StatusResponse.KO,
					Message = string.IsNullOrEmpty(result?.Message) ? "Se ha producido un error al authenticacion" : result.Message
				};
			}
			else
			{
				ProfileModel profile = BuildProfile(result.Result);

				authenticationResponse = new BaseViewModel
				{
					Status = StatusResponse.OK,
					Message = result?.Message,
					ProfileModel = profile
				};
			}
			return authenticationResponse;
		}

	}
}