using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostEffectiveCode.Backoffice.AspNet5.Web.Sample.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
