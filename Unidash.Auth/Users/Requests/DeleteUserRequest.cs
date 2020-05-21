using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EnsureThat;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Unidash.Auth.Users.Requests
{
    public class DeleteUserRequest : IRequest<IActionResult>
    {
        public string Password { get; set; }

        public DeleteUserRequest()
        {
        }

        public DeleteUserRequest(string password)
        {
            Password = password;
        }
    }
}
