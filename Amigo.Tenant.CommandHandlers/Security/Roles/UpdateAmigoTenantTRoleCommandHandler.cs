using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amigo.Tenant.CommandHandlers.Abstract;
using Amigo.Tenant.CommandHandlers.Common;
using Amigo.Tenant.CommandModel.Models;
using Amigo.Tenant.Commands.Common;
using Amigo.Tenant.Commands.Security.Permission;
using Amigo.Tenant.Commands.Security.AmigoTenantTRole;
using Amigo.Tenant.Events.Security;
using Amigo.Tenant.Infrastructure.EventSourcing.Abstract;
using Amigo.Tenant.Infrastructure.Mapping.Abstract;
using Amigo.Tenant.Infrastructure.Persistence.Abstract;

namespace Amigo.Tenant.CommandHandlers.Security.Roles
{
    public class UpdateAmigoTenantTRoleCommandHandler : IAsyncCommandHandler<UpdateAmigoTenantTRoleCommand>    
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<AmigoTenantTRole> _repository;
        private readonly IRepository<Permission> _permissionRepository;

        public UpdateAmigoTenantTRoleCommandHandler(
            IBus bus, 
            IMapper mapper,
            IRepository<AmigoTenantTRole> repository,
            IRepository<Permission> permissionRepository,
            IUnitOfWork unitOfWork)
        {
            _bus = bus;
            _mapper = mapper;
            _repository = repository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(UpdateAmigoTenantTRoleCommand message)
        {

            //UPDATING PERMISSIONS
            //var existInUI = message.Permissions.ToList();
            //foreach (var item in existInUI.Where(q => q.EntityStatus != Application.DTOs.Requests.Common.ObjectStatus.Unchanged))
            //{
            //    if (item.EntityStatus == Application.DTOs.Requests.Common.ObjectStatus.Added)
            //    {
            //        var entityPermission = _mapper.Map<PermissionCommand, Permission>(item);
            //        _permissionRepository.Add(entityPermission);
            //    }
            //    else if (item.EntityStatus == Application.DTOs.Requests.Common.ObjectStatus.Modified)
            //    {
            //        var entityPermission = _mapper.Map<PermissionCommand, Permission>(item);
            //        _permissionRepository.Update(entityPermission);
            //    }
            //    else if (item.EntityStatus == Application.DTOs.Requests.Common.ObjectStatus.Deleted)
            //    {
            //        var entityPermission = _mapper.Map<PermissionCommand, Permission>(item);
            //        _permissionRepository.Delete(entityPermission);
            //    }
            //}

            //UPDATING ROLE
            //message.Permissions = null;
            var entity = _mapper.Map<UpdateAmigoTenantTRoleCommand, AmigoTenantTRole>(message);
            _repository.Update(entity);

            //COMMITING THE OPERATIONS
            await _unitOfWork.CommitAsync();

            await _bus.PublishAsync(new RoleEditionEvent(entity.Code, RoleEditionEventType.Update));

            return entity.ToResult();
        }

        //public async Task<CommandResult> Handle(UpdateAmigoTenantTRoleCommand message)
        //{
        //    var existInDB = await _permissionRepository.ListAsync(w => w.AmigoTenantTRoleId == message.AmigoTenantTRoleId);
        //    var existInUI = message.Permissions.ToList();


        //    foreach (var item in existInUI.Where(q => q.EntityStatus != Application.DTOs.Requests.Common.ObjectStatus.Unchanged))
        //    {
        //        if (item.EntityStatus == Application.DTOs.Requests.Common.ObjectStatus.Added)
        //        {
        //            var permissions = existInUI.Where(i => !existInDB.Select(s => s.ActionId).Contains(i.ActionId));
        //            var entityPermission = _mapper.Map<List<PermissionCommand>, List<Permission>>(permissions.ToList());
        //            entityPermission.ToList().ForEach(p => _permissionRepository.Add(p));
        //        }
        //        else if (item.EntityStatus == Application.DTOs.Requests.Common.ObjectStatus.Modified)
        //        {
        //        }
        //        else if (item.EntityStatus == Application.DTOs.Requests.Common.ObjectStatus.Deleted)
        //        {
        //            var permissions = existInDB.Where(i => !existInUI.Select(s => s.ActionId).Contains(i.ActionId.Value));
        //            permissions.ToList().ForEach(p => _permissionRepository.Delete(p));
        //        }

        //    }

        //    //ExistInDB and NO ExistInUI -- Delete
        //    DeletePermissions(existInDB.ToList(), existInUI);

        //    //ExistInUI and NO ExistInDB -- Insert
        //    InsertPermissions(existInDB.ToList(), existInUI);

        //    message.Permissions = null;
        //    var entity = _mapper.Map<UpdateAmigoTenantTRoleCommand, AmigoTenantTRole>(message);

        //    _repository.Update(entity);

        //    await _unitOfWork.CommitAsync();
        //    return entity.ToResult();
        //}

        private void InsertPermissions(List<Permission> existInDB, List<PermissionCommand> existInUI)
        {
            var permissions = existInUI.Where(i => !existInDB.Select(s=>s.ActionId).Contains(i.ActionId));
            var entityPermission = _mapper.Map<List<PermissionCommand>, List<Permission>>(permissions.ToList());
            entityPermission.ToList().ForEach(p => _permissionRepository.Add(p));
        }

        private void DeletePermissions(List<Permission> existInDB, List<PermissionCommand> existInUI)
        {
            var permissions = existInDB.Where(i => !existInUI.Select(s => s.ActionId).Contains(i.ActionId.Value));
            permissions.ToList().ForEach(p => _permissionRepository.Delete(p));
        }
    }
}

