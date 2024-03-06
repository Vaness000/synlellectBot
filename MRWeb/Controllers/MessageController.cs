using BotManager.Commands;
using BotManager.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MRWeb.Controllers
{
    [Route("api/message/update")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] object body)
        {
            Update update = JsonConvert.DeserializeObject<Update>(body.ToString());
            if (update.Type == UpdateType.Message && update.Message.Type == MessageType.Text)
            {
                Command command = Command.ParseCommand(update.Message, out CommandData commandData);

                if (command != null)
                {
                    try
                    {
                        await command.ExecuteAsync(BotClient.Instance, update.Message.Chat, commandData);
                    }
                    catch(Exception e)
                    {
                        Logger.Log(LogType.Error, e.Message);
                    }
                }
            }

            return Ok();
        }
    }
}
