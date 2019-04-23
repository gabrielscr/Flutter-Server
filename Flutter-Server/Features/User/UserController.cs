using FlutterServer.Features.User;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flutter.Server.Features.User
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(InsertEdit.Query query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpGet]
        public async Task<IActionResult> List(List.Query query)
        {
            return Json(await _mediator.Send(query));
        }

        [HttpPost]
        public async Task Insert([FromBody]InsertEdit.Command command)
        {
            await _mediator.Send(command);
        }

        [HttpPut]
        public async Task Edit([FromBody]InsertEdit.Command command)
        {
            await _mediator.Send(command);
        }

        [HttpDelete]
        public async Task Delete(Delete.Command command)
        {
            await _mediator.Send(command);
        }
    }

}
