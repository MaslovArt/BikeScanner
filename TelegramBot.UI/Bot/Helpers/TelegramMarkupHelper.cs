using System.Collections.Generic;
using System.Linq;
using BikeScanner.Domain.Extensions;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.UI.Bot.Helpers
{
    /// <summary>
    /// Telegram markup generation
    /// </summary>
	public static class TelegramMarkupHelper
	{
        /// <summary>
        /// Return direction row buttons for keyboard
        /// </summary>
        /// <param name="btns">Array of btns (same text and callback)</param>
        /// <param name="autoHide">Hide keyboard after first usage</param>
        /// <returns></returns>
        public static ReplyKeyboardMarkup KeyboardRowBtns(string[] btns, bool autoHide = false)
        {
            var buttons = new List<List<KeyboardButton>>
            {
                btns.Select(o => new KeyboardButton(o)).ToList()
            };
            return new ReplyKeyboardMarkup(buttons)
            {
                OneTimeKeyboard = autoHide
            };
        }

        /// <summary>
        /// Return direction row buttons for message
        /// </summary>
        /// <param name="btns">Array of btns (same text and callback)</param>
        /// <returns></returns>
		public static InlineKeyboardMarkup MessageRowBtns(params string[] btns)
        {
            var buttons = btns
                .ToMax64BytesValue()
                .Select(btn => InlineKeyboardButton.WithCallbackData(btn));
            return new InlineKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Return direction row buttons for message
        /// </summary>
        /// <param name="btns">Array of btns (btn text, btn callback)</param>
        /// <returns></returns>
        public static InlineKeyboardMarkup MessageRowBtns(params (string text, string callback)[] btns)
        {
            var buttons = btns
                .Select(btn => InlineKeyboardButton.WithCallbackData(
                    btn.text.ToMax64BytesValue(),
                    btn.callback.ToMax64BytesValue())
                );
            return new InlineKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Return direction column buttons for message
        /// </summary>
        /// <param name="btns">Array of btns (same text and callback)</param>
        /// <returns></returns>
        public static InlineKeyboardMarkup MessageColumnBtns(params string[] btns)
        {
            var buttons = btns
                .ToMax64BytesValue()
                .Select(btn => new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(btn)
                });
            return new InlineKeyboardMarkup(buttons);
        }

        /// <summary>
        /// Return direction column buttons for message
        /// </summary>
        /// <param name="btns">Array of btns (btn text, btn callback)</param>
        /// <returns></returns>
        public static InlineKeyboardMarkup MessageColumnBtns(params (string text, string callback)[] btns)
        {
            var buttons = btns
                .Select(btn => new List<InlineKeyboardButton>()
                {
                    InlineKeyboardButton.WithCallbackData(
                        btn.text.ToMax64BytesValue(),
                        btn.callback.ToMax64BytesValue())
                });
            return new InlineKeyboardMarkup(buttons);
        }
    }
}
