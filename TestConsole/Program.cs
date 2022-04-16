// See https://aka.ms/new-console-template for more information
using ClarityVentures.Emailer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

var builder = new ConfigurationBuilder()
      .SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

IConfigurationRoot config = builder.Build();

EmailProviderSmtp emailer = new(config, new NullLogger<EmailProviderSmtp>());

var success = await emailer.SendEmail(config["email_login"], "greggat96@gmail.com", "test", "test body");

Console.WriteLine(success);

