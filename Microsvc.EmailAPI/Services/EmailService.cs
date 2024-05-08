using Microsvc.Services.EmailAPI.Models.Dto;
using System.Text;

namespace Microsvc.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        public EmailService()
        {
            
        }
        public Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/> Cart Email Requested");
            message.AppendLine("<br/> Total" + cartDto.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul/>");
            foreach(var item in cartDto.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("<li>");
            }
            message.Append("<ul>");
            return Task.FromResult(message.ToString());
        } 
    }
}
