using Microsoft.AspNetCore.Identity;
using WebWork.Domain.Entities.Identity;

namespace WebWork.Intefaces.Services.Identity;

public interface IUsersClient :
    IUserRoleStore<User>,
    IUserPasswordStore<User>,
    IUserEmailStore<User>,
    IUserPhoneNumberStore<User>,
    IUserTwoFactorStore<User>,
    IUserLoginStore<User>,
    IUserClaimStore<User>
{

}
