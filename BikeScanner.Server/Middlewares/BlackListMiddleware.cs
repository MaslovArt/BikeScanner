using System.Text.Json;
using BikeScanner.Application.Services.UsersService;
using Telegram.Bot.Types;

namespace BikeScanner.Server.Middlewares
{
    public static class BlackListMiddlewareExtension
    {
        public static IApplicationBuilder UseBlackList(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TelegramBotMiddleware>();
        }
    }

    public class BlackListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUsersService _usersService;

        public BlackListMiddleware(RequestDelegate next, IUsersService usersService)
        {
            _next = next;
            _usersService = usersService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();
            if (path == "/bike-scanner-telegram-handler")
            {
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                var update = JsonSerializer.Deserialize<Update>(body);
                var userId = GetUserId(update);
                var isBanned = await _usersService.IsBanned(userId);

                if (isBanned) return;
            }

            await _next.Invoke(context);
        }

        private long GetUserId(Update update)
        {
            return
                update.Message?.Chat?.Id ??
                update.CallbackQuery?.Message?.Chat?.Id ??
                update.MyChatMember?.Chat?.Id ??
                throw new Exception("Can't get telegram user id");
        }
    }
}

