using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BlyckBox
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateHostBuilder(args).Build();
            using (var services = host.Services.CreateScope())
            {
                var dbcontext = services.ServiceProvider.GetRequiredService<DataContext>();
                var usermgr = services.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var rolemgr = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                dbcontext.Database.Migrate();
                var adminrole = new IdentityRole("Admin");
                if (!dbcontext.Roles.Any())
                {
                    rolemgr.CreateAsync(adminrole).GetAwaiter().GetResult();
                }
                if (!dbcontext.Users.Any(u => u.UserName == "navaiz@blyckbox.com"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "navaiz@blyckbox.com",
                        EmailConfirmed = true,
                        Email = "navaiz@blyckbox.com"
                    };
                    var result = usermgr.CreateAsync(adminUser, "navaiz@admin").GetAwaiter().GetResult();
                    usermgr.AddToRoleAsync(adminUser, adminrole.Name).GetAwaiter().GetResult();
                }
                if (!dbcontext.Users.Any(u => u.UserName == "solutions@blyckbox.com"))
                {
                    var adminUser = new IdentityUser
                    {
                        UserName = "solutions@blyckbox.com",
                        EmailConfirmed = true,
                        Email = "solutions@blyckbox.com"
                    };
                    var result = usermgr.CreateAsync(adminUser, "sidhujuTT1").GetAwaiter().GetResult();
                    usermgr.AddToRoleAsync(adminUser, adminrole.Name).GetAwaiter().GetResult();
                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
