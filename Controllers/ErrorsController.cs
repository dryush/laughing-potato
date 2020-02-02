using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailBank.Filters.StatusToBody;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mail_bank.Controllers
{
    [Route("errors")]
    [ApiController]
    public class ErrorsController : ControllerBase
    {
        [Route("{status:int}")]
        [AllowAnonymous]
        public BodyStatusAnswer Get(int status)
        {
            return BodyStatusAnswer.Empty(status);
        }
    }
}