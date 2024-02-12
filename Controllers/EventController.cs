using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Security.Claims;
using Homies.Data;
using Homies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Homies.Controllers;

[Authorize]
public class EventController : Controller
{
    private readonly HomiesDbContext data;

    public EventController(HomiesDbContext _data)
    {
        data = _data;
    }
    // GET
    public async Task<IActionResult> ALl()
    {
        var events = await data.Events
            .AsNoTracking()
            .Select(e => new EventViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Start = e.Start.ToString(DataConstants.DateFormat),
                Organiser = e.Organiser.UserName,
                Type = e.Type.Name
            })
            .ToListAsync();
        
        return View(events);
    }
    [HttpPost]
    public async Task<IActionResult> Join(int id)
    {
        var e = await data.Events
            .Where(e => e.Id == id)
            .Include(e => e.EventsParticipants)
            .FirstOrDefaultAsync();

        if (e == null)
        {
            BadRequest();
        }

        string userId = GetUserId();
        
        if (!e.EventsParticipants.Any(ep => ep.HelperId == userId))
        {
            e.EventsParticipants.Add(new EventParticipant()
            {
                EventId = id,
                HelperId = userId
            });

            await data.SaveChangesAsync();
        }
        return RedirectToAction("Joined");
    }
    
    [HttpGet]
    public async Task<IActionResult> Joined()
    {
        string userId = GetUserId();

        var joinedEvents = data.EventsParticipants
            .Where(ep => ep.HelperId == userId)
            .Select(e => new EventViewModel()
            {
                Id = e.Event.Id,
                Name = e.Event.Name,
                Start = e.Event.Start.ToString(DataConstants.DateFormat),
                Type = e.Event.Type.Name,
                Organiser = userId
            });

        return View(joinedEvents);
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var model = new EventFormViewModel();
        model.Types = await GetTypes();
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(EventFormViewModel model)
    {
        DateTime start = DateTime.Now;
        DateTime end = DateTime.Now;

        if(!DateTime.TryParseExact(
               model.Start,
               DataConstants.DateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out start))
        {
            ModelState.AddModelError(nameof(model.Start), $"Invalid Date! Format must be: {DataConstants.DateFormat}");
        }
        
        if(!DateTime.TryParseExact(model.End,
               DataConstants.DateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out end))
        {
            ModelState.AddModelError(nameof(model.End), $"Invalid Date! Format must be: {DataConstants.DateFormat}");
        }
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        Event e = new Event()
        {
            Name = model.Name,
            Description = model.Description,
            CreatedOn = DateTime.Now,
            Start = start,
            End = end,
            TypeId = model.TypeId,
            OrganiserId = GetUserId()
        };

        await data.Events.AddAsync(e);
        await data.SaveChangesAsync();

        return RedirectToAction("All");
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var e = await data.Events
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EventDetailsViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                CreatedOn = e.CreatedOn.ToString(DataConstants.DateFormat),
                End = e.End.ToString(DataConstants.DateFormat),
                Organiser = e.Organiser.UserName,
                Start = e.Start.ToString(DataConstants.DateFormat),
                Type = e.Type.Name
            })
            .FirstOrDefaultAsync();

        if (e == null)
        {
            BadRequest();
        }

        if (!User.Identity.IsAuthenticated)
        {
            Unauthorized();
        }

        return View(e);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var e = await data.Events.FindAsync(id);
        
        if (e == null)
        {
            return BadRequest();
        }
        if (e.OrganiserId != GetUserId())
        {
            return Unauthorized();
        }

        var model = new EventFormViewModel()
        {
            Name = e.Name,
            Description = e.Description,
            End = e.End.ToString(DataConstants.DateFormat),
            Start = e.Start.ToString(DataConstants.DateFormat),
            TypeId = e.TypeId
        };

        model.Types = await GetTypes();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EventFormViewModel model)
    {
        var e = await data.Events.FindAsync(model.Id);
        
        if (e == null)
        {
            return BadRequest();
        }
        if (e.OrganiserId != GetUserId())
        {
            return Unauthorized();
        }
        
        DateTime start = DateTime.Now;
        DateTime end = DateTime.Now;

        if(!DateTime.TryParseExact(
               model.Start,
               DataConstants.DateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out start))
        {
            ModelState.AddModelError(nameof(model.Start), $"Invalid Date! Format must be: {DataConstants.DateFormat}");
        }
        
        if(!DateTime.TryParseExact(model.End,
               DataConstants.DateFormat,
               CultureInfo.InvariantCulture,
               DateTimeStyles.None,
               out end))
        {
            ModelState.AddModelError(nameof(model.End), $"Invalid Date! Format must be: {DataConstants.DateFormat}");
        }
        
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        e.Name = model.Name;
        e.Description = model.Description;
        e.Start = start;
        e.End = end;
        e.TypeId = model.TypeId;

        await data.SaveChangesAsync();

        return RedirectToAction("ALl");
    }

    public async Task<IActionResult> Leave(int id)
    {
        var e = await data.Events
            .Where(e => e.Id == id)
            .Include(e => e.EventsParticipants)
            .FirstOrDefaultAsync();

        if (e == null)
        {
            return BadRequest();
        }
        
        var ep = e.EventsParticipants
            .FirstOrDefault(ep => ep.HelperId == GetUserId());
        
        if (ep == null)
        {
            return BadRequest();
        }

        e.EventsParticipants.Remove(ep);

        await data.SaveChangesAsync();

        return RedirectToAction("ALl");
    }

    private string GetUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    private async Task<IEnumerable<TypeViewModel>> GetTypes()
    {
        return await data.Types.AsNoTracking()
            .Select(t => new TypeViewModel()
            {
                Id = t.Id,
                Name = t.Name
            })
            .ToListAsync();
    }
    
}