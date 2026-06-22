using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MediCheck.Api.Data;

namespace MediCheck.Api.Authorization
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequirePermissionAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _maQuyen;

        public RequirePermissionAttribute(string maQuyen)
        {
            _maQuyen = maQuyen;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user == null || !user.Identity!.IsAuthenticated)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedObjectResult("Chưa đăng nhập.");
                return;
            }

            var tenVaiTro = user.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(tenVaiTro))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
                return;
            }

            var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

            bool isAllowed = await dbContext.VaiTroQuyens
                .Include(vq => vq.VaiTro)
                .Include(vq => vq.Quyen)
                .AnyAsync(vq => vq.VaiTro.TenVaiTro == tenVaiTro && vq.Quyen.MaQuyen == _maQuyen);

            if (!isAllowed)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(new
                {
                    message = $"Tài khoản với vai trò '{tenVaiTro}' không có quyền '{_maQuyen}' để thực hiện hành động này."
                })
                { StatusCode = 403 };
            }
        }
    }
}
