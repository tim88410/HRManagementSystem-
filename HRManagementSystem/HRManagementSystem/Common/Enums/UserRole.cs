using System.ComponentModel.DataAnnotations;

namespace HRManagementSystem.Common.Enums
{
    public enum UserRole
    {
        [Display(Name = "使用者")]
        User = 0,

        [Display(Name = "管理員")]
        Admin = 1,

        [Display(Name = "人事")]
        HR = 2
    }
}
