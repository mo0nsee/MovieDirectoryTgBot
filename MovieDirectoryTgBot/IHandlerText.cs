using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieDirectoryTgBot
{
    internal interface IHandlerText
    {
        /// <summary>
        /// Обработка команд в виде текста
        /// </summary>
        Task HandlerCommand();
    }
}
