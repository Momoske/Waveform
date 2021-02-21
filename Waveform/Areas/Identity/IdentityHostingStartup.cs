using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Waveform.Areas.Identity.Data;
using Waveform.Data;

[assembly: HostingStartup(typeof(Waveform.Areas.Identity.IdentityHostingStartup))]
namespace Waveform.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WaveformContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WaveformContextConnection")));

                services.AddDefaultIdentity<WaveformUser>(options => options.SignIn.RequireConfirmedAccount = false)
                    .AddEntityFrameworkStores<WaveformContext>();
            });
        }
    }
}