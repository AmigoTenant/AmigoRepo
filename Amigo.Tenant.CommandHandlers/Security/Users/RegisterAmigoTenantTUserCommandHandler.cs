using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandHandlers.Extensions;
using Amigo.Tenant.CommandModel.BussinesEvents.Security;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.AmigoTenantTUsers;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Users
{
    public class RegisterAmigoTenantTUserCommandHandler : IAsyncCommandHandler<RegisterAmigoTenantTUserCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTUser> _repository;

        //public ISClientSettings IdentityServerClientSettings { get; set; }

        public RegisterAmigoTenantTUserCommandHandler(
            IBus bus, 
            IMapper mapper,
            IRepository<AmigoTenantTUser> repository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }                

        public async Task<CommandResult> Handle(RegisterAmigoTenantTUserCommand message)
        {
            //Validate using domain models
            var entity = _mapper.Map<RegisterAmigoTenantTUserCommand, AmigoTenantTUser>(message);
            var alreadyExists = await _repository.ExistsByUserName(message.Username);
            if (alreadyExists) entity.AddError("A User already exists for this driver.");

            //var httpClient1 = ISHttpClient.GetClient(IdentityServerClientSettings)
            
            //using (var httpClient = ISHttpClient.GetClient(IdentityServerClientSettings))
            //{
            //    var response = await httpClient.PostAsJsonAsync("api/Users", message);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var content = await response.Content.ReadAsAsync<ResponseDTO<UserResponse>>();
            //        message.AmigoTenantTUserId = content.Data.Id;
            //    }
            //    else
            //    {
            //        throw new Exception("Amigo.Tenant.Application.Services.Security - AmigoTenantTUserApplicationService - SearchUsersByCriteriaAsync - call to IdentityServerHttpClient api/Users/GetUsersDetails was not successful");
            //    }
            //}


            //if is not valid
            if (entity.HasErrors) return entity.ToResult();

            //Insert
            _repository.Add(entity);
            await _unitOfWork.CommitAsync();

            //Publish bussines Event
            await _bus.PublishAsync(new AmigoTenantTUserRegistered() { AmigoTenantTUserId = entity.AmigoTenantTUserId });

            //Return result
            return entity.ToRegisterdResult().WithId(entity.AmigoTenantTUserId);
        }
    }
}
