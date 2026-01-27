using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryBot.DTOS_OpenAI
{
    public class OpenAiResponse
    {
        public string Id { get; set; }

        public List<OpenAiChoices> Choices { get; set; }
    }

    public class OpenAiChoices
    {
        public OpenAiChoicesMessage Message { get; set; }
    }

    public class OpenAiChoicesMessage
    {
        public string Content { get; set; }
    }
}
