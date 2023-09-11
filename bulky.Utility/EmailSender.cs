﻿using Microsoft.AspNetCore.Identity.UI.Services;

namespace bulky.Utility;

public class EmailSender : IEmailSender
{
  Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
  {
    return Task.CompletedTask;
    // throw new NotImplementedException();
  }
}
