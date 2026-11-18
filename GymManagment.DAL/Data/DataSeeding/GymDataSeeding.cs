using GymManagment.DAL.Data.DbContexts;
using GymManagment.DAL.Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

namespace GymManagment.DAL.Data.DataSeeding
{
    public class GymDataSeeding
    {
        public static async Task SeedAsync( GymDbContext dbContext ,  string FolderPath , ILogger logger , CancellationToken ct = default)
        {
			try
			{
                if(!dbContext.plans.Any())
                {
                    var data = LoadJsonData<Plan>(FolderPath, "plans.json");
                    if(data.Any())
                    {
                        dbContext.plans.AddRange(data);
                        logger.LogInformation($"Plans Seeded With Count {data.Count}");
                    }

                    if(dbContext.ChangeTracker.HasChanges())
                    {
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        logger.LogInformation($"Plan Already Seeded");
                    }

                }
			}
			catch(Exception ex)
			{
                logger.LogError(ex, "Gym data Seeding Failed");
                throw;
			}
        }


        private static List<T> LoadJsonData<T>(string FolderPath , string FileName)
        {
            var filepath = Path.Combine(FolderPath, FileName);

            if(!File.Exists(filepath))
            {
                throw new FileNotFoundException($"Seed Data File Not Found : {filepath}");
            }

            var data = File.ReadAllText(filepath);
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

          return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
        }
    }
}
