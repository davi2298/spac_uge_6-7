using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lagersystem.Utilitys;
using Microsoft.AspNetCore.Mvc;

namespace Lagersystem.API;

[ApiController]
[Route("api/[controller]")]
public class ExapleController : ControllerBase
{
    private LagerContext LagerContext{ get; init;}
    public ExapleController(LagerContext lagerContext){
        LagerContext = lagerContext;
    }
}
