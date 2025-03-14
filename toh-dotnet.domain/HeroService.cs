﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tohdotnet.domain.Models;

namespace tohdotnet.domain
{
    public interface IHeroService
    {
        Task<List<Hero>> GetHeros();
        Task<Hero> GetHero(int id);
        Task<List<Hero>> SearchHeros(string name);

        Task<Hero> CreateHero(Hero hero);

        Task DeleteHero(Hero hero);

        Task UpdateHero(int id, Hero hero);
    }
    public class HeroService : IHeroService
    {
        private readonly tohdotnetContext context;

        public HeroService(tohdotnetContext _context)
        {
            context = _context;
        }
        public async Task<Hero> CreateHero(Hero hero)
        {
            context.Heroes.Add(hero);
            await context.SaveChangesAsync();
            return hero;
        }

        public async Task DeleteHero(Hero hero)
        {
            context.Heroes.Remove(hero);
            await context.SaveChangesAsync();
        }

        public async Task<Hero> GetHero(int id)
        {
            return await context.Heroes.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Hero>> GetHeros()
        {
            return await context.Heroes.ToListAsync();
        }

        public async Task<List<Hero>> SearchHeros(string name)
        {
            var heros = await context.Heroes.Where(m => m.Name.Contains(name)).ToListAsync();
            return heros;
        }

        public async Task UpdateHero(int id, Hero hero)
        {
            context.Entry(hero).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
