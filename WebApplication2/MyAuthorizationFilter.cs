//using Hangfire.Annotations;
//using Hangfire.Dashboard;

//namespace WebApplication2
//{
//    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
//    {
//        public bool Authorize([NotNull] DashboardContext context)
//        {
//            var httpContext = context.GetHttpContext();
//            if (httpContext.User.Identity.IsAuthenticated)
//            {
//                // Kiểm tra quyền truy cập dựa trên thông tin người dùng
//                // Ví dụ: return httpContext.User.IsInRole("Admin");
//                return true; // Thay thế bằng logic xác thực của bạn
//            }
//            return false;
//        }
//    }
//}
