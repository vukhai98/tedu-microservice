﻿using Shared.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Identityserver
{
    public interface ITokenService
    {
        TokenRespone GetToken(TokenRequest request);
    }
}
