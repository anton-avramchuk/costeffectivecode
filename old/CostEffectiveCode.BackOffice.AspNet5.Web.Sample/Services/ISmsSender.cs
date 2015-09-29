using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostEffectiveCode.BackOffice.AspNet5.Web.Sample.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
