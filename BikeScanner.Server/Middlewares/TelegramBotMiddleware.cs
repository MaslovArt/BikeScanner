using System.Text.Json;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using TelegramBot.UI.Bot;

namespace BikeScanner.Server.Middlewares
{
    public static class TelegramBotMiddlewareExtension
    {
        public static IApplicationBuilder UseTelegramBotHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<TelegramBotMiddleware>();
        }
    }

    public class TelegramBotMiddleware
	{
        private readonly RequestDelegate _next;
        private readonly BikeScannerBot _bot;

        public TelegramBotMiddleware(RequestDelegate next, BikeScannerBot bot)
        {
            _next = next;
            _bot = bot;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string path = context.Request.Path.Value.ToLower();
            if (path.StartsWith("/bike-scanner-telegram-handler"))
            {
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                //var update = JsonSerializer.Deserialize<Update>(body);
                var update = JsonConvert.DeserializeObject<Update>(body);
                await _bot.Handle(update);
            }

            await _next.Invoke(context);
        }
    }
}

