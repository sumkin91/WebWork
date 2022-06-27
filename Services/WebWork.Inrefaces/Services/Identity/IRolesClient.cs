using Microsoft.AspNetCore.Identity;
using WebWork.Domain.Entities.Identity;

namespace WebWork.Intefaces.Services.Identity;

public interface IRolesClient : IRoleStore<Role>
{

}